﻿using System;
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
        private int[,] imageRotate_Array; //回転可能か検証する用の配列

        private float GRAVITY = 5f; //重力
        private IGameMediator mediator; //ゲーム仲介者

        private ArrayRenderer arrayRenderer; //二次元配列描画用オブジェクト

        //private CountDown_Timer landTimer; //着地後操作猶予タイマー
        //private CountDown_Timer fallTimer; //自動落下タイマー
        //private CountDown_Timer inputFallTimer; //キー入力時の落下タイマー

        //private bool landON; //着地フラグ
        //private bool isLocked; //操作可能か？
        //private bool inputFallON; //落下用キー入力がされたか？

        private Random rnd; //ランダムオブジェクト
        private List<Form_mino> formList; //テトリミノの形を格納したリスト
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
            //ランダムを生成
            rnd = DeviceManager.CreateInstance().GetRandom();
            //テトリミノの型の中からランダムに型を指定
            int enumLength = Enum.GetValues(typeof(Form_mino)).GetLength(0);
            form = (Form_mino)(rnd.Next(0, enumLength));
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

            //初期位置を設定
            Position = new Vector2(Size.WIDTH * 7, Size.HEIGHT * 3); //X座標が大体フィールドの真ん中らへんに来るように設定

            //配列描画オブジェクトを生成・使用配列を指定
            //コンストラクタの引数がLineFieldで生成したArrayRendererのものと紐づける方法を考える
            arrayRenderer = new ArrayRenderer(Size.offset);
            arrayRenderer.SetData(rotate_Array);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            #region Aキーが入力されたら時計回りに90度回転→Rotate_Clockwiseメソッドに委託
            //if (Input.GetKeyTrigger(Keys.A))
            //{
            //    //回転用配列に回転後の配列を格納
            //    rotate_Array = GetRotate_Clockwise_Array();
            //    //描画する配列を設定しなおす(refとかで書き直せる？)
            //    arrayRenderer.SetData(rotate_Array);
            //}
            #endregion Aキーが入力されたら時計回りに90度回転→Rotate_Clockwiseメソッドに委託

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
        public void Rotate_Clockwise()
        {
            //検証用回転配列を実際の回転配列に代入
            rotate_Array = imageRotate_Array;
            //二次元描画対象を設定しなおす
            arrayRenderer.SetData(rotate_Array);
            #region GetRotate_Clockwise_Arrayメソッドに委託
            //int rows = rotate_Array.GetLength(0); //列(横)
            //int cols = rotate_Array.GetLength(1); //行(縦)
            //imageRotate_Array = new int[cols, rows]; //回転前とは行と列を逆にした配列を生成

            //for (int y = 0; y < rows; y++) //回転後の配列の列に回転前の配列の行分要素を用意
            //{
            //    for (int x = 0; x < cols; x++) //回転後の配列の行に回転前の配列の列分要素を用意
            //    {
            //        //回転後の配列の列は、回転前の配列の行からy(新規配列の列の生成回数)と1を引いたものに一致する
            //        //知るかこの野郎
            //        imageRotate_Array[x, rows - y - 1] = rotate_Array[y, x];
            //    }
            //}
            #endregion GetRotate_Clockwise_Arrayメソッドに委託
        }

        /// <summary>
        /// 時計回りに90度回転後のテトリミノ配列を取得する
        /// </summary>
        /// <returns>時計回りに90度回転後の二次元配列</returns>
        public int[,] GetClockwise_RotatedArray()
        {
            int rows = rotate_Array.GetLength(0); //列(横)
            int cols = rotate_Array.GetLength(1); //行(縦)
            imageRotate_Array = new int[cols, rows]; //回転前とは行と列を逆にした配列を生成

            for (int y = 0; y < rows; y++) //回転後の配列の列に回転前の配列の行分要素を用意
            {
                for (int x = 0; x < cols; x++) //回転後の配列の行に回転前の配列の列分要素を用意
                {
                    //回転後の配列の列は、回転前の配列の行からy(新規配列の列の生成回数)と1を引いたものに一致する
                    //知るかこの野郎
                    imageRotate_Array[x, rows - y - 1] = rotate_Array[y, x];
                }
            }
            return imageRotate_Array;
        }

        /// <summary>
        /// テトリミノを反時計回りに90度回転させる
        /// </summary>
        public void Rotate_AntiClockwise()
        {
            #region GetAntiClockwise_RotatedArrayメソッドに委託
            //int rows = rotate_Array.GetLength(0); //行(横)
            //int cols = rotate_Array.GetLength(1); //列(縦)
            //imageRotate_Array = new int[cols, rows];

            //for (int y = 0; y < rows; y++) //回転後の配列の列に回転前の配列の行分だけ要素を用意
            //{
            //    for (int x = 0; x < cols; x++) //回転後の配列の行に回転前の配列の列分だけ要素を用意
            //    {
            //        //回転後の配列の列は回転前の配列の行からx(回転後の列を用意した回数)と1を引いたものに一致する
            //        //時計回りとはややこしくなる方が逆になるんだね。知らんけど。
            //        imageRotate_Array[rows - x - 1, y] = rotate_Array[y, x];
            //    }
            //}
            #endregion GetAntiClockwise_RotatedArrayメソッドに委託

            //回転用配列に回転後の配列を格納
            rotate_Array = imageRotate_Array;
            //描画する配列を設定しなおす(refとかで書き直せる？)
            arrayRenderer.SetData(rotate_Array);
        }

        /// <summary>
        /// 反時計回りに回転させたテトリミノ配列の取得
        /// </summary>
        /// <returns>反時計回りに90°回転させた二次元配列</returns>
        public int[,] GetAntiClockwise_RotatedArray()
        {
            int rows = rotate_Array.GetLength(0); //行(横)
            int cols = rotate_Array.GetLength(1); //列(縦)
            imageRotate_Array = new int[cols, rows];

            for (int y = 0; y < rows; y++) //回転後の配列の列に回転前の配列の行分だけ要素を用意
            {
                for (int x = 0; x < cols; x++) //回転後の配列の行に回転前の配列の列分だけ要素を用意
                {
                    //回転後の配列の列は回転前の配列の行からx(回転後の列を用意した回数)と1を引いたものに一致する
                    //時計回りとはややこしくなる方が逆になるんだね。知らんけど。
                    imageRotate_Array[rows - x - 1, y] = rotate_Array[y, x];
                }
            }
            return imageRotate_Array;
        }

        /// <summary>
        /// 横移動→キー入力と移動確認をLineFieldに委託
        /// </summary>
        //public void MoveX()
        //{
        //    //左移動
        //    if (Input.GetKeyTrigger(Keys.Left))
        //    {
        //        Position.X -= Size.WIDTH;
        //    }
        //    //右移動
        //    if (Input.GetKeyTrigger(Keys.Right))
        //    {
        //        Position.X += Size.WIDTH;
        //    }
        //}

        /// <summary>
        /// 右へ移動→MinoMoveへ委託
        /// </summary>
        //public void MoveR()
        //{
        //    Position += new Vector2(Size.WIDTH, 0);
        //}

        /// <summary>
        /// 左へ移動→MinoMoveへ委託
        /// </summary>
        //public void MoveL()
　      //{
        //    Position -= new Vector2(Size.WIDTH, 0);
        //}

        /// <summary>
        /// 縦(下方向のみ)移動→キー入力・移動確認をLineFieldに委託
        /// </summary>
        /// <param name="gameTime"></param>
        //public void MoveY(GameTime gameTime)
        //{
        //    //fallTimer.Update(gameTime);
        //    //if (fallTimer.TimeUP()) //専用タイマーが時間切れになったら1マス分落下
        //    //{
        //    //    Position.Y += Height;
        //    //    fallTimer.Initialize();
        //    //}

        //    if (Input.IsKeyDown(Keys.Down))
        //    {
        //        Position.Y += GRAVITY;
        //    }
        //}

        /// <summary>
        /// 下へ移動→MinoMoveへ委託
        /// </summary>
        //public void MoveDown()
        //{
        //    Position += new Vector2(0, Size.HEIGHT);
        //}

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
            arrayRenderer.RenderTetrimino(renderer, Position, col);

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
            //            new Vector2(Position.X + 64 * x, Position.Y + 64 * y));
            //    }
            //}
            #endregion　回転用配列の要素番号に合わせて描画する→二次元配列描画クラスに委託
        }

        /// <summary>
        /// テトリミノの影の描画
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="bottonPos"></param>
        public void DrawShadow(Renderer renderer, Vector2 bottonPos)
        {
            //透明度を下げた状態で描画する
            arrayRenderer.RenderTetrimino(renderer, bottonPos, col, 0.3f);
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
        /// 回転後のテトリミノ構成ブロックの中心からの相対座標の取得
        /// </summary>
        /// <param name="checkArray">回転後のテトリミノの回転配列</param>
        /// <returns>配列内のブロックの座標を格納したリスト</returns>
        public List<Vector2> GetRotatedUnitPos(int[,] checkArray)
        {
            //返却用のリストを生成
            List<Vector2> list_Pos = new List<Vector2>();

            //引数の配列内の要素を一つずつ確かめる
            for (int y = 0; y < checkArray.GetLength(0); y++)
            {
                for (int x = 0; x < checkArray.GetLength(1); x++)
                {
                    //確認した要素が0以外なら
                    if (checkArray[y, x] != 0)
                    {
                        //リストに追加する
                        //中心[2,2]を基準とした座標を算出
                        list_Pos.Add(
                            new Vector2(Size.WIDTH * (x - 2), Size.HEIGHT * (y - 2))
                            );
                    }
                }
            }
            //リストを返却する
            return list_Pos;
        }

        /// <summary>
        /// 着地状態の切り替え
        /// </summary>
        /// <param name="flag">True→着地　False→離陸</param>
        //public void LandSwich(bool flag)
        //{
        //    landON = Convert.ToBoolean(flag);
        //}

        /// <summary>
        /// キー入力による落下受付
        /// </summary>
        /// <param name="flag"></param>
        //public void InputFallSwitch(bool flag)
        //{
        //    inputFallON = Convert.ToBoolean(flag);
        //}

        /// <summary>
        /// 落下状態か？
        /// </summary>
        /// <returns>落下タイマーが時間切れかどうか返却</returns>
        //public bool IsFall()
        //{
        //    return fallTimer.TimeUP();
        //}

        /// <summary>
        /// 落下タイマーの初期化
        /// </summary>
        //public void InitFall()
        //{
        //    fallTimer.Initialize();
        //}

        /// <summary>
        /// キー入力落下タイマーの初期化
        /// </summary>
        //public void InitInputFall()
        //{
        //    inputFallTimer.Initialize();
        //}

        /// <summary>
        /// 落下タイマーを強制終了
        /// </summary>
        //public void LandTimeZero()
        //{
        //    landTimer.ForceZero();
        //}

        /// <summary>
        /// テトリミノが固定状態か？
        /// </summary>
        /// <returns>フィールドのisLocked</returns>
        //public bool IsLocked()
        //{
        //    return isLocked;
        //}

        /// <summary>
        /// 型に応じた数値を取得
        /// </summary>
        /// <returns>フィールドformをint型にキャストして＋２した値</returns>
        public int GetUnitNum()
        {
            return (int)form + 2;
        }

        /// <summary>
        /// 着地後のタイマーの制限時間を再設定
        /// </summary>
        /// <param name="newCount"></param>
        //public void ResetLandTimer(float newCount)
        //{
        //    landTimer.ResetLimitTime(newCount);
        //}

        /// <summary>
        /// 落下タイマーの制限時間を再設定
        /// </summary>
        /// <param name="newCount"></param>
        //public void ResetFallTimer(float newCount)
        //{
        //    fallTimer.ResetLimitTime(newCount);
        //}

        /// <summary>
        /// キー入力落下可能となったか？
        /// </summary>
        /// <returns>キー入力後の落下タイマーが時間切れになったかどうか</returns>
        //public bool CanInputFall()
        //{
        //    return inputFallTimer.TimeUP();
        //}

        #region 回転可能かのプロパティ→MinoStateManagerクラスのCanMoveに委託
        //public bool CanRotate { get; set; }
        #endregion 回転可能かのプロパティ→MinoStateManagerクラスに委託
    }
}
