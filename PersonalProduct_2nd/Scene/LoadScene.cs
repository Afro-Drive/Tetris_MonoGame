using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Device;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// ロードシーンクラス
    /// 作成者:谷永吾
    /// 作成開始日:2018年10月18日
    /// </summary>
    class LoadScene : IScene
    {
        #region フィールド
        private bool isEndFlag;//終了フラグ

        //各種デバイス
        private DeviceManager device;//デバイス管理者
        private SoundManager sound; //サウンド管理者   
        private Renderer renderer; //レンダー管理者
        private GraphicsDevice graphicsDevice; //グラフィックスデバイスオブジェクト

        //各種Loader
        private Texture_Loader textureLoader;
        private Texture_Loader pixcelLoader;
        private BGM_Loader bgmLoader;
        private SE_Loader seLoader;
        #endregion フィールド

        private string[,] TextureMatrix()
        {
            string[,] textures = new string[,]
            {
            };

            return textures;
        }

        private string[,] BGMMatrix()
        {
            string[,] bgms = new string[,]
            {

            };

            return bgms;
        }


        private string[,] SEMatrix()
        {
            string[,] seMatrix = new string[,]
            {

            };

            return seMatrix;
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Graphics">グラフィック管理者(Game1の所有フィールドを利用)</param>
        public LoadScene(GraphicsDevice Graphics)
        {
            isEndFlag = false;

            device = DeviceManager.CreateInstance();
            sound = device.GetSound();
            renderer = device.GetRenderer();
            graphicsDevice = Graphics;
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("christmas_dance_tonakai", Vector2.Zero);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEndFlag = false;
        }

        /// <summary>
        /// 終了か？
        /// </summary>
        /// <returns>LoadSceneのフィールドisEndFlag</returns>
        public bool IsEnd()
        {
            return isEndFlag;
        }

        /// <summary>
        /// 次のシーンへ
        /// </summary>
        /// <returns>ロゴシーン</returns>
        public EScene Next()
        {
            return EScene.GameScene;
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
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (Input.IskeyDown(Keys.Enter))
            {
                isEndFlag = true;
            }
        }
    }
}
