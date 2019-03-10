using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// プレイ後のリザルトシーン
    /// 作成者:谷永吾
    /// </summary>
    class Result : IScene
    {
        private bool isEndFlag;//終了フラグ
        private IScene backGround; //背景に移すシーン

        private DeviceManager device;//デバイス管理者
        private SoundManager sound; //サウンド管理者      

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pastScene">1つ前のシーン</param>
        public Result(IScene pastScene)
        {
            isEndFlag = false;
            this.backGround = pastScene;

            device = DeviceManager.CreateInstance();
            sound = device.GetSound();
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //背景シーンを描画
            backGround.Draw(renderer);
            renderer.DrawTexture("christmas_dance_tonakai", new Vector2(700, 190));
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
        /// <returns>ResultクラスのフィールドisEndFlag</returns>
        public bool IsEnd()
        {
            return isEndFlag;
        }

        /// <summary>
        /// 次のシーンへ
        /// </summary>
        /// <returns>Titleシーンに対応する列挙型</returns>
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
                isEndFlag = true;
        }
    }
}
