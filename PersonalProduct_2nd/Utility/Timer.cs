using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Utility
{
    /// <summary>
    /// タイマー抽象クラス
    /// </summary>
    abstract class Timer
    {
        //子クラスでも使えるようにprivate→protected
        protected float limitTime;
        protected float currentTime;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="second">秒数</param>
        public Timer(float second)
        {

            limitTime = 60 * second;//60fps/秒)
        }

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public Timer() : this(1)
        { }

        //抽象メソッド
        public abstract void Initialize();//初期化
        public abstract void Update(GameTime gameTime);//更新
        public abstract bool TimeUP();//時間切れか？
        public abstract float Rate();//制限時間における現在時間の割合

        /// <summary>
        /// 制限時間を設定
        /// </summary>
        /// <param name="second"></param>
        public void SetTime(float second)
        {
            limitTime = 60 * second;
        }

        /// <summary>
        /// 現在時間の取得
        /// </summary>
        /// <returns>秒</returns>
        public float Now()
        {
            return currentTime / 60;//60fpsなので60で割る
        }
    }
}
