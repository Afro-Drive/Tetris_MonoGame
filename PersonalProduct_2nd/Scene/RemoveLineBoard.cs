using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// 消去したライン数を計測・表示するクラス
    /// </summary>
    class RemoveLineBoard
    {
        //フィールド
        private int removeCnt;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RemoveLineBoard()
        {
            removeCnt = 0;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //消去したライン数を描画する
            renderer.DrawTexture("delete", new Vector2(1400, 750));
            renderer.DrawNumber("number", new Vector2(1400, 824), removeCnt);
        }

        /// <summary>
        /// 消去したライン数を取得
        /// </summary>
        /// <returns>フィールドのremoveCnt</returns>
        public int GetRemoveLineValue()
        {
            return removeCnt;
        }

        /// <summary>
        /// 消去したラインを加算
        /// </summary>
        public void AddRemoveLine()
        {
            removeCnt++;
        }
        
        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
            removeCnt = 0;
        }
    }
}
