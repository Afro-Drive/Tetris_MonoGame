using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Utility;
using PersonalProduct_2nd.Device;

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

        //レベルに合わせたテトリミノの落下速度を格納したディクショナリ
        private Dictionary<int, float> minoSpeedDict;
        private int currentLv;

        private IControllerMediator controlMediator;//テトリミノ制御の仲介者
        private IGameMediator gameMediator;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target"></param>
        /// <param name="controlMediator"></param>
        public MinoStateManager(Tetrimino target, IControllerMediator controlMediator, IGameMediator gameMediator)
        {
            //引数受け取り
            SetTarget(target);
            this.controlMediator = controlMediator;
            this.gameMediator = gameMediator;

            //各種変数の初期化
            landON = false;
            IsLocked = false;
            inputFallON = false;

            //ディクショナリを初期化
            minoSpeedDict = new Dictionary<int, float>()
            {
                { 1, 1.0f },
                { 2, 0.8f },
                { 3, 0.7f },
                { 4, 0.6f },
                { 5, 0.5f },
                { 6, 0.35f },
                { 7, 0.25f },
                { 8, 0.17f },
                { 9, 0.12f },
                { 10, 0.05f },
            };
            currentLv = 1;

            landTimer = new CountDown_Timer(1.5f);
            fallTimer = new CountDown_Timer(minoSpeedDict[currentLv]);
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

            //レベルに応じたミノの落下速度設定
            ChangeMinoSpeed(gameMediator.GetLevel());

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
            get; set;
        }

        /// <summary>
        /// Tetriminoの落下速度を変える
        /// </summary>
        /// <param name="level">現在レベル</param>
        public void ChangeMinoSpeed(int level)
        {
            //レベルに変動がなければそのまま
            if (currentLv == level)
                return;

            currentLv = level;
            fallTimer.SetTime(minoSpeedDict[currentLv]);
        }
    }
}
