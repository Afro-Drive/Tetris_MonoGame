using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// Tetriminoオブジェクトへの制御オブジェクト間の仲介者
    /// </summary>
    interface IControllerMediator
    {
        //テトリミノが固定状態か
        bool IsMinoLocked();

        //テトリスを積むフィールドデータ取得
        int[][] GetFieldArray();

        //ネクストミノの情報受け渡し
        List<Tetrimino> GetNextMinos();

        //フィールド上に出ているミノの取得
        Tetrimino GetActiveMino();

        void OrderToSetNewMinoActive(Tetrimino newActiveMino);

        //テトリミノの生産要求
        void OrderToGenerate();

        void OrderToPickHead();

        int[,] GetClockwise_RotatedArray();

        int[,] GetAntiClockwise_RotatedArray();
    }
}
