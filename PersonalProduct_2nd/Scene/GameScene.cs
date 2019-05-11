using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Tetris_Block;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PersonalProduct_2nd.Tetris_Block.Tetrimino;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// ゲームプレイ関連シーン
    /// 作成者:谷永吾
    /// </summary>
    class GameScene : IScene, IGameMediator
    {
        public const int CLEAR_LEVEL = 11;

        private bool isEndFlag;//終了フラグ
        private LineField field; //プレイエリアのフィールド
        private Score score; //スコア
        private RemoveLineBoard removeLineBoard; //消去ライン数表示ボード
        private LevelBoard levelBoard; //レベルボード

        private DeviceManager device;//デバイス管理者
        private SoundManager sound; //サウンド管理者  

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameScene()
        {
            device = DeviceManager.CreateInstance();
            sound = device.GetSound();

            isEndFlag = false;
            //tetriminoFactory = new TetrminoFactory(this); //LineFieldオブジェクトに委託
        }

        /// <summary>
        /// 消去したラインの加算をラインボードに通達する
        /// </summary>
        public void AddRemoveLine()
        {
            removeLineBoard.AddRemoveLine();
        }

        /// <summary>
        /// スコアの加算
        /// </summary>
        /// <param name="num">加算得点</param>
        public void AddScore(int num)
        {
            score.Add(num);
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        public void Draw(Renderer renderer)
        {
            field.Draw(renderer);
            score.Draw(renderer);
            removeLineBoard.Draw(renderer);
            levelBoard.Draw(renderer);
        }

        /// <summary>
        /// 現在レベルの取得
        /// </summary>
        /// <returns></returns>
        public int GetLevel()
        {
            return levelBoard.Level;
        }

        /// <summary>
        /// スコアの取得
        /// </summary>
        /// <returns>現在スコア</returns>
        public int GetScore()
        {
            return score.GetScore();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEndFlag = false;

            field = new LineField(device, this);
            field.Load("LineField.csv", "./csv/"); //フィールド元のファイルの読み込み
            field.Initialize();

            score = new Score();
            removeLineBoard = new RemoveLineBoard();
            levelBoard = new LevelBoard();
        }

        /// <summary>
        /// レベルアップ状態か審査
        /// </summary>
        private void LevelJudge()
        {
            //開始直後は飛ばす
            if (removeLineBoard.GetRemoveLineValue() == 0)
                return;

            var remoLine = removeLineBoard.GetRemoveLineValue();
            //10列消去ごとにレベルアップ！
            if (remoLine - levelBoard.Level * 10 >= 0 &&
                levelBoard.JudgeMultiple(remoLine / 10 + 1))
            {
                levelBoard.LevelUP();
            }
        }

        /// <summary>
        /// 終了か？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return isEndFlag;
        }

        /// <summary>
        /// テトリスのレベルアップ
        /// </summary>
        public void LevelUP()
        {
            levelBoard.LevelUP();
        }

        /// <summary>
        /// 次のシーンへ
        /// </summary>
        /// <returns>シーンオブジェクトに対応する列挙型</returns>
        public EScene Next()
        {
            if (levelBoard.Level == CLEAR_LEVEL)
                return EScene.Clear;
            return EScene.GameOver;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
            score.Shutdown();
            removeLineBoard.Shutdown();
            sound.PauseBGM();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update(GameTime gameTime)
        {
            //デッドラインにブロックが達するか
            //クリアレベルに到達したら終了
            if (field.IsDeadFlag || levelBoard.Level == CLEAR_LEVEL)
                isEndFlag = true;

            //BGM
            sound.PlayBGM("gameplay", 0.4f);

            //レベルアップ処理
            LevelJudge();

            field.Update(gameTime);
            score.Update(gameTime);
            removeLineBoard.Update(gameTime);
        }
    }
}
