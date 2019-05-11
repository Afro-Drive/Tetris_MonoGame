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
        private IControllerMediator mediator;

        public NextMinoBoard(IControllerMediator mediator)
        {
            this.mediator = mediator;
            nextMinos = mediator.GetNextMinos();
            holdMino = null;
            middleKeepMino = null;

            CanHold = true;
        }

        public void UpdateNext()
        {
            nextMinos = mediator.GetNextMinos();
        }

        public void UpdateHold()
        {
            if (Input.GetKeyTrigger(Keys.Space) && CanHold)
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
            renderer.DrawTexture("next", new Vector2(1100, 45));
            renderer.DrawTexture("hold", new Vector2(1100, 400));
            for (int i = 0; i < nextMinos.Count; i++)
            {
                nextMinos[i].DrawNoOffset(renderer, new Vector2(1200 + 256 * i, 230));
            }

            if (holdMino != null)
                holdMino.DrawNoOffset(renderer, new Vector2(1200, 600));
        }

        /// <summary>
        /// 最初にホールド機能を使う場合のホールド手順
        /// </summary>
        private void NullHold()
        {
            //ホールドオブジェクトに落下中のミノを収容
            Tetrimino currentTarget = mediator.GetActiveMino();
            currentTarget.Initialize();
            holdMino = currentTarget;
            //落下ミノを更新
            mediator.OrderToPickHead();
            //末尾にテトリミノを追加
            mediator.OrderToGenerate();
            //更新された落下ミノを取得
            Tetrimino newTarget = mediator.GetActiveMino();
            newTarget.Initialize();
            //各種テトリミノ制御オブジェクトの制御対象を更新
            mediator.OrderToSetNewMinoActive(newTarget);

            CanHold = false;
        }

        /// <summary>
        /// 標準ホールド
        /// </summary>
        private void NormalHold()
        {
            //中間保持者にホールド状態のミノを受け渡し
            middleKeepMino = holdMino;
            //落下中のミノをホールドに格納
            Tetrimino currentTarget = mediator.GetActiveMino();
            currentTarget.Initialize();
            holdMino = currentTarget;
            middleKeepMino.Initialize();
            //中間保持者にホールド状態のミノを受け渡す
            mediator.OrderToSetNewMinoActive(middleKeepMino);

            middleKeepMino = null;
            CanHold = false;
        }

        public bool CanHold
        {
            get; set;
        }
    }
}
