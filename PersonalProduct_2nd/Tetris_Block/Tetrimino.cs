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
    { I = 2, T, J, L, S, Z, O, }

    /// <summary>
    /// テトリミノの種類ごとの描画するアセット名の列挙型
    /// </summary>
    enum Blk_Col
    { black = 1, mino_I, mino_T, mino_J, mino_L, mino_S, mino_Z, mino_O, }

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
            rotate_Array = Mino_Array.mino_Data[(int)form - 2]; //要素番号と列挙型のメンバの値とのズレをなくして指定

            //初期位置を設定
            Position = new Vector2(Size.WIDTH * 7, Size.HEIGHT * 3); //X座標が大体フィールドの真ん中らへんに来るように設定

            //配列描画オブジェクトを生成・使用配列を指定
            //コンストラクタの引数がLineFieldで生成したArrayRendererのものと紐づける方法を考える
            arrayRenderer = new ArrayRenderer(Size.OFFSET);
            arrayRenderer.SetData(rotate_Array);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
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
        /// 型に応じた数値を取得
        /// </summary>
        /// <returns>フィールドformをint型にキャストして＋２した値</returns>
        public int GetUnitNum()
        {
            return (int)form;
        }

        public void SetArray(int[,] data)
        {
            rotate_Array = data;
            arrayRenderer.SetData(rotate_Array);
        }

        public int[,] GetRotate_Array()
        {
            return rotate_Array;
        }
    }
}
