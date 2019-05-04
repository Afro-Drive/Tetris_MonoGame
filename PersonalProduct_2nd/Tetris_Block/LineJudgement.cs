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
    /// LineFieldのライン審判クラス
    /// </summary>
    class LineJudgement
    {
        //フィールド
        private int[][] judgeData; //審判を行うラインデータ
        private IControllerMediator mediator;//テトリミノ制御の仲介者
        private const int DEAD_LINE = 3;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setData">審判に使う配列データ</param>
        /// <param name="mediator"></param>
        public LineJudgement(int[][] setData, IControllerMediator mediator)
        {
            this.judgeData = setData;
            this.mediator = mediator;
        }

        /// <summary>
        /// 検証後のデータを返却
        /// </summary>
        /// <returns></returns>
        public int[][] ReflectData()
        {
            return judgeData;
        }

        /// <summary>
        /// 固定状態のテトリミノをフィールドに反映させる
        /// </summary>
        /// <param name="minoUnitRelatPos">フィールドに反映させたいテトリミノのフィールドにおける相対座標</param>
        /// <param name="unitNum">固定状態のテトリミノの要素番号</param>
        public void RefToField(List<Vector2> minoUnitRelatPos, int unitNum)
        {
            foreach (var point in minoUnitRelatPos)
            {
                //構成ブロックのフィールドにおける相対座標を算出
                int unitPos_Y = (int)(point.Y / Size.HEIGHT);
                int unitPos_X = (int)(point.X / Size.WIDTH);

                //対応する場所にミノの構成番号を代入
                judgeData[unitPos_Y][unitPos_X] = unitNum;
            }

            //横一列が揃ったか検証、揃ったらラインを消去
            RemoveAndFillLine();
        }

        /// <summary>
        /// ラインを消去し、空白部を詰める
        /// </summary>
        private void RemoveAndFillLine()
        {
            FillLine(RemoveLine());
        }

        /// <summary>
        /// ラインを消去する
        /// </summary>
        /// <returns>空白状態となった列番号を格納したリスト</returns>
        private List<int> RemoveLine()
        {
            //消去対象の列がなければNullを返却
            if (PickUpRemoveLineNum().Count == 0)
                return null;

            //返却用のリストを生成
            List<int> removeLineNum = PickUpRemoveLineNum();

            //消去対象の列番号内の要素を全て０にする
            foreach (var lineNum in PickUpRemoveLineNum())
            {
                //列番号内の要素を一つずつ書き換え
                for (int row = 0; row < judgeData[lineNum].Length; row++)
                {
                    //右端と左端は飛ばす
                    if (row == 0 ||
                        row == judgeData[lineNum].Length - 1)
                        continue;

                    //要素を０にする
                    judgeData[lineNum][row] = 0;
                }
                //消去したライン数を加算
                RemoLineCnt++;
                //スコアを加算する
                AddScoreVal += 100;
            }

            //詰める必要のある列数を格納したリストを返却
            //詰める処理にバトンタッチ
            return removeLineNum;
        }

        /// <summary>
        /// 消去対象の列番号を特定
        /// </summary>
        /// <returns>消去対象の列番号を格納したリスト</returns>
        private List<int> PickUpRemoveLineNum()
        {
            //返却用のリストを生成
            List<int> removeLineNum = new List<int>();

            //フィールドのデータの要素を下段から一つずつ確認する
            //(ただし下端は除外)
            for (int y = judgeData.GetLength(0) - 2; y > 0; y--)
            {
                //列内に0が含まれていた場合は除外
                if (judgeData[y].Contains(0))
                    continue;

                //上記の条件に抵触しなければその列番号をリストに追加する
                removeLineNum.Add(y);
            }
            //リストを返却
            return removeLineNum;

        }

        /// <summary>
        /// 空白状態となった列を詰める
        /// </summary>
        /// <param name="removeLine"></param>
        private void FillLine(List<int> removeLine)
        {
            //特に削除する列がなければ終了
            if (removeLine == null)
                return;

            //消去対象の列の個数だけ繰り返す
            for (int i = 0; i < removeLine.Count; i++)
            {
                //列番号が連続していない場合は飛ばす
                //ここがうまくいかない…
                if (i < removeLine.Count - 1
                    && Math.Abs(removeLine[i] - removeLine[i + 1]) != 1)
                {
                    continue;
                }
                //消去対象の列番号のうち最大の列番号からコピーを行う
                //(ただし下端・上端は除外)
                //ループ方向は下の列から上の列なのに注意
                for (int col = removeLine.Max(); col > 1; col--)
                {
                    //一段上の列をコピーする
                    judgeData[col - 1].CopyTo(judgeData[col], 0);
                }
            }
        }

        /// <summary>
        /// デッドラインにブロックが残っているか判定
        /// </summary>
        /// <returns>ブロックが残っている→真
        /// 残っていない→偽</returns>
        public bool DeadCheck()
        {
            //fliedData内で、デッドラインの領域を調べる
            for (int col = 0; col < DEAD_LINE; col++)
            {
                //ただし、両端は飛ばす
                for (int row = 1; row < judgeData[col].Length - 1; row++)
                {
                    //その配列内に0以外の要素があった時点で終了
                    if (judgeData[col][row] != 0)
                        return true;
                }
            }
            //普段は偽を返却
            return false;
        }

        /// <summary>
        /// 消去した行数のプロパティ
        /// </summary>
        public int RemoLineCnt
        {
            get;set;
        }

        /// <summary>
        /// 加算スコアのプロパティ
        /// </summary>
        public int AddScoreVal
        {
            get;set;
        }
    }
}
