using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Utility;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// テトリミノの状態を管理するクラス
    /// 作成者:谷 永吾
    /// 作成日:2019年2月16日
    /// </summary>
    class MinoStateManager
    {
        //フィールド
        //状態管理を行うテトリミノ
        private Tetrimino target;

        //各種トリガー
        private bool landON;//着地したか？
        private bool inputFallON;//落下キー入力受付

        //各種タイマー
        private CountDown_Timer landTimer; //着地後の操作猶予タイマー
        private CountDown_Timer fallTimer; //自動落下タイマー
        private CountDown_Timer inputFallTimer; //キー入力後落下猶予タイマー

        private IControllerMediator mediator;//テトリミノ制御の仲介者

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mediator"></param>
        public MinoStateManager(Tetrimino target, IControllerMediator mediator)
        {
            //引数受け取り
            SetTarget(target);
            this.mediator = mediator;

            //各種変数の初期化
            landON = false;
            IsLocked = false;
            inputFallON = false;

            landTimer = new CountDown_Timer(1.5f);
            fallTimer = new CountDown_Timer(0.5f);
            inputFallTimer = new CountDown_Timer(0.2f);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (inputFallON)
            {
                inputFallTimer.Update(gameTime);
            }
            else
            {
                inputFallTimer.Initialize();
            }

            if (landON) //着地状態なら
            {
                //着地タイマーを起動
                landTimer.Update(gameTime);
            }
            else //離陸したら
            {
                //初期化
                landTimer.Initialize();
            }

            if (landTimer.TimeUP()) //着地タイマーが時間切れ
            {
                //操作不可能とする(その場で位置を固定）
                IsLocked = true;
            }

            //落下タイマーの更新（初期化はLineFieldで行われる）
            fallTimer.Update(gameTime);

        }

        /// <summary>
        /// 各種データの初期化
        /// </summary>
        public void Initialize()
        {
            fallTimer.Initialize();
            inputFallTimer.Initialize();
            landTimer.Initialize();

            IsLocked = false;
            landON = false;
            inputFallON = false;
        }

        /// <summary>
        /// テトリミノの着地状態を設定
        /// </summary>
        /// <param name="state">true→着地状態　false→離陸状態</param>
        public void SetLandState(bool state)
        {
            if (!IsLocked)
            {
                landON = Convert.ToBoolean(state);
            }
        }

        /// <summary>
        /// キー入力落下状態の設定
        /// </summary>
        /// <param name="state"></param>
        public void SetInputFallState(bool state)
        {
            if (!IsLocked)
            {
                inputFallON = Convert.ToBoolean(state);
            }
        }

        /// <summary>
        /// テトリミノはキー入力後の落下ができる状態か？
        /// </summary>
        /// <returns>キー入力後の落下タイマーが時間切れになったかどうか</returns>
        public bool CanInputFall()
        {
            return inputFallTimer.TimeUP(); 
        }

        /// <summary>
        /// テトリミノの自動落下状態を取得
        /// </summary>
        /// <returns>自動落下タイマーが切れたらTrue</returns>
        public bool IsFall()
        {
            return fallTimer.TimeUP();
        }

        /// <summary>
        /// テトリミノのインプット落下タイマーの初期化
        /// </summary>
        public void ResetInputFallTimer()
        {
            if (!IsLocked)
            {
                inputFallTimer.Initialize();
            }
        }

        /// <summary>
        /// テトリミノの落下タイマーを初期化
        /// </summary>
        public void ResetFallTimer()
        {
            if (!IsLocked)
            {
                fallTimer.Initialize();
            }
        }

        /// <summary>
        /// テトリミノの落下タイマーを０にする
        /// </summary>
        public void ShutLandTimeDown()
        {
            if (!IsLocked) 
            {
                landTimer.ForceZero();
            }
        }

        /// <summary>
        /// 状態管理を行う対象を設定
        /// </summary>
        /// <param name="target">管理したいテトリミノ</param>
        public void SetTarget(Tetrimino target)
        {
            this.target = target;
        }

        /// <summary>
        /// テトリミノを固定させるか？
        /// </summary>
        public bool IsLocked
        {
            get;set;
        }
    }
}
