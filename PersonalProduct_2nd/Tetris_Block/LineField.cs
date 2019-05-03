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
    class LineField
    {
        #region フィールド
        //ListのListで縦横の２次元配列的構造
        private List<List<Cell>> mapList;
        private DeviceManager deviceManager; //ゲームデバイス
        private Tetrimino tetrimino; //フィールドに出現しているテトリミノ
        private ArrayRenderer arrayRenderer; //二次元配列描画オブジェクト
        private int[][] fieldData; //プレイ画面内のフィールドデータ
        private MinoMove minoMove; //テトリミノ移動オブジェクト
        private MinoStateManager minoStateManager; //テトリミノの状態管理オブジェクト
        private IGameMediator mediator; //ゲーム仲介者

        private readonly int deadLine = 3; //ゲームオーバーか確認用の列番号
        private int removeCnt; //消去したラインの数
        #endregion フィールド

        /// <summary>
        ///　コンストラクタ
        /// </summary>
        /// <param name="device"></param>
        /// <param name="mediator"></param>
        public LineField(DeviceManager device, IGameMediator mediator)
        {
            mapList = new List<List<Cell>>();//マップの実態生成
            deviceManager = device;
            this.mediator = mediator;

            Initialize();
        }

        public void Initialize()
        {
            //テトリミノの実体生成
            tetrimino = new Tetrimino();
            //テトリミノ移動オブジェクトを生成、移動ターゲットを設定
            minoMove = new MinoMove(tetrimino);
            //テトリミノの状態管理オブジェクトを生成、管理ターゲットを設定
            minoStateManager = new MinoStateManager(tetrimino);

            //二次元配列描画オブジェクトを実体生成
            arrayRenderer = new ArrayRenderer(Size.offset);
            //最初はテトリミノは移動可能として初期化
            minoStateManager.CanMove = true;
            //最初は死亡フラグはOFFにする
            IsDeadFlag = false;
            //消去した列の数は０で初期化
            removeCnt = 0;
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

            #region ラインをブロックに変換せずに生成する方法に変更
            //1行ごとにmapListに追加していく
            //for (int lineCnt = 0; lineCnt < data.Count(); lineCnt++)
            //{
            //    //ここで更にListの要素の配列をひとつずつ処理する
            //    mapList.Add(addBlock(lineCnt, data[lineCnt]));
            //}
            #endregion ラインをブロックに変換せずに生成する方法に変更
        }

        /// <summary>
        /// マップリストのクリア
        /// </summary>
        public void Unload()
        {
            mapList.Clear();
        }

        /// <summary>
        /// フィールド内の空白要素以外を更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //テトリミノを更新
            tetrimino.Update(gameTime);

            RefToField(); //テトリミノが凍結後にフィールドに反映
            #region テトリミノの変数を一つにしたため削除
            //一つ目のテトリミノが固定され、二つ目のテトリミノがまだ生成されてなければ
            //この方法だと変数が無限に必要になるため、生成者を用意する
            //if(tetrimino.IsLocked() && tetrimino2 == null)
            //{
            //    tetrimino2 = new Tetrimino();　//新たに生成
            //}
            //if (tetrimino2 != null) //二つ目のテトリミノが生成されたら
            //{
            //    //テトリミノの状態管理・移動制御の対象をこっちに設定しなおす
            //    minoStateManager.SetTarget(tetrimino2);
            //    minoStateManager.CanMove = true;
            //    minoMove.SetTarget(tetrimino2);

            //    //テトリミノを更新する
            //    tetrimino2.Update(gameTime);
            //}
            #endregion テトリミノの変数を一つにしたため削除
            //ゲームオーバー領域にブロックが残っているか検証
            DeadCheck();

            //テトリミノが動ける状態か判定
            MoveLRCheck(); //左右移動
            MoveDCheck();　//下移動
            HardFallCheck(); //超高速落下移動
            RotateCheck(); //回転

            //消去した列の数に応じて落下速度を再設定
            //→メソッド化予定
            if (removeCnt % 5 == 0 && removeCnt != 0)
            {
                tetrimino.ResetFallTimer(0.05f);
            }
        }

        /// <summary>
        /// テトリミノとの衝突判定・衝突後処理
        /// →キー入力時に判定を行う方法に変更
        /// </summary>
        /// <param name="cell">基準となるセルオブジェクト</param>
        //public void Hit(Tetrimino tetrimino)
        //{
        //    //当たり判定を格納した配列を受け取る
        //    Rectangle[] checkArray = tetrimino.GetHitArea();

        //    //テトリミノの当たり判定を一つずつ確認
        //    for (int i = 0; i < checkArray.Length; i++)
        //    {
        //        //左上の座標を取得
        //        Point work = checkArray[i].Location;

        //        //配列の何行何列目にいるか計算
        //        int x = work.X / Size.WIDTH;
        //        int y = work.Y / Size.HEIGHT;

        //        //移動で食い込んでいる時の修正
        //        if (x < 1)
        //            x = 1;
        //        if (y < 1)
        //            y = 1;

        //        Range yRange = new Range(0, mapList.Count() - 1); //行の範囲(配列番号に対応)
        //        Range xRange = new Range(0, mapList[0].Count() - 1); //列の範囲(配列番号に対応)

        //        //引数cellの周りの8つのセルに衝突対象がないかどうか確認(テトリミノのブロックごとに改良する必要ありか？)
        //        for (int row = y - 1; row <= (y + 1); row++) //自分の上と下のセル
        //        {
        //            for (int col = x - 1; col <= (x + 1); col++) //自分の右と左のセル
        //            {
        //                //配列外なら何もしない
        //                if (xRange.IsOutOfRange(col) || yRange.IsOutOfRange(row))
        //                    continue;

        //                //その場所のオブジェクトを取得(調べる相手)
        //                Cell cll = mapList[row][col];

        //                //Spaceクラスのオブジェクトなら次へ
        //                if (cll is Space)
        //                    continue;

        //                if (cll is Block)
        //                    tetrimino.Hit(cll); //引数のTetriminoオブジェクトはその隣り合うCellオブジェクトに衝突した
        //            }
        //        }
        //    }
        //}

        ///<summary>
        ///固定状態のテトリミノに対応するフィールドの座標データへ反映
        ///</summary>
        public void RefToField()
        {
            if (tetrimino.IsLocked())
            {
                //テトリミノの構成ブロックの座標を取得(配列？)
                foreach (var point in tetrimino.GetMinoUnitPos())
                {
                    //構成ブロックに対応するフィールドの配列位置を取得
                    int unitPos_X = (int)(tetrimino.Position.X + point.X) / Size.WIDTH;
                    int unitPos_Y = (int)(tetrimino.Position.Y + point.Y) / Size.HEIGHT;

                    //対応する位置のフィールドの要素を書き換える
                    fieldData[unitPos_Y][unitPos_X] = tetrimino.GetUnitNum();
                }
                //横一列が揃ったか検証、揃ったらラインを消去
                RemoveAndFillLine();
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
            //テトリミノが右に移動しようとしている
            if (Input.GetKeyTrigger(Keys.Right))
            {
                //移動可能にする
                minoStateManager.CanMove = true;
                //canMove = true;

                //出現中のテトリミノの構成ブロックの回転中心からの相対座標を取得
                //それを一つずつ取り出し、フィールド内での位置を取得する
                foreach (var point in tetrimino.GetMinoUnitPos())
                {
                    //構成ブロックに対応するフィールドの配列位置を取得
                    int unitPos_X = (int)(tetrimino.Position.X + point.X) / Size.WIDTH;
                    int unitPos_Y = (int)(tetrimino.Position.Y + point.Y) / Size.HEIGHT;

                    //テトリミノ本体の座標と相対座標の和に右側隣接するマップ要素がSpace以外なら
                    if (fieldData[unitPos_Y][unitPos_X + 1] != 0)
                    {
                        //移動不能に通知
                        minoStateManager.CanMove = false;
                        //canMove = false; 
                    }
                }
                //全て取り出し切って、一つも条件に抵触しなければ
                if (minoStateManager.CanMove)
                    minoMove.LetMinoMoveR(); //テトリミノを右移動させる
                                             //tetrimino.MoveR(); //移動する
            }

            //テトリミノが左に移動しようとしている
            if (Input.GetKeyTrigger(Keys.Left))
            {
                //移動可能にする
                minoStateManager.CanMove = true;
                //canMove = true;

                //出現中のテトリミノの構成ブロックの回転中心からの相対座標を取得
                //それを一つずつ取り出し、フィールド内での位置を取得する
                foreach (var point in tetrimino.GetMinoUnitPos())
                {
                    //構成ブロックに対応するフィールドの配列位置を取得
                    int unitPos_X = (int)(tetrimino.Position.X + point.X) / Size.WIDTH;
                    int unitPos_Y = (int)(tetrimino.Position.Y + point.Y) / Size.HEIGHT;

                    //テトリミノ本体の座標と相対座標の和に左側隣接するマップ要素が０以外なら
                    if (fieldData[unitPos_Y][unitPos_X - 1] != 0)
                    {
                        //移動不能に通知
                        minoStateManager.CanMove = false;
                        //canMove = false; 
                    }
                }
                //全て取り出し切って、一つも条件に抵触しなければ
                if (minoStateManager.CanMove)
                    minoMove.LetMinoMoveL(); //テトリミノを左移動させる
                                             //tetrimino.MoveL(); //移動する
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
                //まずは移動可能とする
                minoStateManager.CanMove = true;
                //canMove = true;

                //出現中のテトリミノの構成ブロックの回転中心からの相対座標を取得
                //それを一つずつ取り出し、フィールド内での位置を取得する
                foreach (var point in tetrimino.GetMinoUnitPos())
                {
                    //構成ブロックに対応した配列位置を取得
                    int unitPos_X = (int)(tetrimino.Position.X + point.X) / Size.WIDTH;
                    int unitPos_Y = (int)(tetrimino.Position.Y + point.Y) / Size.WIDTH;

                    //構成ブロックの1ブロック分下部のブロックが0（＝空白）以外なら
                    if (fieldData[unitPos_Y + 1][unitPos_X] != 0)
                    {
                        //移動不可能にする
                        minoStateManager.CanMove = false;
                        //canMove = false;
                        //テトリミノを着地状態とする
                        minoStateManager.SetLandState(true);
                    }
                }

                //構成ブロックを全て調べて、条件に抵触しなければ移動する
                if (minoStateManager.CanMove)
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
                minoStateManager.CanMove = true;
                //ミノ構成ブロックと着地点までの距離リストを用意
                var toLandVal = new List<int>();
                //テトリミノの構成ブロックを一つずつ取り出し
                foreach (var point in tetrimino.GetMinoUnitPos())
                {
                    //構成ブロックの座標に対応する要素番号の特定
                    int unitPos_X = (int)(tetrimino.Position.X + point.X) / Size.WIDTH;
                    int unitPos_Y = (int)(tetrimino.Position.Y + point.Y) / Size.HEIGHT;
                    //落下中のミノの下部のフィールド位置の要素を検証
                    for (int i = 1; i < fieldData.GetLength(0) - unitPos_Y; i++)
                    {
                        //0以外の要素が出てきた時点で距離を記録
                        if (fieldData[unitPos_Y + i][unitPos_X] != 0)
                        {
                            toLandVal.Add(unitPos_Y + i);
                        }
                    }
                }
                //最も近距離の値の一つ上を着地予定地Yと定める
                var landPosY = (toLandVal.Min() - 1) * Size.HEIGHT;
                //着地予定地まで一気に落下させる
                minoMove.HardFall(new Vector2(tetrimino.Position.X, landPosY));
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
            //なんか見づらいコードやなぁ・・・
            if (Input.GetKeyTrigger(Keys.A) || Input.GetKeyTrigger(Keys.D))
            {
                //まずは回転可能にする
                minoStateManager.CanMove = true;
                //回転配列をカラの状態で生成
                int[,] willRotateArray = null;

                //Aキー入力→テトリミノが時計回りに90度回転した際の配列を取得
                if (Input.GetKeyTrigger(Keys.A))
                    willRotateArray = tetrimino.GetClockwise_RotatedArray();
                //Dキー入力→テトリミノが反時計回りに90回転した際の配列
                if (Input.GetKeyTrigger(Keys.D))
                    willRotateArray = tetrimino.GetAntiClockwise_RotatedArray();

                //テトリミノの構成ブロックの座標を取得、一つずつ調べる
                foreach (var point in tetrimino.GetRotatedUnitPos(willRotateArray))
                {
                    //構成ブロックに対応した配列位置を取得
                    int unitPos_X = (int)(tetrimino.Position.X + point.X) / Size.WIDTH;
                    int unitPos_Y = (int)(tetrimino.Position.Y + point.Y) / Size.WIDTH;

                    //対応するフィールドの位置がブロックならば
                    //配列指定位置が負の数の場合も
                    if (unitPos_X <= 0
                        || unitPos_Y <= 0
                        || fieldData[unitPos_Y][unitPos_X] != 0)
                    {
                        //回転不可能とする
                        minoStateManager.CanMove = false;
                        //ループ終了
                        break;
                    }
                }
                //条件に抵触しなければ入力キーに応じて回転処理
                if (minoStateManager.CanMove)
                {
                    if (Input.GetKeyTrigger(Keys.A))
                        //テトリミノを回転する(後でMinoMoveに行わせる)
                        minoMove.LetMinoRotate_Clockwise();
                    //tetrimino.Rotate_Clockwise();
                    else if (Input.GetKeyTrigger(Keys.D))
                        //テトリミノを反時計回りに回転
                        minoMove.LetMinoRotate_AntiClockwise();
                    //tetrimino.Rotate_AntiClockwise();
                }
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
            if (DeadCheck())
            {
                //積まれたブロックを赤く描画
                //→Endingシーンなどで色を変える予定
                arrayRenderer.RenderJugField(renderer, Color.Red);
                //死亡フラグを立てる→ここでフラグをいじってはダメ
                IsDeadFlag = true;
            }
            else
                //普段は標準描画
                arrayRenderer.RenderJugField(renderer, Color.White);

            #region int型のままフィールドを生成する方法に変更
            //すべてのオブジェクト(Block, Space)を要素一つずつ描画していく
            //maplistは二重配列的構造よりループは2重となる
            //foreach (var line in mapList) //line is List<Cell>型
            //{
            //    foreach (var cell in line) //cell is Cell型{
            //    {
            //        cell.Draw(renderer);
            //    }
            //}
            #endregion int型のままフィールドを生成する方法に変更
        }

        /// <summary>
        /// 落下中のミノの影を落とす位置の計算
        /// </summary>
        /// <returns></returns>
        private Vector2 CalcMinoShadowPos()
        {
            //ミノ構成ブロックと着地点までの距離リストを用意
            var toLandVal = new List<int>();
            //テトリミノの構成ブロックを一つずつ取り出し
            foreach (var point in tetrimino.GetMinoUnitPos())
            {
                //構成ブロックの座標に対応する要素番号の特定
                int unitPos_X = (int)(tetrimino.Position.X + point.X) / Size.WIDTH;
                int unitPos_Y = (int)(tetrimino.Position.Y + point.Y) / Size.HEIGHT;
                //落下中のミノの下部のフィールド位置の要素を検証
                for (int i = 1; i < fieldData.GetLength(0) - unitPos_Y; i++)
                {
                    //0以外の要素が出てきた時点で距離を記録
                    if (fieldData[unitPos_Y + i][unitPos_X] != 0)
                    {
                        toLandVal.Add(unitPos_Y + i);
                    }
                }
            }
            //最も近距離の値の一つ上を着地予定地Yと定める
            var shadowPosY = (toLandVal.Min() - 1) * Size.HEIGHT;
            //影を投射する座標を返却
            return new Vector2(tetrimino.Position.X, shadowPosY);
        }

        /// <summary>
        /// ラインを消去し、詰める
        /// </summary>
        public void RemoveAndFillLine()
        {
            //消去されたラインを詰める
            FillLine(RemoveLine());
        }

        /// <summary>
        /// ブロックが一列揃った列番号を取得
        /// </summary>
        /// <returns>一列揃った列番号を格納したリスト</returns>
        public List<int> PickUpRemoveLineNum()
        {
            //返却用のリストを生成
            List<int> removeLineNum = new List<int>();

            //フィールドのデータの要素を下段から一つずつ確認する
            //(ただし下端は除外)
            for (int y = fieldData.GetLength(0) - 2; y > 0; y--)
            {
                //列内に0が含まれていた場合は除外
                if (fieldData[y].Contains(0))
                    continue;

                //上記の条件に抵触しなければその列番号をリストに追加する
                removeLineNum.Add(y);
            }
            //リストを返却
            return removeLineNum;
        }

        /// <summary>
        /// ラインの消去
        /// </summary>
        /// <returns>消去対象の列数</returns>
        public List<int> RemoveLine()
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
                for (int row = 0; row < fieldData[lineNum].Length; row++)
                {
                    //右端と左端は飛ばす
                    if (row == 0 ||
                        row == fieldData[lineNum].Length - 1)
                        continue;

                    //要素を０にする
                    fieldData[lineNum][row] = 0;
                }
                //消去したライン数を加算
                removeCnt++;
                //消去ライン計測者へ受け渡し
                mediator.AddRemoveLine();
                //スコアを加算する
                mediator.AddScore(100);
            }

            //詰める必要のある列数を格納したリストを返却
            //詰める処理にバトンタッチ
            return removeLineNum;
        }

        /// <summary>
        /// 一段上の列をコピーして詰める
        /// </summary>
        /// <param name="removeLine">消去された列番号を格納したリスト</param>
        public void FillLine(List<int> removeLine)
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
                    fieldData[col - 1].CopyTo(fieldData[col], 0);
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
            for (int col = 0; col < deadLine; col++)
            {
                //ただし、両端は飛ばす
                for (int row = 1; row < fieldData[col].Length - 1; row++)
                {
                    //その配列内に0以外の要素があった時点で終了
                    if (fieldData[col][row] != 0)
                        return true;
                }
            }
            //普段は偽を返却
            return false;
        }

        /// <summary>
        /// プレイヤーが死亡したか？
        /// </summary>
        public bool IsDeadFlag
        {
            get;
            private set;
        }

        /// <summary>
        /// 消去したライン数の取得
        /// </summary>
        /// <returns>フィールドremoveCnt</returns>
        public int GetRemoveCnt()
        {
            return removeCnt;
        }
    }
}
