using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// Cellを継承した視認可能ブロッククラス
    /// 作成者:谷永吾
    /// 作成開始日:谷永吾
    /// </summary>
    class Block : Cell
    {
        protected bool onFieldFlag; //画面内表示フラグ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mediator">ゲーム仲介者</param>
        public Block(IGameMediator mediator)
            : base("black", mediator)
        {
            onFieldFlag = true ;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            onFieldFlag = true;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
        }
    }
}
