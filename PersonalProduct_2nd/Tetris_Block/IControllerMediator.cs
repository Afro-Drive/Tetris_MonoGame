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
    }
}
