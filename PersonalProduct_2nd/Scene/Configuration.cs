using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PersonalProduct_2nd.Device;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// 各種設定シーン
    /// </summary>
    class Configuration : IScene
    {
        private bool isEndFlag;
        private SoundManager sound;

        public Configuration()
        {
            isEndFlag = false;
            sound = DeviceManager.CreateInstance().GetSound();
        }

        public void Draw(Renderer renderer)
        {
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
            sound.PlayBGM("tutorial");
            if (Input.GetKeyTrigger(Keys.Enter))
                isEndFlag = true;

            if(Input.GetKeyTrigger(Keys.A))
            {
                sound.BGMSwitch = MediaState.Stopped;
            }
            else if (Input.GetKeyTrigger(Keys.B))
            {
                sound.BGMSwitch = MediaState.Paused;
            }
        }
    }
}
