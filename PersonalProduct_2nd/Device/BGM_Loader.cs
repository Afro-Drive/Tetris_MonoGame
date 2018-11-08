using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Device
{
    /// <summary>
    /// BGM(MP3形式)読み込み用Loader継承クラス
    /// 作成者:谷　永吾
    /// 作成開始日:2018年11月7日
    /// </summary>
    class BGM_Loader : Loader
    {
        private SoundManager soundManager;

        /// <summary>
        ///　コンストラクタ
        /// </summary>
        /// <param name="setResource"></param>
        public BGM_Loader(string[,] setResource)
            : base(setResource)
        {
            soundManager = DeviceManager.CreateInstance().GetSound();
            base.Initialze();
        }

        /// <summary>
        /// 更新(BGMファイルを読み込み）
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //まずは終了フラグを有効にする
            isEndFlag = true;
            //カウンタが最大登録数に達していないか確認
            if (counter < maxNum)
            {
                //BGMを読み込む
                soundManager.LoadBGM(resources[counter, 0], resources[counter, 1]);
                //カウンタを加算
                counter += 1;
                //読み込むものがあったので終了フラグを偽(継続)としてセット
                isEndFlag = false;
            }
        }
    }
}
