using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Define;
using PersonalProduct_2nd.Device;
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
        private IControllerMediator mediator; //テトリミノ制御の仲介者

        private SoundManager sound;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">移動処理を施したいテトリミノ</param>
        /// <param name="mediator"></param>
        public MinoMove(Tetrimino target, IControllerMediator mediator)
        {
            //引数受け取り
            SetTarget(target);
            this.mediator = mediator;

            sound = DeviceManager.CreateInstance().GetSound();

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
            if (!mediator.IsMinoLocked())
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
            if (!mediator.IsMinoLocked())
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
            if (!mediator.IsMinoLocked())
            {
                target.Position += new Vector2(0, Size.HEIGHT);
            }
        }

        /// <summary>
        /// テトリミノを時計回りに回転させる
        /// </summary>
        public void LetMinoRotate_Clockwise()
        {
            if (!mediator.IsMinoLocked())
            {
                int[,] rotated = mediator.GetClockwise_RotatedArray();
                sound.PlaySE("minospin");
                target.SetArray(rotated);
            }
        }

        /// <summary>
        /// テトリミノを反時計回りに90度回転させる
        /// </summary>
        public void LetMinoRotate_AntiClockwise()
        {
            if (!mediator.IsMinoLocked())
            {
                int[,] antiRotated = mediator.GetAntiClockwise_RotatedArray();
                sound.PlaySE("minospin");
                target.SetArray(antiRotated);
            }
        }

        /// <summary>
        /// 移動処理を施す対象を設定する
        /// </summary>
        /// <param name="target">動かしたいターゲット</param>
        public void SetTarget(Tetrimino target)
        {
            this.target = target;
        }

        /// <summary>
        /// 落下タイマーを無視した超高速落下
        /// </summary>
        /// <param name="landPos">落下後の着地地点</param>
        public void HardFall(Vector2 landPos)
        {
            if (!mediator.IsMinoLocked())
            {
                //ミノの座標を着地位置まで一気に移動
                target.Position = landPos;
                sound.PlaySE("hardfall");
            }
        }

        /// <summary>
        /// 落下中のミノの影を落とす位置の計算
        /// </summary>
        /// <returns>ミノの影を投射する座標</returns>
        public Vector2 CalcMinoShadowPos()
        {
            int[][] data = mediator.GetFieldArray();

            //ミノ構成ブロックと着地点までの距離リストを用意
            var unitPosList_Y = new List<int>();
            var toLandVal = new List<int>();

            var currentPosX = (int)(this.target.Position.X) / Size.WIDTH;
            var currentPosY = (int)(this.target.Position.Y) / Size.HEIGHT;

            //テトリミノの構成ブロックを一つずつ取り出し
            foreach (var point in target.GetMinoUnitPos())
            {
                //構成ブロックのY座標に対応する要素番号の特定
                int unitPos_Y = (int)(target.Position.Y + point.Y) / Size.HEIGHT;

                //それぞれをリストに格納
                unitPosList_Y.Add(unitPos_Y);
            }

            //格納したユニットの座標を1つずつ検証
            foreach (var pointY in unitPosList_Y)
            {
                //落下中のミノの下部のフィールド位置の要素を検証
                for (int i = 1; i < data.GetLength(0) - pointY; i++)
                {
                    //0以外の要素が出てきた時点で距離を記録
                    if (data[pointY + i][currentPosX] != 0)
                    {
                        toLandVal.Add(i);
                        break;//記録後にforのループを脱出
                    }
                }
            }
            //ブロックとの距離が最短の座標-1を着地予定地Yと定める
            int landPosY = currentPosY + toLandVal.Min() - 1;
            int shadowPosY = landPosY * Size.HEIGHT;
            //影を投射する座標を返却
            return new Vector2(target.Position.X, shadowPosY);
        }

    }
}
