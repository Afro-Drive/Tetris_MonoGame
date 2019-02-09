using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;
using PersonalProduct_2nd.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// Cellを継承した視認可能ブロッククラス
    /// 作成者:谷永吾
    /// 作成開始日:谷永吾
    /// </summary>
    class Block : Cell
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mediator">ゲーム仲介者</param>
        public Block()
            : base("black")
        {
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
        }

        /// <summary>
        /// ピクセル画像の描画
        /// (Tetriminoオブジェクト用)
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        public void DrawMino(Renderer renderer, string minoCol, Vector2 position, float scale)
        {
            renderer.DrawTexture(
                minoCol,
                position,
                null, 
                scale,
                Vector2.Zero);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
        }

        public override void Hit(Cell other)
        {
        }
    }
}
