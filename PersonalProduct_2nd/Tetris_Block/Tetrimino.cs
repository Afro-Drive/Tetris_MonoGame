using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Define;
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
        public enum Form_mino
        { I, T, J, L, S, Z, O, }
        private Form_mino form;

        /// <summary>
        /// テトリミノの種類ごとの描画するアセット名の列挙型
        /// </summary>
        public enum blk_Col
        { mino_I, mino_T, mino_J, mino_L, mino_S, mino_Z, mino_O, }
         blk_Col col;

        private int[,] rotate_Array; //回転処理用配列
        //private Cell[,] imageRotate_Array; //回転可能か検証する用の配列

        private float GRAVITY = 5f; //重力
        private IGameMediator mediator; //ゲーム仲介者

        private ArrayRenderer arrayRenderer; //二次元配列描画用オブジェクト

        private Timer landTimer; //着地後操作猶予タイマー
        private Timer fallTimer; //自動落下タイマー

        private bool landON; //着地フラグ
        private bool isLocked; //操作可能か？
        #endregion フィールド

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        public Tetrimino()
            : base()
        {

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
            //型を指定
            form = Form_mino.I;　//今はI型にしておく
            //使用色ブロックを指定(formの値に対応した色を指定する)
            col = (blk_Col)((int)form);

            //回転用配列を初期化
            rotate_Array = Mino_Array.mino_Data[(int)form]; //要素番号と列挙型のメンバの値とのズレをなくして指定
            #region データクラスから持ってくる処理に変更
            //rotate_Array = new int[5, 5] 
            //{
            //    { 0, 0, 1, 0, 0 },
            //    { 0, 0, 1, 0, 0 },
            //    { 0, 0, 1, 0, 0 },
            //    { 0, 0, 1, 0, 0 },
            //    { 0, 0, 0, 0, 0 },
            //}; //5×5を基本単位とする(奇数の方が回転の中心を取りやすい)
            #endregion データクラスから持ってくる処理に変更

            ////使用するアセットの初期化(formに応じて指定できるように変更したため削除)
            //assetName = form.ToString();

            //LineFieldの枠とズレるため削除
            position = new Vector2(Size.WIDTH * 10, Size.HEIGHT * 3); //X座標が大体フィールドの真ん中らへんに来るように設定

            //配列描画オブジェクトを生成・使用配列を指定
            //コンストラクタの引数がLineFieldで生成したArrayRendererのものと紐づける方法を考える
            arrayRenderer = new ArrayRenderer(new Vector2(Size.WIDTH * 2, Size.HEIGHT * 1));
            arrayRenderer.SetData(Mino_Array.mino_Data[(int)form]);

            //各種タイマーを生成
            landTimer = new CountDown_Timer(2.0f);
            fallTimer = new CountDown_Timer(1.5f);

            //離陸状態で初期化
            landON = false;
            //操作可能状態で初期化
            isLocked = false;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (landON) //着地状態なら
            {
                //着地タイマーを起動
                landTimer.Update(gameTime);
            }
            else //離陸したら
            {
                //初期化
                landTimer.Initialize();
            }
            if (landTimer.TimeUP()) //着地タイマーが時間切れ
            {
                //操作不可能とする(その場で位置を固定）
                isLocked = true;
            }

            //落下タイマーの更新（初期化はLineFieldで行われる）
            fallTimer.Update(gameTime);

            #region 落下タイマーが時間切れ時の処理→LineFieldに委託
            //if(fallTimer.TimeUP()) 
            //{
            //    //落下タイマーを初期化
            //    fallTimer.Initialize();
            //}
            #endregion 落下タイマーが時間切れ時の処理→LineFieldに委託
            //MoveX();
            //MoveY(gameTime);
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
        /// 横移動→キー入力と移動確認をLineFieldに委託
        /// </summary>
        //public void MoveX()
        //{
        //    //左移動
        //    if (Input.GetKeyTrigger(Keys.Left))
        //    {
        //        position.X -= Size.WIDTH;
        //    }
        //    //右移動
        //    if (Input.GetKeyTrigger(Keys.Right))
        //    {
        //        position.X += Size.WIDTH;
        //    }
        //}

        /// <summary>
        /// 右へ移動
        /// </summary>
        public void MoveR()
        {
            position.X += Size.WIDTH;
        }

        /// <summary>
        /// 左へ移動
        /// </summary>
        public void MoveL()
        {
            position.X -= Size.WIDTH;
        }

        /// <summary>
        /// 縦(下方向のみ)移動→キー入力・移動確認をLineFieldに委託
        /// </summary>
        /// <param name="gameTime"></param>
        //public void MoveY(GameTime gameTime)
        //{
        //    //fallTimer.Update(gameTime);
        //    //if (fallTimer.TimeUP()) //専用タイマーが時間切れになったら1マス分落下
        //    //{
        //    //    position.Y += Height;
        //    //    fallTimer.Initialize();
        //    //}

        //    if (Input.IsKeyDown(Keys.Down))
        //    {
        //        position.Y += GRAVITY;
        //    }
        //}

        /// <summary>
        /// 下へ移動
        /// </summary>
        public void MoveDown()
        {
            position.Y += Size.HEIGHT;
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
                position.Y = cell.GetHitArea().Top - Size.HEIGHT;
            }
            //右面
            else if (dir == Direction.Right)
            {
                position.X = cell.GetHitArea().Right;
            }
            //左面
            else if (dir == Direction.Left)
            {
                position.X = cell.GetHitArea().Left - Size.WIDTH;
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
            //配列描画オブジェクトに描画を委託
            arrayRenderer.RenderTetrimino(renderer, position, col);

            #region 回転用配列の要素番号に合わせて描画する→二次元配列描画クラスに委託
            //for (int y = 0; y < rotate_Array.GetLength(0); y++)
            //{
            //    for (int x = 0; x < rotate_Array.GetLength(1); x++)
            //    {
            //        //要素番号によって使用するアセットを設定
            //        switch (rotate_Array[y, x])
            //        {
            //            case 0:
            //                assetName = "";
            //                break;
            //            case 1:
            //                assetName = "mino_I";
            //                break;
            //            case 2:
            //                assetName = "mino_T";
            //                break;
            //            case 3:
            //                assetName = "mino_J";
            //                break;
            //            case 4:
            //                assetName = "mino_L";
            //                break;
            //            case 5:
            //                assetName = "mino_S";
            //                break;
            //            case 6:
            //                assetName = "mino_Z";
            //                break;
            //            case 7:
            //                assetName = "mino_O";
            //                break;
            //        }

            //        if (rotate_Array[y, x] == 0) continue; //要素が0の場合は飛ばす

            //        //設定したアセットで座標を指定して描画する
            //        renderer.DrawTexture(
            //            assetName,
            //            new Vector2(position.X + 64 * x, position.Y + 64 * y));
            //    }
            //}
            #endregion　回転用配列の要素番号に合わせて描画する→二次元配列描画クラスに委託
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

        /// <summary>
        /// 回転配列の中心位置からのテトリミノの構成ブロックの相対位置の取得
        /// </summary>
        /// <returns>ミノの1ブロックの座標を格納したリスト</returns>
        public List<Vector2> GetMinoUnitPos()
        {
            //ミノのユニット位置格納用リストの生成
            List<Vector2> list_UnitPos = new List<Vector2>();

            //回転用配列の要素を一つずつ確認
            for (int y = 0; y < rotate_Array.GetLength(0); y++)
            {
                for (int x = 0; x < rotate_Array.GetLength(1); x++)
                {
                    //テトリミノを構成するブロック(＝空白でない)ならば
                    if (rotate_Array[y, x] != 0)
                    {
                        //[2,2]を基準とした相対座標をリストに追加
                        list_UnitPos.Add(
                            new Vector2(Size.WIDTH * (x - 2), Size.HEIGHT * (y - 2))
                            );
                    }
                }
            }
            //リストを返却
            return list_UnitPos;
        }

        /// <summary>
        /// 着地状態の切り替え
        /// </summary>
        /// <param name="flag">True→着地　False→離陸</param>
        public void LandSwich(bool flag)
        {
            landON = Convert.ToBoolean(flag);
        }

        /// <summary>
        /// 落下状態か？
        /// </summary>
        /// <returns>落下タイマーが時間切れかどうか返却</returns>
        public bool IsFall()
        {
            return fallTimer.TimeUP();
        }

        /// <summary>
        /// 落下タイマーの初期化
        /// </summary>
        public void InitFall()
        {
            fallTimer.Initialize();
        }
    }
}
