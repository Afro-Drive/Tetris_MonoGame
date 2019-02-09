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
    /// Cellを継承した空白セルクラス
    /// 作成者:谷永吾
    /// 作成開始日:谷永吾
    /// </summary>
    class Space : Cell
    {
        //フィールド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mediator">ゲーム仲介者</param>
        public Space()
            : base()
        { }

        public Space(Space other)
            :this()
        { }

        /// <summary>
        /// コピー
        /// </summary>
        /// <returns>コピーコンストラクタ</returns>
        public override object Clone()
        {
            return new Space(this); //コピーコンストラクタを生成・取得
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public override void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// 描画(何もしない)
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer)
        {            
        }

        public override void Hit(Cell other)
        {
        }
    }
}
