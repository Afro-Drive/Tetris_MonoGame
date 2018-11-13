using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Device;
using PersonalProduct_2nd.Utility;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// Blockクラスを継承した操作可能テトリスブロッククラス
    /// 作成者:谷永吾
    /// 作成開始日:2018年11月7日
    /// </summary>
    class Tetrimino : Block
    {
        #region フィールド
        /// <summary>
        /// テトリミノの種類列挙型
        /// </summary>
        internal enum Form_mino
        {
            I = 0, T, J, L, S, Z, Test
        }
        private Form_mino form;
        private Cell[,] realRotate_Array; //回転処理用配列
        private Cell[,] imageRotate_Array; //回転可能か検証する用の配列

        private float GRAVITY = 5f; //重力
        private SoundManager sound; //動作音用
        private IGameMediator mediator; //ゲーム仲介者
        #endregion フィールド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mediator">スコア加算用ゲーム仲介者</param>
        public Tetrimino(IGameMediator mediator)
            : base()
        {
            sound = DeviceManager.CreateInstance().GetSound();
            this.mediator = mediator;

            form = Form_mino.Test; //テトリミノの形の設定

            Initialize(); //各種変数の初期化
        }

        /// <summary>
        /// TetriminoFactoryクラス内で使用するコンストラクタ
        /// (Minoの方を指定する)
        /// </summary>
        /// <param name="setForm">Minoの形の指定</param>
        public Tetrimino(Form_mino setForm, IGameMediator mediator)
            : base()
        {
            sound = DeviceManager.CreateInstance().GetSound();
            this.mediator = mediator;

            form = setForm; //ここで引数に基づいて型を決定する
            Initialize();　//各種変数の初期化
        }

        /// <summary>
        /// テトリミノの形の取得プロパティ
        /// </summary>
        public Form_mino Form
        {
            get { return Form; }
        }

        /// <summary>
        /// テトリミノのFormの値に合わせた描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer)
        {
            switch (form)
            {
                case Form_mino.I:
                    DrawI(renderer);
                    break;

                case Form_mino.J:
                    DrawJ(renderer);
                    break;

                case Form_mino.L:
                    DrawL(renderer);
                    break;

                case Form_mino.S:
                    DrawS(renderer);
                    break;

                case Form_mino.T:
                    DrawT(renderer);
                    break;

                case Form_mino.Z:
                    DrawZ(renderer);
                    break;

                default: //テスト用テトリミノ描画
                    DrawO(renderer);
                    break;
            }
        }
        #region テトリミノごとの描画
        public void DrawI(Renderer renderer)
        {

        }

        public void DrawT(Renderer renderer)
        {

        }

        public void DrawJ(Renderer renderer)
        {

        }

        public void DrawL(Renderer renderer)
        {

        }

        public void DrawS(Renderer renderer)
        {

        }

        public void DrawZ(Renderer renderer)
        {

        }

        public void DrawO(Renderer renderer)
        {
            //Mino_Arrayのリストのうち、2の要素の時のみピクセルを表示
            int[,] data = Mino_Array.mino_Data[(int)form]; //List内で使用する二次元配列を取得
            for (int y = 0; y < data.GetLength(0); y++)
            {
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    if (data[y, x] == 2)
                    {
                        realRotate_Array[y, x] = new Block();
                        ((Block)realRotate_Array[y, x]).DrawMino(
                            renderer,
                            "Omino",
                            position + new Vector2(HEIGHT * y, WIDTH * x),
                            (float)WIDTH);
                        #region renderer.DrawTexture(→Blockクラスの描画処理に委託
                        //    "Omino",
                        //    position + new Vector2(HEIGHT * y, WIDTH * x),
                        //    null,
                        //    (float)WIDTH,
                        //    Vector2.Zero);
                        #endregion
                    }
                    else
                    {
                        realRotate_Array[y, x] = new Space();
                        ((Space)realRotate_Array[y, x]).Draw(renderer);
                    }
                }
            }
        }
        #endregion テトリミノごとの描画

        /// <summary>
        ///  初期化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //各種回転用の配列を初期化
            realRotate_Array = new Cell[4, 4]; //4×4を基本単位とする
            imageRotate_Array =　　　　　　　　//回転検証用は実際に描画等で扱う配列を複製する
                new Cell[
                    realRotate_Array.GetLength(0),
                    realRotate_Array.GetLength(1)];

            position = new Vector2(WIDTH * 10, 0); //X座標が大体フィールドの真ん中らへんに来るように設定
            IsOnLand = false; //Blockと異なりTetriminoでは偽にする
        }

        public override void Update(GameTime gameTime)
        {
            MoveX();
            MoveY(gameTime);
        }

        /// <summary>
        /// 二次元配列を時計回りに回転
        /// </summary>
        /// <param name="basedArray">回転したい配列</param>
        /// <returns>時計回りに90°回転させた二次元配列</returns>
        public Cell[,] Rotate_Clockwise(Cell[,] basedArray)
        {
            int rows = basedArray.GetLength(0); //列(横)
            int cols = basedArray.GetLength(1); //行(縦)
            imageRotate_Array = new Cell[cols, rows]; //回転前とは行と列を逆にした配列を生成

            for (int y = 0; y < rows; y++) //回転後の配列の列に回転前の配列の行分要素を用意
            {
                for (int x = 0; x < cols; x++) //回転後の配列の行に回転前の配列の列分要素を用意
                {
                    //回転後の配列の列は、回転前の配列の行からy(新規配列の列の生成回数)と1を引いたものに一致する
                    //知るかこの野郎
                    imageRotate_Array[x, rows - y - 1] = basedArray[y, x];
                }
            }
            return imageRotate_Array;
        }

        /// <summary>
        /// 二次元配列を反時計回りに回転
        /// </summary>
        /// <param name="basedArray">回転したい二次元配列</param>
        /// <returns>反時計回りに90°回転させた二次元配列</returns>
        public Cell[,] Rotate_AntiClockwise(Cell[,] basedArray)
        {
            int rows = basedArray.GetLength(0); //行(横)
            int cols = basedArray.GetLength(1); //列(縦)
            imageRotate_Array = new Cell[cols, rows];

            for (int y = 0; y < rows; y++) //回転後の配列の列に回転前の配列の行分だけ要素を用意
            {
                for (int x = 0; x < cols; x++) //回転後の配列の行に回転前の配列の列分だけ要素を用意
                {
                    //回転後の配列の列は回転前の配列の行からx(回転後の列を用意した回数)と1を引いたものに一致する
                    //時計回りとはややこしくなる方が逆になるんだね。知らんけど。
                    imageRotate_Array[rows - x - 1, y] = basedArray[y, x];
                }
            }

            return imageRotate_Array;
        }

        /// <summary>
        /// 横移動
        /// </summary>
        public void MoveX()
        {
            if (Input.IsKeyDown(Keys.Left))
            {
                position.X -= WIDTH;
            }
            if (Input.IsKeyDown(Keys.Right))
            {
                position.X += WIDTH;
            }
        }

        /// <summary>
        /// 縦(下方向のみ)移動
        /// </summary>
        /// <param name="gameTime"></param>
        public void MoveY(GameTime gameTime)
        {
            fallTimer.Update(gameTime);
            if (fallTimer.TimeUP()) //専用タイマーが時間切れになったら1マス分落下
            {
                position.Y += Height;
                fallTimer.Initialize();
            }

            if (Input.IsKeyDown(Keys.Down))
            {
                position.Y += GRAVITY;
            }
        }

        public override void Hit(Cell other)
        {
            if(other is Block) //衝突対象がブロック
            {
                //TetriminoとBlockの衝突処理
                hitBlock(other);
            }
        }

        private void hitBlock(Cell cell)
        {
            //衝突対象の衝突面を取得
            Direction dir = this.CheckDirection(cell);

            //上面
            if(dir == Direction.Top)
            {
                //衝突対象の矩形の上面の上に接する座標に
                position.Y = cell.CellRect.Top - this.Height;
                LandFlag = true;
            }
            //右面
            else if(dir == Direction.Right)
            {
                position.X = cell.CellRect.Right;
            }
            //左面
            else if(dir == Direction.Left)
            {
                position.X = cell.CellRect.Left - this.Width;
            }
            //下面
            else if(dir == Direction.Bottom)
            {
                position.Y = cell.CellRect.Bottom;
            }
        }
    }
}
