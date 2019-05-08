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
                    middleKeepMino = holdMino;
                    holdMino = mediator.GetActiveMino();
                    mediator.OrderToPickHead();
                    mediator.OrderToGenerate();
                    return;
                }
                middleKeepMino = holdMino;
                holdMino = mediator.GetActiveMino();
                mediator.OrderToSetNewMinoActive(middleKeepMino);
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
    }
}
