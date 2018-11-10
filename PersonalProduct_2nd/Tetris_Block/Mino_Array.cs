using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// TetriminoのForm_Mino列挙型メンバごとのポリオミノ(二次元配列で表現)
    /// 作成者:谷　永吾
    /// 作成開始日:2018年11月10日
    /// </summary>
    static class Mino_Array
    {
        #region Tetrimino専用の配列(４×４が基本単位)
        public static List<int[,]> mino_Data = new List<int[,]>()
        {
            //Iミノ
            new int[,]
            {
                { 0, 2, 0, 0 },
                { 0, 2, 0, 0 },
                { 0, 2, 0, 0 },
                { 0, 2, 0, 0 }
            },

            //T
            new int[,]
            {
                { 0, 0, 0, 0 },
                { 0, 2, 0, 0 },
                { 0, 2, 2, 0 },
                { 0, 2, 0, 0 }
            },

            //J
            new int[,]
            {
                { 0, 0, 0, 0 },
                { 0, 2, 2, 0 },
                { 0, 2, 0, 0 },
                { 0, 2, 0, 0 }
            },

            //L
            new int[,]
            {
                { 0, 0, 0, 0 },
                { 0, 2, 2, 0 },
                { 0, 0, 2, 0 },
                { 0, 0, 2, 0 }
            },

            //S
            new int[,]
            {
                { 0, 0, 0, 0 },
                { 0, 0, 2, 2 },
                { 0, 2, 2, 0 },
                { 0, 0, 0, 0 }
            },

            //Z
            new int[,]
            {
                { 0, 0, 0, 0 },
                { 2, 2, 0, 0 },
                { 0, 2, 2, 0 },
                { 0, 0, 0, 0 }
            },

            //O(Test)
            new int[,]
            {
                { 0, 0, 0, 0 },
                { 0, 2, 2, 0 },
                { 0, 2, 2, 0 },
                { 0, 0, 0, 0 }
            },
        };
        #endregion Tetrimino専用の配列(４×４が基本単位)

        /// <summary>
        /// 各種テトリミノの長さの取得
        /// </summary>
        /// <param name="form">Minoの形に対応する要素番号</param>
        /// <returns>指定テトリミノの縦の長さ</returns>
        public static int Length(int form)
        {
            int sides = Cell.HEIGHT; //一辺の長さ(Cellの縦の長さで初期化)
            //管理リスト内の指定配列の縦の要素数分さらに加算
            for (int i = 0; i < mino_Data[form].GetLength(0); i++)
            {
                sides += Cell.HEIGHT;
            }
            return sides;
        }
    }
}
