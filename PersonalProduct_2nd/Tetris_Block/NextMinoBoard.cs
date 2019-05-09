using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// 次に排出されるテトリミノの表示ボード
    /// </summary>
    class NextMinoBoard
    {
        //フィールド
        private List<Tetrimino> nextMinos;
        private Tetrimino holdMino;
        private Tetrimino middleKeepMino;
        private bool canHold;
        private IControllerMediator mediator;

        public NextMinoBoard(IControllerMediator mediator)
        {
            this.mediator = mediator;
            nextMinos = mediator.GetNextMinos();
            holdMino = null;
            middleKeepMino = null;

            canHold = true;
        }

        public void UpdateNext()
        {
            nextMinos = mediator.GetNextMinos();
        }

        public void UpdateHold()
        {
            if (Input.GetKeyTrigger(Keys.Space))
            {
                if (holdMino == null)
                {
                    NullHold();
                    return;
                }
                NormalHold();
            }
        }

        public void Draw(Renderer renderer)
        {
            for (int i = 0; i < nextMinos.Count; i++)
            {
                nextMinos[i].Draw(renderer, new Vector2(970 + 256 * i, 500));
            }

            if (holdMino != null)
                holdMino.Draw(renderer, new Vector2(970, 700));
        }

        public void SetHoldSwitch(bool state)
        {
            canHold = state;
        }

        /// <summary>
        /// 最初にホールド機能を使う場合のホールド手順
        /// </summary>
        private void NullHold()
        {
            //ホールドオブジェクトに落下中のミノを収容
            holdMino = mediator.GetActiveMino();
            //落下ミノを更新
            mediator.OrderToPickHead();
            //末尾にテトリミノを追加
            mediator.OrderToGenerate();
            //更新された落下ミノを取得
            Tetrimino newTarget = mediator.GetActiveMino();
            newTarget.Initialize();
            //各種テトリミノ制御オブジェクトの制御対象を更新
            mediator.OrderToSetNewMinoActive(newTarget);
        }

        /// <summary>
        /// 標準ホールド
        /// </summary>
        private void NormalHold()
        {
            //中間保持者にホールド状態のミノを受け渡し
            middleKeepMino = holdMino;
            //落下中のミノをホールドに格納
            holdMino = mediator.GetActiveMino();
            middleKeepMino.Initialize();
            //中間保持者にホールド状態のミノを受け渡す
            mediator.OrderToSetNewMinoActive(middleKeepMino);
            middleKeepMino = null;
        }
    }
}
