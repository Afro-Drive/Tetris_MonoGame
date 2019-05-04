﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
    /// テトリミノの動作可能性の検証・制御クラス
    /// </summary>
    class MinoCordinateController
    {
        //フィールド
        private Tetrimino target;//動作制御オブジェクト
        private int[][] field;//テトリミノの制御を行う配列データ
        private IControllerMediator mediator; //テトリミノ制御オブジェクト間の仲介者

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">制御を行うテトリミノオブジェクト</param>
        /// <param name="jagArrayData">制御対象のテトリミノが含まれるフィールドデータ</param>
        /// <param name="mediator">テトリミノの制御の仲介者</param>
        public MinoCordinateController(Tetrimino target, int[][] jagArrayData, IControllerMediator mediator)
        {
            SetTarget(target);
            this.field = jagArrayData;
            this.mediator = mediator;
            CanMove = true;
        }

        /// <summary>
        /// 制御対象を設定する
        /// </summary>
        /// <param name="newTarget">制御を行うテトリミノ</param>
        public void SetTarget(Tetrimino newTarget)
        {
            this.target = newTarget;
        }

        /// <summary>
        /// テトリミノが動作可能かのプロパティ
        /// </summary>
        public bool CanMove
        {
            get; set;
        }

        /// <summary>
        /// テトリミノは下部へ移動可能か？
        /// </summary>
        /// <returns></returns>
        public bool CanDown()
        {
            //まずは移動可能とする
            CanMove = true;
            //canMove = true;

            //出現中のテトリミノの構成ブロックの回転中心からの相対座標を取得
            //それを一つずつ取り出し、フィールド内での位置を取得する
            foreach (var point in target.GetMinoUnitPos())
            {
                //構成ブロックに対応した配列位置を取得
                int unitPos_X = (int)(target.Position.X + point.X) / Size.WIDTH;
                int unitPos_Y = (int)(target.Position.Y + point.Y) / Size.WIDTH;

                //構成ブロックの1ブロック分下部のブロックが0（＝空白）以外なら
                if (field[unitPos_Y + 1][unitPos_X] != 0)
                {
                    //移動不可能にする
                    CanMove = false;

                    //テトリミノを着地状態とする
                    //→これはLinefieldでStateManagerが直接行う
                    //stateRole.SetLandState(true);
                }
            }

            //動作可能性を返却
            return CanMove;
            //構成ブロックを全て調べて、条件に抵触しなければ移動する
            //→これはLineFieldで行う

            //if (stateRole.CanMove)
            //{
            //    //テトリミノを離陸状態とする
            //    stateRole.SetLandState(false);
            //    //キー入力による落下要求の場合
            //    if (Input.GetKeyState(Keys.Down))
            //    {
            //        //キー入力による猶予タイマーを起動
            //        stateRole.SetInputFallState(true);
            //        //その猶予タイマーが終了したらミノを落下させる
            //        if (stateRole.CanInputFall())
            //            moveRole.LetMinoFall();
            //    }
            //    //テトリミノの自動落下ならタイマーを初期化
            //    if (stateRole.IsFall())
            //    {
            //        //テトリミノを落下移動させる
            //        moveRole.LetMinoFall();
            //        stateRole.ResetFallTimer();
            //    }
            //}
        }

        /// <summary>
        /// テトリミノが左右に移動可能か？
        /// </summary>
        /// <returns></returns>
        public bool CanMoveX()
        {
            CanMove = true;

            //テトリミノが右に移動しようとしている
            if (Input.GetKeyTrigger(Keys.Right))
            {
                //出現中のテトリミノの構成ブロックの回転中心からの相対座標を取得
                //それを一つずつ取り出し、フィールド内での位置を取得する
                foreach (var point in target.GetMinoUnitPos())
                {
                    //構成ブロックに対応するフィールドの配列位置を取得
                    int unitPos_X = (int)(target.Position.X + point.X) / Size.WIDTH;
                    int unitPos_Y = (int)(target.Position.Y + point.Y) / Size.HEIGHT;

                    //テトリミノ本体の座標と相対座標の和に右側隣接するマップ要素がSpace以外なら
                    if (field[unitPos_Y][unitPos_X + 1] != 0)
                    {
                        //移動不能に通知
                        CanMove = false;
                    }
                }
                //LineFieldで直接行う
                ////全て取り出し切って、一つも条件に抵触しなければ
                //if (minoStateManager.CanMove)
                //    minoMove.LetMinoMoveR(); //テトリミノを右移動させる
                //tetrimino.MoveR(); //移動する
            }

            //テトリミノが左に移動しようとしている
            if (Input.GetKeyTrigger(Keys.Left))
            {
                //出現中のテトリミノの構成ブロックの回転中心からの相対座標を取得
                //それを一つずつ取り出し、フィールド内での位置を取得する
                foreach (var point in target.GetMinoUnitPos())
                {
                    //構成ブロックに対応するフィールドの配列位置を取得
                    int unitPos_X = (int)(target.Position.X + point.X) / Size.WIDTH;
                    int unitPos_Y = (int)(target.Position.Y + point.Y) / Size.HEIGHT;

                    //テトリミノ本体の座標と相対座標の和に左側隣接するマップ要素が０以外なら
                    if (field[unitPos_Y][unitPos_X - 1] != 0)
                    {
                        //移動不能に通知
                        CanMove = false;
                    }
                }

                //LineFieldで直接行う
                //全て取り出し切って、一つも条件に抵触しなければ
                //if (minoStateManager.CanMove)
                //    minoMove.LetMinoMoveL(); //テトリミノを左移動させる
                //tetrimino.MoveL(); //移動する
            }

            return CanMove;
        }

        /// <summary>
        /// 超高速落下時の着地予定地点の計算
        /// </summary>
        /// <returns>落下時の落下予定地点</returns>
        public Vector2 CalcHardFallPos()
        {
            CanMove = true;

            //ミノ構成ブロックと着地点までの距離リストを用意
            var toLandVal = new List<int>();
            //テトリミノの構成ブロックを一つずつ取り出し
            foreach (var point in target.GetMinoUnitPos())
            {
                //構成ブロックの座標に対応する要素番号の特定
                int unitPos_X = (int)(target.Position.X + point.X) / Size.WIDTH;
                int unitPos_Y = (int)(target.Position.Y + point.Y) / Size.HEIGHT;
                //落下中のミノの下部のフィールド位置の要素を検証
                for (int i = 1; i < field.GetLength(0) - unitPos_Y; i++)
                {
                    //0以外の要素が出てきた時点で距離を記録
                    if (field[unitPos_Y + i][unitPos_X] != 0)
                    {
                        toLandVal.Add(unitPos_Y + i);
                        break;//for文のループを脱出
                    }
                }
            }
            //最も近距離の値の一つ上を着地予定地Yと定める
            var landPosY = (toLandVal.Min() - 1) * Size.HEIGHT;

            return new Vector2(target.Position.X, landPosY);

            //LineFieldで直接行う
            //    //着地予定地まで一気に落下させる
            //    minoMove.HardFall(new Vector2(tetrimino.Position.X, landPosY));
            //    //ミノの着地時間も終了状態にする
            //    minoStateManager.ShutLandTimeDown();
            //    minoStateManager.SetLandState(true);
        }

        /// <summary>
        /// テトリミノが回転可能か？
        /// </summary>
        /// <returns></returns>
        public bool CanRotate()
        {
            CanMove = true;

            //回転配列をカラの状態で生成
            int[,] willRotateArray = null;

            //Aキー入力→テトリミノが時計回りに90度回転した際の配列を取得
            if (Input.GetKeyTrigger(Keys.A))
                willRotateArray = target.GetClockwise_RotatedArray();
            //Dキー入力→テトリミノが反時計回りに90回転した際の配列
            if (Input.GetKeyTrigger(Keys.D))
                willRotateArray = target.GetAntiClockwise_RotatedArray();

            //テトリミノの構成ブロックの座標を取得、一つずつ調べる
            foreach (var point in target.GetRotatedUnitPos(willRotateArray))
            {
                //構成ブロックに対応した配列位置を取得
                int unitPos_X = (int)(target.Position.X + point.X) / Size.WIDTH;
                int unitPos_Y = (int)(target.Position.Y + point.Y) / Size.WIDTH;

                //対応するフィールドの位置がブロックならば
                //配列指定位置が負の数の場合も
                if (unitPos_X <= 0 || 
                    unitPos_Y <= 0 || 
                    field[unitPos_Y][unitPos_X] != 0)
                {
                    //回転不可能とする
                    CanMove = false;
                    //ループ終了
                    break;
                }
            }

            return CanMove;

            //LineFieldで直接行うMoveが直接行う
            //条件に抵触しなければ入力キーに応じて回転処理
            //if (minoStateManager.CanMove)
            //{
            //    if (Input.GetKeyTrigger(Keys.A))
            //        //テトリミノを回転する(後でMinoMoveに行わせる)
            //        minoMove.LetMinoRotate_Clockwise();
            //    //tetrimino.Rotate_Clockwise();
            //    else if (Input.GetKeyTrigger(Keys.D))
            //        //テトリミノを反時計回りに回転
            //        minoMove.LetMinoRotate_AntiClockwise();
            //    //tetrimino.Rotate_AntiClockwise();
            //}
        }

    }
}
