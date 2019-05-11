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
    /// テトリミノの種類列挙型
    /// </summary>
    enum Form_mino
    { I, T, J, L, S, Z, O, }

    /// <summary>
    /// テトリミノの種類ごとの描画するアセット名の列挙型
    /// </summary>
    enum Blk_Col
    { mino_I, mino_T, mino_J, mino_L, mino_S, mino_Z, mino_O, }

    /// <summary>
    /// Blockクラスを継承した操作可能テトリスブロッククラス
    /// 作成者:谷永吾
    /// 作成開始日:2018年11月7日
    /// </summary>
    class Tetrimino : Cell
    {
        #region フィールド
        private Form_mino form;//テトリミノの形
        Blk_Col col;//テトリミノの色

        private int[,] rotate_Array; //回転処理用配列
        private int[,] imageRotate_Array; //回転可能か検証する用の配列

        private ArrayRenderer arrayRenderer; //二次元配列描画用オブジェクト

        private Random rnd; //ランダムオブジェクト
        #endregion フィールド

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        public Tetrimino()
            : base()
        {
            //ランダムを生成
            rnd = DeviceManager.CreateInstance().GetRandom();
            //テトリミノの型の中からランダムに型を指定
            int enumLength = Enum.GetValues(typeof(Form_mino)).GetLength(0);
            form = (Form_mino)(rnd.Next(0, enumLength));
            //使用色ブロックを指定(formの値に対応した色を指定する)
            col = (Blk_Col)((int)form);

            Initialize(); //各種変数の初期化
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="form">指定する形</param>
        public Tetrimino(Form_mino form)
            :base()
        {
            this.form = form;
            col = (Blk_Col)((int)form);

            Initialize();
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
            //回転用配列を初期化
            rotate_Array = Mino_Array.mino_Data[(int)form]; //要素番号と列挙型のメンバの値とのズレをなくして指定

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
        }

        /// <summary>
        /// 描画(座標指定・補正値加算無)
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="position"></param>
        public void DrawNoOffset(Renderer renderer, Vector2 position)
        {
            arrayRenderer.NotOffsetRender(renderer, position, col);
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
        /// 型に応じた数値を取得
        /// </summary>
        /// <returns>フィールドformをint型にキャストして＋２した値</returns>
        public int GetUnitNum()
        {
            return (int)form + 2;
        }
    }
}
