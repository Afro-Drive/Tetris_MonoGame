using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Device
{
    /// <summary>
    /// SE(WAV形式)ダウンロード用Loader継承クラス
    /// 作成者:谷　永吾
    /// 作成開始日:2018年11月7日
    /// </summary>
    class SE_Loader : Loader
    {
        private SoundManager soundManager;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setResouces"></param>
        public SE_Loader(string[,] setResouces)
            : base(setResouces)
        {
            soundManager = DeviceManager.CreateInstance().GetSound();
            base.Initialze();
        }

        /// <summary>
        /// 更新(SEファイルを読み込み)
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //まずは終了フラグを有効にする
            isEndFlag = true;
            //カウンタが登録最大数に達していないか確認
            if (counter < maxNum)
            {
                //SEを読み込む
                soundManager.LoadSE(resources[counter, 0], resources[counter, 1]);
                //カウンターを加算
                counter += 1;
                //読み込むものがあったので終了フラグを偽(false)にセット
                isEndFlag = false;
            }
        }
    }
}
