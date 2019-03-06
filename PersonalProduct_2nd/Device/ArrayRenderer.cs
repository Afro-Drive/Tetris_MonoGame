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
        private int[,] fieldData; //LineField用の描画に用いる配列データ
        private Dictionary<blk_Col, int[,]> data; //テトリミノの描画に用いるデータをまとめたディクショナリ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="offset">描画開始位置</param>
        public ArrayRenderer(Vector2 offset)
        {
            //描画の基点を取得
            this.offset = offset;
            //ディクショナリの生成(ミノの形に対応した回転配列を登録)
            data = new Dictionary<blk_Col, int[,]>()
            {
                { blk_Col.mino_I, mino_Data[0] },
                { blk_Col.mino_T, mino_Data[1] },
                { blk_Col.mino_J, mino_Data[2] },
                { blk_Col.mino_L, mino_Data[3] },
                { blk_Col.mino_S, mino_Data[4] },
                { blk_Col.mino_Z, mino_Data[5] },
                { blk_Col.mino_O, mino_Data[6] },
            };
        }

        /// <summary>
        /// 描画に用いる配列の設定
        /// </summary>
        /// <param name="dataName">使用用途に合わせた二次元配列の名前</param>
        /// <param name="data">描画に使う二次元配列</param>
        /// <returns></returns>
        public bool SetData(int[,] data)
        {
            this.fieldData = data;
            return true;
        }

        /// <summary>
        /// ステージの描画
        /// </summary>
        /// <param name="renderer"></param>
        public void RenderField(Renderer renderer)
        {
            //配列を一要素ずつ描画する
            for (int y = 0; y < fieldData.GetLength(0); y++)
            {
                for (int x = 0; x < fieldData.GetLength(1); x++)
                {
                    //要素番号に応じたブロックを描画する(もう少し効率的に書けないか？)
                    if (fieldData[y, x] == 1)
                        renderer.DrawTexture("black",
                            new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    else if(fieldData[y, x] == 2)
                        renderer.DrawTexture("mino_I",
                            new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    else if (fieldData[y, x] == 3)
                        renderer.DrawTexture("mino_T",
                            new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    else if (fieldData[y, x] == 4)
                        renderer.DrawTexture("mino_J",
                            new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    else if (fieldData[y, x] == 5)
                        renderer.DrawTexture("mino_L",
                            new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    else if (fieldData[y, x] == 6)
                        renderer.DrawTexture("mino_S",
                            new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    else if (fieldData[y, x] == 7)
                        renderer.DrawTexture("mino_Z",
                            new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);
                    else if (fieldData[y, x] == 8)
                        renderer.DrawTexture("mino_O",
                            new Vector2(x * Size.WIDTH, y * Size.HEIGHT) + offset);

                }
            }
        }

        /// <summary>
        /// テトリミノの描画
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="center">回転配列の中心座標</param>
        /// <param name="color">描画を行うテトリミノのblk_Colメンバ</param>
        public void RenderTetrimino(Renderer renderer, Vector2 center, blk_Col color)
        {
            for (int y = 0; y < data[color].GetLength(0); y++)
            {
                for (int x = 0; x < data[color].GetLength(1); x++)
                {
                    //0(Space)でなければ描画する
                    if (fieldData[y, x] != 0)
                        renderer.DrawTexture(color.ToString(),
                            center + new Vector2((x - 2) * Size.WIDTH, (y - 2) * Size.HEIGHT) + offset);
                }
            }
        }
    }
}
