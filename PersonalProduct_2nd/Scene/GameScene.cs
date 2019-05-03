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
        private bool isEndFlag;//終了フラグ
        private LineField field; //プレイエリアのフィールド
        private Score score; //スコア
        private RemoveLineBoard removeLineBoard; //消去ライン数表示ボード

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
        }

        /// <summary>
        /// 消去したライン数の取得
        /// </summary>
        /// <returns></returns>
        public int GetRemoveLineValue()
        {
            return field.GetRemoveCnt();
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
            score = new Score();
            removeLineBoard = new RemoveLineBoard();

            //tetrimino = new Tetrimino();
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
        /// 次のシーンへ
        /// </summary>
        /// <returns>シーンオブジェクトに対応する列挙型</returns>
        public EScene Next()
        {
            //return EScene.Result;
            return EScene.Title;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
            score.Shutdown();
            removeLineBoard.Shutdown();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update(GameTime gameTime)
        {
            //デッドラインにブロックが達したら終了
            if (field.IsDeadFlag)
                isEndFlag = true;

            //tetrimino.Update(gameTime);
            //field.Hit(tetrimino); //表示したテトリミノが接触してないか確認

            field.Update(gameTime);
            score.Update(gameTime);
            removeLineBoard.Update(gameTime);
        }
    }
}
