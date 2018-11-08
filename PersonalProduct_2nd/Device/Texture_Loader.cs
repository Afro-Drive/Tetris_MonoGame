using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Device
{
    /// <summary>
    /// 画像読み込み用ローダー継承クラス
    /// 作成者:谷　永吾
    /// 作成開始日:2018年11月7日
    /// </summary>
    class Texture_Loader : Loader
    {
        private Renderer renderer;//描画オブジェクト
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setResources"></param>
        public Texture_Loader(string[,] setResources)
            : base(setResources)
        {
            renderer = DeviceManager.CreateInstance().GetRenderer();
            base.Initialze();//親クラスの初期化メソッドで初期化
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //まずは終了フラグを立てる
            isEndFlag = true;
            //カウンタが最大値に達していないか確認
            if (counter < maxNum)
            {
                //画像を読み込み
                renderer.LoadContent(resources[counter, 0], resources[counter, 1]);
                //カウンタを加算
                counter += 1;
                //読み込むアセットがあったので、終了フラグを継続にセット
                isEndFlag = false;
            }
        }
    }
}
