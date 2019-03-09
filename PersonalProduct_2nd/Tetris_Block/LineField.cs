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
        #endregion フィールド

        /// <summary>
        ///　コンストラクタ
        /// </summary>
        /// <param name="device"></param>
        public LineField(DeviceManager device)
        {
            mapList = new List<List<Cell>>();//マップの実態生成
            deviceManager = device;

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
            arrayRenderer = new ArrayRenderer(new Vector2(Size.WIDTH * 2, Size.HEIGHT * 1));
            //最初はテトリミノは移動可能として初期化
            minoStateManager.CanMove = true;
        }

        /// <summary>
        /// CSVファイルに合わせてブロックの追加
        /// (フィールドmapListの要素内の文字列型配列をここで更に分析)
        /// </summary>
        /// <param name="lineCnt"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<Cell> addBlock(int lineCnt, string[] line)
        //privateメソッドより先頭を小文字とする
        {
            //コピー元オブジェクト登録用ディクショナリ
            Dictionary<string, Cell> cellDict = new Dictionary<string, Cell>();
            //スペースは0
            cellDict.Add("0", new Space());
            //ブロックは1
            cellDict.Add("1", new Block());

            //作業用リスト
            List<Cell> workList = new List<Cell>();

            int colCnt = 0; //列カウント用
            foreach (var s in line)
            {
                try
                {
                    //ディクショナリから元データを取り出し、クローン機能で複製
                    Cell work = (Cell)cellDict[s].Clone();
                    //1列の要素を１ブロックずつ配置する
                    work.Position = new Vector2(colCnt * Size.WIDTH, lineCnt * Size.HEIGHT);
                    workList.Add(work);
                }
                catch (Exception e)　//例外処理
                {
                    Console.WriteLine(e);
                }
                //列カウンタを増やす。次の列へ移動
                colCnt += 1;
            }
            return workList;
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

            //テトリミノが動ける状態か判定
            MoveLRCheck(); //左右移動
            MoveDCheck();　//下移動
            RotateCheck(); //回転

            foreach (var list in mapList)//listはList<Cell>型
            {
                foreach (var cell in list) //cellはCell型
                {
                    //objがSpaceクラスのオブジェクトなら次へ
                    if (cell is Space)
                        continue;

                    cell.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// 衝突判定・衝突後処理
        /// </summary>
        /// <param name="cell">基準となるセルオブジェクト</param>
        public void Hit(Cell cell)
        {
            Point work = cell.GetHitArea().Location;//左上の座標を取得
            //配列の何行何列目にいるか計算
            int x = work.X / Size.WIDTH;
            int y = work.Y / Size.HEIGHT;

            //移動で食い込んでいる時の修正
            if (x < 1)
                x = 1;
            if (y < 1)
                y = 1;

            Range yRange = new Range(0, mapList.Count() - 1); //行の範囲(配列番号に対応)
            Range xRange = new Range(0, mapList[0].Count() - 1); //列の範囲(配列番号に対応)

            //引数cellの周りの8つのセルに衝突対象がないかどうか確認(テトリミノのブロックごとに改良する必要ありか？)
            for (int row = y - 1; row <= (y + 1); row++)　//自分の上と下のセル
            {
                for (int col = x - 1; col <= (x + 1); col++)　//自分の右と左のセル
                {
                    //配列外なら何もしない
                    //
                    if (xRange.IsOutOfRange(col) || yRange.IsOutOfRange(row))
                        continue;

                    //その場所のオブジェクトを取得(調べる相手)
                    Cell cll = mapList[row][col];

                    //Spaceクラスのオブジェクトなら次へ
                    if (cll is Space)
                        continue;

                    if (cll is Block)
                        cell.Hit(cll); //引数のCellオブジェクトはその隣り合うCellオブジェクトに衝突した
                }
            }
        }
        /// <summary>
        /// テトリミノとの衝突判定・衝突後処理
        /// </summary>
        /// <param name="cell">基準となるセルオブジェクト</param>
        public void Hit(Tetrimino tetrimino)
        {
            //当たり判定を格納した配列を受け取る
            Rectangle[] checkArray = tetrimino.GetHitArea();

            //テトリミノの当たり判定を一つずつ確認
            for (int i = 0; i < checkArray.Length; i++)
            {
                //左上の座標を取得
                Point work = checkArray[i].Location;

                //配列の何行何列目にいるか計算
                int x = work.X / Size.WIDTH;
                int y = work.Y / Size.HEIGHT;

                //移動で食い込んでいる時の修正
                if (x < 1)
                    x = 1;
                if (y < 1)
                    y = 1;

                Range yRange = new Range(0, mapList.Count() - 1); //行の範囲(配列番号に対応)
                Range xRange = new Range(0, mapList[0].Count() - 1); //列の範囲(配列番号に対応)

                //引数cellの周りの8つのセルに衝突対象がないかどうか確認(テトリミノのブロックごとに改良する必要ありか？)
                for (int row = y - 1; row <= (y + 1); row++) //自分の上と下のセル
                {
                    for (int col = x - 1; col <= (x + 1); col++) //自分の右と左のセル
                    {
                        //配列外なら何もしない
                        if (xRange.IsOutOfRange(col) || yRange.IsOutOfRange(row))
                            continue;

                        //その場所のオブジェクトを取得(調べる相手)
                        Cell cll = mapList[row][col];

                        //Spaceクラスのオブジェクトなら次へ
                        if (cll is Space)
                            continue;

                        if (cll is Block)
                            tetrimino.Hit(cll); //引数のTetriminoオブジェクトはその隣り合うCellオブジェクトに衝突した
                    }
                }
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
            if (Input.GetKeyTrigger(Keys.Down) || minoStateManager.IsFall())
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
                    //テトリミノを落下移動させる
                    minoMove.LetMinoFall();

                    //テトリミノの落下状態を初期化
                    minoStateManager.ResetFallTimer();
                    //tetrimino.InitFall();

                    //tetrimino.MoveDown();
                }
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
            if (Input.GetKeyTrigger(Keys.A)
                || Input.GetKeyTrigger(Keys.D))
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

            //フィールドを描画
            arrayRenderer.RenderJugField(renderer);

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

            //フィールドのデータの要素を一つずつ確認する
            for (int y = 0; y < fieldData.GetLength(0); y++)
            {
                //下端は除外
                if (y == fieldData.GetLength(0) - 1)
                    continue;

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
        public int RemoveLine()
        {
            //詰める列数を格納する変数を用意
            int fillLine = 0;

            //消去対象の列がなければ終了
            if (PickUpRemoveLineNum().Count == 0)
                return fillLine;

            //消去対象の列番号内の要素を全て０にする
            foreach (var lineNum in PickUpRemoveLineNum())
            { 
                //列番号内の要素を一つずつ書き換え
                for (int row = 0; row < fieldData[lineNum].Length; row++)
                {
                    //右端と左端は飛ばす
                    if (row == 0
                        || row == fieldData[lineNum].Length - 1)
                        continue;

                    //要素を０にする
                    fieldData[lineNum][row] = 0;
                }
                //詰める列数に加算
                fillLine++;
            }

            //詰める必要のある列数を返却、詰める処理にバトンタッチ
            return fillLine;
        }

        /// <summary>
        /// 一段上の列をコピーして詰める
        /// </summary>
        /// <param name="removeLineCnt">消去された列数</param>
        public void FillLine(int removeLineCnt)
        {
            //消去対象の列の個数だけ繰り返す
            for (int i = 0; i < removeLineCnt; i++)
            {
                //全ての列でコピーを行う(ただし下端・上端は除外)
                for (int col = 1; col < fieldData.GetLength(0) - 1; col++)
                {
                    //一段上の列をコピーする
                    fieldData[col - 1].CopyTo(fieldData[col], 0);
                }
            }
        }
    }
}
