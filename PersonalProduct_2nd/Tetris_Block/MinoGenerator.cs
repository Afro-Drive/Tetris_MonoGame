using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PersonalProduct_2nd.Device;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// テトリミノ生産者
    /// </summary>
    class MinoGenerator
    {
        //フィールド
        private List<Tetrimino> nextMinos;//次に控えているテトリミノリスト
        private Random rand;
        private int kindOfMino;
        private const int VISIBLE_NUM = 3;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MinoGenerator()
        {
            nextMinos = new List<Tetrimino>();
            rand = DeviceManager.CreateInstance().GetRandom();
            kindOfMino = Enum.GetValues(typeof(Form_mino)).Length;

            //最初にテトリミノを追加する
            for(int i = 0; i < VISIBLE_NUM; i++)
            {
                GenerateEndOfNextMino();
            }
        }

        /// <summary>
        /// ネクストテトリミノの末尾を生成
        /// </summary>
        public void GenerateEndOfNextMino()
        {
            var form = (Form_mino)(rand.Next(2, kindOfMino));
            nextMinos.Add(new Tetrimino(form));
        }

        /// <summary>
        /// ネクストミノの先頭を受け渡す
        /// </summary>
        /// <returns></returns>
        public Tetrimino PickHeaderMino()
        {
            var head = nextMinos[0];
            nextMinos.RemoveAt(0);

            return head;
        }

        public List<Tetrimino> GetNextTetriminos()
        {
            return nextMinos;
        }
    }
}
