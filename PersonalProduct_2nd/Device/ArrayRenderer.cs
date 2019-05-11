using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Tetris_Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PersonalProduct_2nd.Tetris_Block.Tetrimino;
using static PersonalProduct_2nd.Tetris_Block.Mino_Array;
using PersonalProduct_2nd.Define;

namespace PersonalProduct_2nd.Device
{
    /// <summary>
    /// フィールド・テトリミノの配列描画用オブジェクト
    /// 作成者:谷 永吾
    /// 作成開始日:2019年2月13日
    /// </summary>
    class ArrayRenderer
    {
        //フィールド
        private Vector2 offset; //描画開始位置
        private int[,] fieldMatrixData; //LineField用の描画に用いる正方形配列データ
        private int[][] fieldJaggedData; //LineField用の描画に用いるジャグ配列データ
        private Dictionary<Blk_Col, int[,]> data; //テトリミノの描画に用いるデータをまとめたディクショナリ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="offset">描画開始位置</param>
        public ArrayRenderer(Vector2 offset)
        {
            //描画の基点を取得
            this.offset = offset;
            //ディクショナリの生成(ミノの形に対応した回転配列を登録)
            data = new Dictionary<Blk_Col, int[,]>()
            {
                { Blk_Col.mino_I, mino_Data[0] },
                { Blk_Col.mino_T, mino_Data[1] },
                { Blk_Col.mino_J, mino_Data[2] },
                { Blk_Col.mino_L, mino_Data[3] },
                { Blk_Col.mino_S, mino_Data[4] },
                { Blk_Col.mino_Z, mino_Data[5] },
                { Blk_Col.mino_O, mino_Data[6] },
            };
        }

        /// <summary>
        /// 描画に用いる配列の設定
        /// </summary>
        /// <param name="data">描画に使う二次元配列</param>
        /// <returns></returns>
        public bool SetData(int[,] data)
        {
            this.fieldMatrixData = data;
            return true;
        }

        /// <summary>
        /// 描画に用いるジャグ配列の設定
        /// </summary>
        /// <param name="jaggedData">描画用配列(ジャグ配列)</param>
        /// <returns></returns>
        public bool SetData(int[][] jaggedData)
        {
            this.fieldJaggedData = jaggedData;
            return true;
        }

        /// <summary>
        /// ステージの描画
        /// </summary>
        /// <param name="renderer"></param>
        public void RenderField(Renderer renderer)
        {
            //配列を一要素ずつ描画する
            for (int y = 0; y < fieldMatrixData.GetLength(0); y++)
            {
                for (int x = 0; x < fieldMatrixData.GetLength(1); x++)
                {
                    if (fieldMatrixData[y, x] == 0)
                        continue;

                    //配列の要素に応じた列挙型メンバを特定
                    //そのメンバからアセット名(string)へ変換
                    string mino =
                        ((Blk_Col)Enum.ToObject(typeof(Blk_Col), fieldJaggedData[y][x])).ToString();
                    //要素番号に応じたブロックを描画する(もう少し効率的に書けないか？)
                    renderer.DrawTexture(
                        mino,
                        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    #region 記述を簡略化
                    //if (fieldMatrixData[y, x] == 1)
                    //    renderer.DrawTexture("black",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    //else if (fieldMatrixData[y, x] == 2)
                    //    renderer.DrawTexture("mino_I",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    //else if (fieldMatrixData[y, x] == 3)
                    //    renderer.DrawTexture("mino_T",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    //else if (fieldMatrixData[y, x] == 4)
                    //    renderer.DrawTexture("mino_J",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    //else if (fieldMatrixData[y, x] == 5)
                    //    renderer.DrawTexture("mino_L",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    //else if (fieldMatrixData[y, x] == 6)
                    //    renderer.DrawTexture("mino_S",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    //else if (fieldMatrixData[y, x] == 7)
                    //    renderer.DrawTexture("mino_Z",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    //else if (fieldMatrixData[y, x] == 8)
                    //    renderer.DrawTexture("mino_O",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    #endregion 記述を簡略化
                }
            }
        }

        /// <summary>
        /// ステージの描画(ジャグ配列)
        /// ジャグ配列と多次元配列をまとめて使えないだろうか？
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="color">描画色</param>
        public void RenderJugField(Renderer renderer, Color color)
        {
            //配列を一要素ずつ描画する
            for (int y = 0; y < fieldJaggedData.GetLength(0); y++)
            {
                for (int x = 0; x < fieldJaggedData[y].Length; x++)
                {
                    if (fieldJaggedData[y][x] == 0)
                        continue;

                    //配列の要素に応じた列挙型メンバを特定
                    //そのメンバからアセット名(string)へ変換
                    string mino =
                        ((Blk_Col)Enum.ToObject(typeof(Blk_Col), fieldJaggedData[y][x])).ToString();
                    //要素番号に応じたブロックを描画する(もう少し効率的に書けないか？)
                    renderer.DrawTexture(
                        mino,
                        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset,
                        color);
                    #region 記述を共通化
                    //if (fieldJaggedData[y][x] == 1)
                    //    renderer.DrawTexture("black",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset,
                    //        color);
                    //else if (fieldJaggedData[y][x] == 2)
                    //    renderer.DrawTexture("mino_I",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset,
                    //        color);
                    //else if (fieldJaggedData[y][x] == 3)
                    //    renderer.DrawTexture("mino_T",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset,
                    //        color);
                    //else if (fieldJaggedData[y][x] == 4)
                    //    renderer.DrawTexture("mino_J",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset,
                    //        color);
                    //else if (fieldJaggedData[y][x] == 5)
                    //    renderer.DrawTexture("mino_L",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset,
                    //        color);
                    //else if (fieldJaggedData[y][x] == 6)
                    //    renderer.DrawTexture("mino_S",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset,
                    //        color);
                    //else if (fieldJaggedData[y][x] == 7)
                    //    renderer.DrawTexture("mino_Z",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset,
                    //        color);
                    //else if (fieldJaggedData[y][x] == 8)
                    //    renderer.DrawTexture("mino_O",
                    //        new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset,
                    //        color);
                    #endregion 記述を共通化
                }
            }
        }

        /// <summary>
        /// テトリミノの描画
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="center">回転配列の中心座標</param>
        /// <param name="color">描画を行うテトリミノのblk_Colメンバ</param>
        /// <param name="alpha">透明度(デフォルト値1.0)</param>
        public void RenderTetrimino(Renderer renderer, Vector2 center, Blk_Col color, float alpha = 1.0f)
        {
            for (int y = 0; y < data[color].GetLength(0); y++)
            {
                for (int x = 0; x < data[color].GetLength(1); x++)
                {
                    //0(Space)でなければ描画する
                    if (fieldMatrixData[y, x] != 0)
                        renderer.DrawTexture(color.ToString(),
                            center + new Vector2((x - 2) * Size.WIDTH, (y - 2) * Size.HEIGHT) + offset,
                            alpha);
                }
            }
        }

        /// <summary>
        /// 座標補正を用いない描画
        /// (主にネクストミノ、ホールドミノ用)
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="center"></param>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        public void NotOffsetRender(Renderer renderer, Vector2 center, Blk_Col color, float alpha = 1.0f)
        {
            for (int y = 0; y < data[color].GetLength(0); y++)
            {
                for (int x = 0; x < data[color].GetLength(1); x++)
                {
                    if (fieldMatrixData[y, x] != 0)
                        renderer.DrawTexture(color.ToString(),
                            center + new Vector2((x - 2) * Size.WIDTH, (y - 2) * Size.HEIGHT),
                            alpha);

                }
            }
        }
    }
}
