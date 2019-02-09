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
    class Tetrimino : Cell
    {
        #region フィールド
        /// <summary>
        /// テトリミノの種類列挙型
        /// </summary>
        enum Form_mino
        { I = 1, T, J, L, S, Z, O, }
        private Form_mino form;

        /// <summary>
        /// テトリミノの種類ごとの描画するアセット名の列挙型
        /// </summary>
        enum blk_Col
        { mino_I = 1, mino_T, mino_J, mino_L, mino_S, mino_Z, mino_O, }
        private blk_Col col;

        private int[,] rotate_Array; //回転処理用配列
        //private Cell[,] imageRotate_Array; //回転可能か検証する用の配列

        private float GRAVITY = 5f; //重力
        private SoundManager sound; //動作音用
        private IGameMediator mediator; //ゲーム仲介者
        #endregion フィールド

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        public Tetrimino()
            : base()
        {
            sound = DeviceManager.CreateInstance().GetSound();

            Initialize(); //各種変数の初期化
        }

        public Tetrimino(Tetrimino other)
            : this()
        {

        }

        /// <summary>
        ///  初期化
        /// </summary>
        public override void Initialize()
        {
            //各種回転用の配列を初期化
            //I型
            rotate_Array = new int[5, 5]
            {
                { 0, 0, 1, 0, 0 },
                { 0, 0, 1, 0, 0 },
                { 0, 0, 1, 0, 0 },
                { 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0 },
            }; //5×5を基本単位とする(奇数の方が回転の中心を取りやすい)

            //使用するアセットの初期化
            assetName = "mino_I";

            position = new Vector2(WIDTH * 10, 0); //X座標が大体フィールドの真ん中らへんに来るように設定
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
        //public Cell[,] Rotate_Clockwise(Cell[,] basedArray)
        //{
        //    int rows = basedArray.GetLength(0); //列(横)
        //    int cols = basedArray.GetLength(1); //行(縦)
        //    imageRotate_Array = new Cell[cols, rows]; //回転前とは行と列を逆にした配列を生成

        //    for (int y = 0; y < rows; y++) //回転後の配列の列に回転前の配列の行分要素を用意
        //    {
        //        for (int x = 0; x < cols; x++) //回転後の配列の行に回転前の配列の列分要素を用意
        //        {
        //            //回転後の配列の列は、回転前の配列の行からy(新規配列の列の生成回数)と1を引いたものに一致する
        //            //知るかこの野郎
        //            imageRotate_Array[x, rows - y - 1] = basedArray[y, x];
        //        }
        //    }
        //    return imageRotate_Array;
        //}

        /// <summary>
        /// 二次元配列を反時計回りに回転
        /// </summary>
        /// <param name="basedArray">回転したい二次元配列</param>
        /// <returns>反時計回りに90°回転させた二次元配列</returns>
        //public Cell[,] Rotate_AntiClockwise(Cell[,] basedArray)
        //{
        //    int rows = basedArray.GetLength(0); //行(横)
        //    int cols = basedArray.GetLength(1); //列(縦)
        //    imageRotate_Array = new Cell[cols, rows];

        //    for (int y = 0; y < rows; y++) //回転後の配列の列に回転前の配列の行分だけ要素を用意
        //    {
        //        for (int x = 0; x < cols; x++) //回転後の配列の行に回転前の配列の列分だけ要素を用意
        //        {
        //            //回転後の配列の列は回転前の配列の行からx(回転後の列を用意した回数)と1を引いたものに一致する
        //            //時計回りとはややこしくなる方が逆になるんだね。知らんけど。
        //            imageRotate_Array[rows - x - 1, y] = basedArray[y, x];
        //        }
        //    }

        //    return imageRotate_Array;
        //}

        /// <summary>
        /// 横移動
        /// </summary>
        public void MoveX()
        {
            if (Input.GetKeyTrigger(Keys.Left))
            {
                position.X -= WIDTH;
            }
            if (Input.GetKeyTrigger(Keys.Right))
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
            //fallTimer.Update(gameTime);
            //if (fallTimer.TimeUP()) //専用タイマーが時間切れになったら1マス分落下
            //{
            //    position.Y += Height;
            //    fallTimer.Initialize();
            //}

            if (Input.IsKeyDown(Keys.Down))
            {
                position.Y += GRAVITY;
            }
        }

        public override void Hit(Cell other)
        {
            hitBlock(other);
        }

        private void hitBlock(Cell cell)
        {
            //衝突対象の衝突面を取得
            Direction dir = this.CheckDirection(cell);

            //上面
            if (dir == Direction.Top)
            {
                //衝突対象の矩形の上面の上に接する座標に
                position.Y = cell.GetHitArea().Top - this.Height;
            }
            //右面
            else if (dir == Direction.Right)
            {
                position.X = cell.GetHitArea().Right;
            }
            //左面
            else if (dir == Direction.Left)
            {
                position.X = cell.GetHitArea().Left - this.Width;
            }
            //下面
            else if (dir == Direction.Bottom)
            {
                position.Y = cell.GetHitArea().Bottom;
            }
        }

        public override object Clone()
        {
            return new Tetrimino(this);
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer)
        {
            //回転用配列の要素番号に合わせて描画する
            for (int y = 0; y < rotate_Array.GetLength(0); y++)
            {
                for (int x = 0; x < rotate_Array.GetLength(1); x++)
                {
                    //要素番号によって使用するアセットを設定
                    switch (rotate_Array[y, x])
                    {
                        case 0:
                            assetName = "";
                            break;
                        case 1:
                            assetName = "mino_I";
                            break;
                        case 2:
                            assetName = "mino_T";
                            break;
                        case 3:
                            assetName = "mino_J";
                            break;
                        case 4:
                            assetName = "mino_L";
                            break;
                        case 5:
                            assetName = "mino_S";
                            break;
                        case 6:
                            assetName = "mino_Z";
                            break;
                        case 7:
                            assetName = "mino_O";
                            break;
                    }

                    if (rotate_Array[y, x] == 0) continue; //要素が0の場合は飛ばす

                    //設定したアセットで座標を指定して描画する
                    renderer.DrawTexture(
                        assetName,
                        new Vector2(position.X + 64 * x, position.Y + 64 * y));
                }
            }
        }

        /// <summary>
        /// 回転用配列に合わせた当たり判定エリアの生成・取得
        /// </summary>
        /// <returns>構成されたミノブロックごとの当たり判定エリアを格納した矩形型の配列</returns>
        public new Rectangle[] GetHitArea()
        {
            List<Rectangle> hitAreas = new List<Rectangle>(); //当たり判定エリアリストの生成
            Rectangle[] hitArea_Array; //リストから配列に変換した後に格納する配列を生成

            //回転用配列の要素を一つずつ確認
            for (int y = 0; y < rotate_Array.GetLength(0); y++)
            {
                for (int x = 0; x < rotate_Array.GetLength(1); x++)
                {
                    if (rotate_Array[y, x] == 0) continue; //要素番号が0の時は何もしない

                    //回転配列の座標に合わせた位置に矩形を生成、リストに追加
                    hitAreas.Add(
                        new Rectangle(
                            new Point((int)position.X + x * 64, (int)position.Y + y * 64),
                            new Point(64, 64))
                            );
                }
            }
            //リストを配列に変換し、格納
            hitArea_Array = hitAreas.ToArray();
            //変換した配列を返却
            return hitArea_Array;
        }
    }
}
