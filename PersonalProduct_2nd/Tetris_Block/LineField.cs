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

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// Blockクラスに囲まれ、TetriMinoを積み重ねるプレイエリアクラス
    /// ここに各種制御・判定オブジェクトを集約する
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
        private MinoGenerator minoGenerator; //テトリミノ生産者

        private IGameMediator mediator; //ゲーム仲介者
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

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            minoGenerator = new MinoGenerator();
            //テトリミノの実体生成
            tetrimino = minoGenerator.PickHeaderMino();
            minoGenerator.GenerateEndOfNextMino();

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

            fieldData = csvReader.GetIntData(); //int[][]型で取得

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
            CanMoveLR(); //左右移動
            CanFallDown();　//下移動
            CanFallHard(); //超高速落下移動
            CanRotate(); //回転

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
                tetrimino = minoGenerator.PickHeaderMino();
                minoGenerator.GenerateEndOfNextMino();

                //テトリミノ制御対象を再設定
                minoStateManager.SetTarget(tetrimino);
                minoMove.SetTarget(tetrimino);
                minoCordinate.SetTarget(tetrimino);
            }
        }

        /// <summary>
        /// テトリミノが左右移動可能か検証
        /// </summary>
        /// <param name="cell">基準となるセルオブジェクト</param>
        public void CanMoveLR()
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
        public void CanFallDown()
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
        private void CanFallHard()
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
        public void CanRotate()
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
            tetrimino.DrawShadow(renderer, minoMove.CalcMinoShadowPos());

            arrayRenderer.RenderJugField(renderer, Color.White);
        }

        /// <summary>
        /// フィールドに出ているtetriminoオブジェクトが固定状態か？
        /// </summary>
        /// <returns></returns>
        public bool IsMinoLocked()
        {
            return minoStateManager.IsLocked;
        }

        /// <summary>
        /// フィールドの配列データの返却
        /// </summary>
        /// <returns>フィールドのfieldData</returns>
        public int[][] GetFieldArray()
        {
            return fieldData;
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
