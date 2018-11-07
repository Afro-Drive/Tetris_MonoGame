using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Utility
{
    /// <summary>
    /// タイマー継承カウントダウンクラス
    /// 作成者:谷　永吾
    /// </summary>
    class CountDown_Timer : Timer
    {
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public CountDown_Timer()
            : this(1f)
        { }

        /// <summary>
        /// コンストラクタ(引数あり)
        /// </summary>
        /// <param name="second"></param>
        public CountDown_Timer(float second)
            : base(second)
        {
            currentTime = limitTime;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            currentTime = limitTime;
        }

        /// <summary>
        /// 制限時間における現在時間の割合を返却
        /// </summary>
        /// <returns>起動時→0　終了時→1</returns>
        public override float Rate()
        {
            return 1.0f - currentTime / limitTime;
        }

        /// <summary>
        /// 時間切れか？
        /// </summary>
        /// <returns>現在時間が0になったらtrue, 普段はfalse</returns>
        public override bool TimeUP()
        {
            return currentTime <= 0.0f;
        }

        /// <summary>
        /// タイマーの起動
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //現在時間を1フレームあたり1ずつ減少させ、60回目で1秒分経過する
            currentTime -= 1f;
            //現在時間と0.0で大きい方を返却する
            currentTime = Math.Max(currentTime, 0.0f);
        }
    }
}
