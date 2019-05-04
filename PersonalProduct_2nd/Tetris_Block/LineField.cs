using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Define;
using PersonalProduct_2nd.Device;
using PersonalProduct_2nd.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PersonalProduct_2nd.Tetris_Block.Tetrimino;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// Blockクラスに囲まれ、TetriMinoを積み重ねるプレイエリアクラス
    /// (Actionソリューションをお手本に作成)
    /// 作成者:谷永吾
    /// 作成開始日:2018年11月7日
    /// </summary>
    class LineField : IControllerMediator
    {
        #region フィールド
        private int[][] fieldData; //プレイ画面内のフィールドデータ
        private DeviceManager deviceManager; //ゲームデバイス
        private Tetrimino tetrimino; //フィールドに出現しているテトリミノ
        private ArrayRenderer arrayRenderer; //二次元配列描画オブジェクト
        private LineJudgement lineJudge;//フィールドの配列データ審判オブジェクト

        //各種Tetrimino制御オブジェクト
        private MinoMove minoMove; //テトリミノ移動オブジェクト
        private MinoStateManager minoStateManager; //テトリミノの状態管理オブジェクト
        private MinoCordinateController minoCordinate; //テトリミノの動作検証オブジェクト
        private IGameMediator mediator; //ゲーム仲介者

        private readonly int deadLine = 3; //ゲームオーバーか確認用の列番号
        #endregion フィールド

        /// <summary>
        ///　コンストラクタ
        /// </summary>
        /// <param name="device"></param>
        /// <param name="mediator"></param>
        public LineField(DeviceManager device, IGameMediator mediator)
        {
            deviceManager = device;
            this.mediator = mediator;

            //二次元配列描画オブジェクトを実体生成
            arrayRenderer = new ArrayRenderer(Size.offset);
        }

        public void Initialize()
        {
            //テトリミノの実体生成
            tetrimino = new Tetrimino();
            //ライン審判を実体生成
            lineJudge = new LineJudgement(fieldData, this);

            //各種テトリミノ管理者を生成、ターゲットを設定
            minoMove = new MinoMove(tetrimino, this);
            minoStateManager = new MinoStateManager(tetrimino, this);
            minoCordinate = new MinoCordinateController(tetrimino, fieldData, this);

            //最初は死亡フラグはOFFにする
            IsDeadFlag = false;
        }

        /// <summary>
        /// CSVReaderを使ってMapの読み込み
        /// (フィールドのmapListの要素を分析)
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        public void Load(string filename, string path = "./")
        {
            CSVReader csvReader = new CSVReader();
            csvReader.Read(filename, path);

            fieldData = csvReader.GetIntData(); //int[,]型で取得

            arrayRenderer.SetData(fieldData); //描画を行うための配列を受け渡す→RenderFieldメソッドの準備
        }

        /// <summary>
        /// マップリストのクリア
        /// </summary>
        public void Unload()
        {
        }

        /// <summary>
        /// フィールド内の空白要素以外を更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //テトリミノを更新
            tetrimino.Update(gameTime);
            minoStateManager.Update(gameTime);

            RefToField(); //テトリミノが凍結後にフィールドに反映

            //テトリミノが動ける状態か判定
            MoveLRCheck(); //左右移動
            MoveDCheck();　//下移動
            HardFallCheck(); //超高速落下移動
            RotateCheck(); //回転

            //ゲームオーバー領域にブロックが残っているか検証
            if (lineJudge.DeadCheck())
                IsDeadFlag = true;
        }

        ///<summary>
        ///固定状態のテトリミノに対応するフィールドの座標データへ反映
        ///</summary>
        public void RefToField()
        {
            if (minoStateManager.IsLocked)
            {
                var unitPosList = new List<Vector2>();
                //テトリミノの構成ブロックの座標を取得(配列？)
                foreach (var point in tetrimino.GetMinoUnitPos())
                {
                    //構成ブロックに対応するフィールドの配列位置を取得
                    int unitPos_X = (int)(tetrimino.Position.X + point.X);
                    int unitPos_Y = (int)(tetrimino.Position.Y + point.Y);

                    unitPosList.Add(new Vector2(unitPos_X, unitPos_Y));
                }

                ////対応する位置のフィールドの要素を書き換える
                lineJudge.RefToField(unitPosList, tetrimino.GetUnitNum());
                this.fieldData = lineJudge.ReflectData();

                //消去した列数に反映
                for (int i = 0; i < lineJudge.RemoLineCnt; i++)
                    mediator.AddRemoveLine();
                //スコアを加算
                mediator.AddScore(lineJudge.AddScoreVal);

                //加算用ラインとスコアを初期化
                lineJudge.RemoLineCnt = 0;
                lineJudge.AddScoreVal = 0;

                //状態管理者を初期化
                minoStateManager.Initialize();
                //一通り書き換えが終わったら初期化
                tetrimino.Initialize();
            }
        }

        /// <summary>
        /// テトリミノが左右移動可能か検証
        /// </summary>
        /// <param name="cell">基準となるセルオブジェクト</param>
        public void MoveLRCheck()
        {
            //全て取り出し切って、一つも条件に抵触しなければ
            if (minoCordinate.CanMoveX())
            {
                if (Input.GetKeyTrigger(Keys.Left))
                    minoMove.LetMinoMoveL(); //テトリミノを左移動させる
                if (Input.GetKeyTrigger(Keys.Right))
                    minoMove.LetMinoMoveR(); //テトリミノを右移動させる
            }
        }

        /// <summary>
        /// テトリミノが落下移動可能か検証
        /// </summary>
        public void MoveDCheck()
        {
            //テトリミノが下方に移動しようとしている
            //(下キーが入力された、または、テトリミノが落下状態)
            if (Input.GetKeyState(Keys.Down) || minoStateManager.IsFall())
            {
                //条件に抵触しなければ移動する
                if (minoCordinate.CanDown())
                {
                    //テトリミノを離陸状態とする
                    minoStateManager.SetLandState(false);
                    //キー入力による落下要求の場合
                    if (Input.GetKeyState(Keys.Down))
                    {
                        //キー入力による猶予タイマーを起動
                        minoStateManager.SetInputFallState(true);
                        //その猶予タイマーが終了したらミノを落下させる
                        if (minoStateManager.CanInputFall())
                            minoMove.LetMinoFall();
                    }
                    //テトリミノの自動落下ならタイマーを初期化
                    if (minoStateManager.IsFall())
                    {
                        //テトリミノを落下移動させる
                        minoMove.LetMinoFall();
                        minoStateManager.ResetFallTimer();
                    }
                }
                //動作不可となったら着地状態へ
                else
                {
                    minoStateManager.SetLandState(true);
                }
            }
            //キー入力が途切れたら、落下猶予タイマーを初期化
            else if (!Input.GetKeyState(Keys.Down))
                minoStateManager.ResetInputFallTimer();
        }

        /// <summary>
        /// 超高速落下が可能か調べる
        /// </summary>
        private void HardFallCheck()
        {
            //上キーが入力されたら検証開始
            if (Input.GetKeyTrigger(Keys.Up))
            {
                //着地予定地を計算し、そこへ一気に落下させる
                minoMove.HardFall(minoCordinate.CalcHardFallPos());
                //ミノの着地時間も終了状態にする
                minoStateManager.ShutLandTimeDown();
                minoStateManager.SetLandState(true);
            }
        }

        /// <summary>
        /// テトリミノが回転可能か調べる
        /// 必要に応じて座標を補正する
        /// </summary>
        public void RotateCheck()
        {
            //AかDキーが入力された
            if ((Input.GetKeyTrigger(Keys.A) || Input.GetKeyTrigger(Keys.D)) &&
                minoCordinate.CanRotate())//かつ、条件に抵触しなければ入力キーに応じて回転処理
            {
                if (Input.GetKeyTrigger(Keys.A))
                    //テトリミノを回転する(後でMinoMoveに行わせる)
                    minoMove.LetMinoRotate_Clockwise();
                else if (Input.GetKeyTrigger(Keys.D))
                    //テトリミノを反時計回りに回転
                    minoMove.LetMinoRotate_AntiClockwise();
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //テトリミノを描画
            tetrimino.Draw(renderer);
            //テトリミノの影の描画
            tetrimino.DrawShadow(renderer, CalcMinoShadowPos());

            //ゲーム状態に応じてフィールドを描画
            //if (DeadCheck())
            //{
            //積まれたブロックを赤く描画
            //→Endingシーンなどで色を変える予定
            //arrayRenderer.RenderJugField(renderer, Color.Red);
            //死亡フラグを立てる→ここでフラグをいじってはダメ
            //IsDeadFlag = true;
            //}
            //else

            arrayRenderer.RenderJugField(renderer, Color.White);
        }

        /// <summary>
        /// 落下中のミノの影を落とす位置の計算
        /// </summary>
        /// <returns></returns>
        private Vector2 CalcMinoShadowPos()
        {
            //ミノ構成ブロックと着地点までの距離リストを用意
            var unitPosList_Y = new List<int>();
            var toLandVal = new List<int>();
            var currentPosX = (int)(this.tetrimino.Position.X) / Size.WIDTH;
            var currentPosY = (int)(this.tetrimino.Position.Y) / Size.HEIGHT;

            //テトリミノの構成ブロックを一つずつ取り出し
            foreach (var point in tetrimino.GetMinoUnitPos())
            {
                //構成ブロックのY座標に対応する要素番号の特定
                int unitPos_Y = (int)(tetrimino.Position.Y + point.Y) / Size.HEIGHT;

                //それぞれをリストに格納
                unitPosList_Y.Add(unitPos_Y);
            }

            //格納したユニットの座標を1つずつ検証
            foreach (var pointY in unitPosList_Y)
            {
                //落下中のミノの下部のフィールド位置の要素を検証
                for (int i = 1; i < fieldData.GetLength(0) - pointY; i++)
                {
                    //0以外の要素が出てきた時点で距離を記録
                    if (fieldData[pointY + i][currentPosX] != 0)
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
            return new Vector2(tetrimino.Position.X, shadowPosY);
        }

        /// <summary>
        /// ラインを消去し、詰める
        /// </summary>
        //public void RemoveAndFillLine()
        //{
        //    //消去されたラインを詰める
        //    FillLine(RemoveLine());
        //}

        /// <summary>
        /// ブロックが一列揃った列番号を取得
        /// </summary>
        /// <returns>一列揃った列番号を格納したリスト</returns>
        //public List<int> PickUpRemoveLineNum()
        //{
        //    //返却用のリストを生成
        //    List<int> removeLineNum = new List<int>();

        //    //フィールドのデータの要素を下段から一つずつ確認する
        //    //(ただし下端は除外)
        //    for (int y = fieldData.GetLength(0) - 2; y > 0; y--)
        //    {
        //        //列内に0が含まれていた場合は除外
        //        if (fieldData[y].Contains(0))
        //            continue;

        //        //上記の条件に抵触しなければその列番号をリストに追加する
        //        removeLineNum.Add(y);
        //    }
        //    //リストを返却
        //    return removeLineNum;
        //}

        /// <summary>
        /// ラインの消去
        /// </summary>
        /// <returns>消去対象の列数</returns>
        //public List<int> RemoveLine()
        //{
        //    //消去対象の列がなければNullを返却
        //    if (PickUpRemoveLineNum().Count == 0)
        //        return null;

        //    //返却用のリストを生成
        //    List<int> removeLineNum = PickUpRemoveLineNum();

        //    //消去対象の列番号内の要素を全て０にする
        //    foreach (var lineNum in PickUpRemoveLineNum())
        //    {
        //        //列番号内の要素を一つずつ書き換え
        //        for (int row = 0; row < fieldData[lineNum].Length; row++)
        //        {
        //            //右端と左端は飛ばす
        //            if (row == 0 ||
        //                row == fieldData[lineNum].Length - 1)
        //                continue;

        //            //要素を０にする
        //            fieldData[lineNum][row] = 0;
        //        }
        //        //消去したライン数を加算
        //        //消去ライン計測者へ受け渡し
        //        mediator.AddRemoveLine();
        //        //スコアを加算する
        //        mediator.AddScore(100);
        //    }

        //    //詰める必要のある列数を格納したリストを返却
        //    //詰める処理にバトンタッチ
        //    return removeLineNum;
        //}

        /// <summary>
        /// 一段上の列をコピーして詰める
        /// </summary>
        /// <param name="removeLine">消去された列番号を格納したリスト</param>
        //public void FillLine(List<int> removeLine)
        //{
        //    //特に削除する列がなければ終了
        //    if (removeLine == null)
        //        return;

        //    //消去対象の列の個数だけ繰り返す
        //    for (int i = 0; i < removeLine.Count; i++)
        //    {
        //        //列番号が連続していない場合は飛ばす
        //        //ここがうまくいかない…
        //        if (i < removeLine.Count - 1
        //            && Math.Abs(removeLine[i] - removeLine[i + 1]) != 1)
        //        {
        //            continue;
        //        }
        //        //消去対象の列番号のうち最大の列番号からコピーを行う
        //        //(ただし下端・上端は除外)
        //        //ループ方向は下の列から上の列なのに注意
        //        for (int col = removeLine.Max(); col > 1; col--)
        //        {
        //            //一段上の列をコピーする
        //            fieldData[col - 1].CopyTo(fieldData[col], 0);
        //        }
        //    }
        //}

        /// <summary>
        /// デッドラインにブロックが残っているか判定
        /// </summary>
        /// <returns>ブロックが残っている→真
        /// 残っていない→偽</returns>
        //public bool DeadCheck()
        //{
        //    //fliedData内で、デッドラインの領域を調べる
        //    for (int col = 0; col < deadLine; col++)
        //    {
        //        //ただし、両端は飛ばす
        //        for (int row = 1; row < fieldData[col].Length - 1; row++)
        //        {
        //            //その配列内に0以外の要素があった時点で終了
        //            if (fieldData[col][row] != 0)
        //                return true;
        //        }
        //    }
        //    //普段は偽を返却
        //    return false;
        //}

        /// <summary>
        /// フィールドに出ているtetriminoオブジェクトが固定状態か？
        /// </summary>
        /// <returns></returns>
        public bool IsMinoLocked()
        {
            return minoStateManager.IsLocked;
        }

        /// <summary>
        /// プレイヤーが死亡したか？
        /// </summary>
        public bool IsDeadFlag
        {
            get;
            private set;
        }
    }
}
