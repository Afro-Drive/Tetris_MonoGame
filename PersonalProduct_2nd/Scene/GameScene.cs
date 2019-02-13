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
        /// 描画処理
        /// </summary>
        public void Draw(Renderer renderer)
        {
            field.Draw(renderer);
            //tetrimino.Draw(renderer);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEndFlag = false;
            field = new LineField(device);
            field.Load("LineField.csv", "./csv/"); //フィールド元のファイルの読み込み

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
            return EScene.LoadScene;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update(GameTime gameTime)
        {
            //if (Input.IskeyDown(Keys.Enter))
            //    isEndFlag = true;

            //tetrimino.Update(gameTime);
            //field.Hit(tetrimino); //表示したテトリミノが接触してないか確認

            field.Update(gameTime);
        }
    }
}
