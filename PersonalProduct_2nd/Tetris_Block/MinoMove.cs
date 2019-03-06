using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Define;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// テトリミノの移動処理担当クラス
    /// 作成開始日:2019年2月14日
    /// 作成者:谷 永吾
    /// </summary>
    class MinoMove
    {
        private Vector2 moveValue; //移動量
        private Tetrimino target; //移動を施すテトリミノオブジェクト

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">移動処理を施したいテトリミノ</param>
        public MinoMove(Tetrimino target)
        {
            //引数受け取り
            SetTarget(target);
            //this.target = target;

            //移動量をゼロで初期化
            moveValue = Vector2.Zero;
        }

        /// <summary>
        /// テトリミノを右移動
        /// </summary>
        /// <returns>ブロック1つ分右の移動量</returns>
        public void LetMinoMoveR()
        {
            //ターゲットが固定状態でなければ
            if (!target.IsLocked())
            {
                target.Position += new Vector2(Size.WIDTH, 0);
            }
        }

        /// <summary>
        /// テトリミノを左移動
        /// </summary>
        /// <returns>ブロック1つ分左の移動</returns>
        public void LetMinoMoveL()
        {
            //ターゲットが固定状態でなければ
            if (!target.IsLocked())
            {
                target.Position += new Vector2(-Size.WIDTH, 0);
            }
        }

        /// <summary>
        /// テトリミノを落下移動
        /// </summary>
        public void LetMinoFall()
        {
            //ターゲットが固定状態でなければ
            if (!target.IsLocked())
            {
                target.Position += new Vector2(0, Size.HEIGHT);
            }
        }

        /// <summary>
        /// 移動対象のテトリミノの着地状態を設定
        /// →越権行為によりMinoStateManagerクラスに委託
        /// </summary>
        /// <param name="state">true→着地　false→離陸</param>
        //public void SetLandState(bool state)
        //{
        //    target.LandSwich(state);
        //}

        /// <summary>
        /// 移動処理を施す対象を設定する
        /// </summary>
        /// <param name="target">動かしたいターゲット</param>
        public void SetTarget(Tetrimino target)
        {
            this.target = target;
        }
    }
}
