// このファイルで必要なライブラリのnamespaceを指定
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Device;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// クリアシーン
    /// </summary>
    class Clear : IScene
    {
        private bool isEndFlag;
        private IScene backGround;

        private SoundManager sound;
        private DeviceManager device;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pastScene">直前のシーン</param>
        public Clear(IScene pastScene)
        {
            isEndFlag = false;
            backGround = pastScene;

            device = DeviceManager.CreateInstance();
            sound = device.GetSound();
        }

        public void Draw(Renderer renderer)
        {
            backGround.Draw(renderer);

            renderer.DrawTexture("clear", new Vector2(500, 1000));
        }

        public void Initialize()
        {
            isEndFlag = false;
        }

        public bool IsEnd()
        {
            return isEndFlag;
        }

        public EScene Next()
        {
            return EScene.Title;
        }

        public void Shutdown()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (Input.GetKeyTrigger(Keys.Enter))
                isEndFlag = true;
        }
    }
}