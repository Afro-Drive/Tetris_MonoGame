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
    enum SelectMenu
    {
        Play,
        Tutorial,
    }

    /// <summary>
    /// タイトルシーン
    /// 作成者:谷永吾
    /// 作成開始日:2018年10月18日
    /// </summary>
    class Title : IScene
    {
        private bool isEndFlag; //終了フラグ
        private Dictionary<SelectMenu, Vector2> selectButtons;//セレクト用のディクショナリ
        private Dictionary<SelectMenu, Rectangle> acceptAreas;//マウス受付範囲
        private Dictionary<SelectMenu, bool> imageState;
        private Dictionary<SelectMenu, bool> prevImageState;

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
            ActiveDraw(renderer, "playbutton", selectButtons[SelectMenu.Play], SelectMenu.Play);
            ActiveDraw(renderer, "tutorialbutton", selectButtons[SelectMenu.Tutorial], SelectMenu.Tutorial);
        }

        private void ActiveDraw(Renderer renderer, string asset, Vector2 position, SelectMenu menu)
        {
            if (imageState[menu])
            {
                renderer.DrawTexture(asset, position);
            }
            else
            {
                renderer.DrawTexture(asset, position, 0.7f);
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEndFlag = false;

            selectButtons = new Dictionary<SelectMenu, Vector2>()
            {
                { SelectMenu.Tutorial, new Vector2(650, 800) },
                { SelectMenu.Play, new Vector2(650, 880) },
            };
            acceptAreas = new Dictionary<SelectMenu, Rectangle>()
            {
                { SelectMenu.Tutorial, new Rectangle(new Point(650, 800), new Point(512, 64)) },
                { SelectMenu.Play,     new Rectangle(new Point(650, 880), new Point(512, 64)) },
            };
            imageState = new Dictionary<SelectMenu, bool>();
            prevImageState = new Dictionary<SelectMenu, bool>();
            for (int i = 0; i < Enum.GetValues(typeof(SelectMenu)).Length; i++)
            {
                imageState.Add(
                    (SelectMenu)Enum.ToObject(typeof(SelectMenu), i),
                    false);

                prevImageState.Add(
                    (SelectMenu)Enum.ToObject(typeof(SelectMenu), i),
                    false);
            }
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
            if (imageState[SelectMenu.Play])
                return EScene.GameScene;

            return EScene.Tutorial;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
            sound.PauseBGM();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (Input.IskeyDown(Keys.Enter))
                isEndFlag = true;

            sound.PlayBGM("title");

            //メニュー状態の受付
            foreach (var menu in acceptAreas)
            {
                //メニュー選択中かつキー入力もされたらシーン終了
                if (Input.GetMouseButtonDown(MouseButton.Left) &&
                    imageState[menu.Key])
                {
                    sound.PlaySE("click");
                    isEndFlag = true;
                }

                //SE再生
                if (imageState[menu.Key] && !prevImageState[menu.Key])
                {
                    sound.PlaySE("active");
                }

                //アクティブ状態の更新
                prevImageState[menu.Key] = imageState[menu.Key];

                //メニュー領域のマウスの侵入状態に応じて状態更新
                if (Input.MousePos().X < menu.Value.Right &&
                    Input.MousePos().X > menu.Value.Left &&
                    Input.MousePos().Y > menu.Value.Top &&
                    Input.MousePos().Y < menu.Value.Bottom)
                {
                    imageState[menu.Key] = true;
                    continue;
                }
                imageState[menu.Key] = false;
            }
        }
    }
}
