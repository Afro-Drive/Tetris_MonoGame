using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target"></param>
        public MinoStateManager(Tetrimino target)
        {
            //引数受け取り
            SetTarget(target);
            //this.target = target;
        }

        /// <summary>
        /// テトリミノの着地状態を設定
        /// </summary>
        /// <param name="state">true→着地状態　false→離陸状態</param>
        public void SetLandState(bool state)
        {
            if (!target.IsLocked())
            {
                target.LandSwich(state);
            }
        }

        /// <summary>
        /// キー入力落下状態の設定
        /// </summary>
        /// <param name="state"></param>
        public void SetInputFallState(bool state)
        {
            if (!target.IsLocked())
            {
                target.InputFallSwitch(state);
            }
        }

        /// <summary>
        /// テトリミノはキー入力後の落下ができる状態か？
        /// </summary>
        /// <returns></returns>
        public bool CanInputFall()
        {
            return target.CanInputFall(); 
        }

        /// <summary>
        /// テトリミノの自動落下状態を取得
        /// </summary>
        /// <returns>自動落下タイマーが切れたらTrue</returns>
        public bool IsFall()
        {
            return target.IsFall();
        }

        /// <summary>
        /// テトリミノのインプット落下タイマーの初期化
        /// </summary>
        public void ResetInputFallTimer()
        {
            if (!target.IsLocked())
            {
                target.InitInputFall();
            }
        }

        /// <summary>
        /// テトリミノの落下タイマーを初期化
        /// </summary>
        public void ResetFallTimer()
        {
            if (!target.IsLocked())
            {
                target.InitFall();
            }
        }

        /// <summary>
        /// テトリミノの落下タイマーを０にする
        /// </summary>
        public void ShutLandTimeDown()
        {
            if (!target.IsLocked())
            {
                target.LandTimeZero();
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
        /// 移動可能かのプロパティ
        /// get→trueかfalseかを取得
        /// set→trueかfalseかを設定
        /// </summary>
        public bool CanMove
        {
            get; set;
        }
    }
}
