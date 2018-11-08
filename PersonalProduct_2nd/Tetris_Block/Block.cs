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
        /// <param name="name"></param>
        /// <param name="mediator">ゲーム仲介者</param>
        public Block()
            : base("black")
        {
            onFieldFlag = true ;
        }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="other">コピー元のオブジェクトインスタンス</param>
        public Block(Block other)
            : this()
        {   }

        /// <summary>
        /// コピー
        /// </summary>
        /// <returns>コピーコンストラクタ</returns>
        public override object Clone()
        {
            return new Block(this); //コピーコンストラクタを生成・返却
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
