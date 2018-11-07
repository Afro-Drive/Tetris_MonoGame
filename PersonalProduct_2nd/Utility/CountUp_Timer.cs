using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Utility
{
    /// <summary>
    /// タイマー継承カウントアップクラス
    /// 作成者:谷永吾
    /// 作成開始日:2018年11月7日
    /// </summary>
    class CountUp_Timer : Timer
    {
        public CountUp_Timer()
            : base()
        {
            //自分の初期化メソッドで初期化
            Initialize();
        }

        public CountUp_Timer(float second)
            : base(second)
        {
            Initialize();
        }

        public override void Initialize()
        {
            //CountDownTimerと異なり、最初は０秒の状態で初期化する
            currentTime = 0.0f;
        }

        public override bool TimeUP()
        {
            //制限時間に達したら設定した時間を超えたのでtrueを返す=今回の時間切れは0.0fでなく、制限時間方向
            return currentTime >= limitTime;
        }

        public override float Rate()
        {
            return currentTime / limitTime;
        }

        public override void Update(GameTime gameTime)
        {
            //現在の時間を増やしていく。閾値はlimitTime
            currentTime = Math.Min(currentTime + 1.0f, limitTime);//引数のうち「小さい方」を返す
        }
    }
}
