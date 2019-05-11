using Microsoft.Xna.Framework;
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
        private IControllerMediator controlMediator; //テトリミノ制御オブジェクト間の仲介者
        private IGameMediator gameMediator; //ゲーム仲介者

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">制御を行うテトリミノオブジェクト</param>
        /// <param name="jagArrayData">制御対象のテトリミノが含まれるフィールドデータ</param>
        /// <param name="controlMediator">テトリミノの制御の仲介者</param>
        public MinoCordinateController(Tetrimino target, IControllerMediator controlMediator, IGameMediator gameMediator)
        {
            SetTarget(target);
            this.controlMediator = controlMediator;
            this.field = controlMediator.GetFieldArray();
            this.gameMediator = gameMediator;
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
                }
            }

            //動作可能性を返却
            return CanMove;
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
            //距離に応じたスコアを加算
            var dropVal = (int)((landPosY - target.Position.Y) / Size.HEIGHT);
            gameMediator.AddScore(10 * dropVal);

            return new Vector2(target.Position.X, landPosY);
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
                willRotateArray = GetClockwise_RotatedArray();
            //Dキー入力→テトリミノが反時計回りに90回転した際の配列
            if (Input.GetKeyTrigger(Keys.D))
                willRotateArray = GetAntiClockwise_RotatedArray();

            //テトリミノの構成ブロックの座標を取得、一つずつ調べる
            foreach (var point in GetArrayUnitPos(willRotateArray))
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
        }

        public int[,] GetClockwise_RotatedArray()
        {
            int[,] target_Array = controlMediator.GetActiveMino().GetRotate_Array();
            int cols = target_Array.GetLength(0); //列(横)
            int rows = target_Array.GetLength(1); //行(縦)
            int[,] image_Array = new int[cols, rows]; //回転前とは行と列を逆にした配列を生成

            for (int y = 0; y < cols; y++) //回転後の配列の列に回転前の配列の行分要素を用意
            {
                for (int x = 0; x < rows; x++) //回転後の配列の行に回転前の配列の列分要素を用意
                {
                    //回転後の配列の列は、回転前の配列の行からy(新規配列の列の生成回数)と1を引いたものに一致する
                    //知るかこの野郎
                    image_Array[x, cols - y - 1] = target_Array[y, x];
                }
            }
            return image_Array;
        }

        public int[,] GetAntiClockwise_RotatedArray()
        {
            int[,] target_Array = controlMediator.GetActiveMino().GetRotate_Array();
            int cols = target_Array.GetLength(0); //列(横)
            int rows = target_Array.GetLength(1); //行(縦)
            int[,] image_Array = new int[rows, cols]; //回転前とは行と列を逆にした配列を生成

            for (int y = 0; y < cols; y++) //回転後の配列の列に回転前の配列の行分だけ要素を用意
            {
                for (int x = 0; x < rows; x++) //回転後の配列の行に回転前の配列の列分だけ要素を用意
                {
                    //回転後の配列の列は回転前の配列の行からx(回転後の列を用意した回数)と1を引いたものに一致する
                    //時計回りとはややこしくなる方が逆になるんだね。知らんけど。
                    image_Array[rows - x - 1, y] = target_Array[y, x];
                }
            }
            return image_Array;
        }

        /// <summary>
        /// テトリミノ構成配列から相対座標を取得
        /// </summary>
        /// <param name="checkArray">調べたい配列</param>
        /// <returns>配列内のブロックの座標を格納したリスト</returns>
        public List<Vector2> GetArrayUnitPos(int[,] checkArray)
        {
            //返却用のリストを生成
            List<Vector2> list_Pos = new List<Vector2>();

            //引数の配列内の要素を一つずつ確かめる
            for (int y = 0; y < checkArray.GetLength(0); y++)
            {
                for (int x = 0; x < checkArray.GetLength(1); x++)
                {
                    //確認した要素が0以外なら
                    if (checkArray[y, x] != 0)
                    {
                        //リストに追加する
                        //中心[2,2]を基準とした座標を算出
                        list_Pos.Add(
                            new Vector2(Size.WIDTH * (x - 2), Size.HEIGHT * (y - 2))
                            );
                    }
                }
            }
            //リストを返却する
            return list_Pos;
        }
    }
}
