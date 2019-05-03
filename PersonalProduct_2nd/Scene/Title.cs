using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Device;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// タイトルシーン
    /// 作成者:谷永吾
    /// 作成開始日:2018年10月18日
    /// </summary>
    class Title : IScene
    {
        private bool isEndFlag; //終了フラグ

        private DeviceManager device;//デバイス管理者
        private SoundManager sound; //サウンド管理者      

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Title()
        {
            isEndFlag = false;

            device = DeviceManager.CreateInstance();
            sound = device.GetSound();
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("title", Vector2.Zero);            
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
        /// <returns>TitleクラスのフィールドisEndFlag</returns>
        public bool IsEnd()
        {
            return isEndFlag;
        }

        /// <summary>
        /// 次のシーンへ
        /// </summary>
        /// <returns>Tutorialに対応する列挙型</returns>
        public EScene Next()
        {
            //return EScene.Tutorial;
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
