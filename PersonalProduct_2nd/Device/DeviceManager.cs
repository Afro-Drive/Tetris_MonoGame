using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Device
{
    /// <summary>
    /// デバイス関連管理者クラス（インスタンス・継承不可）
    /// 作成者:谷 永吾
    /// 作成開始日:2018年10月9日
    /// </summary>
    sealed class DeviceManager
    {
        #region フィールド
        private static DeviceManager Instance;//自分自身のインスタンス用変数（他クラスでの生成不可）

        private SoundManager soundManager;    //サウンド管理者
        private Renderer renderer;            //描画管理者
        private ContentManager contentManager;//コンテンツ管理者
        private GraphicsDevice graphics;      //グラフィック管理者
        private GameTime gameTime;            //ゲーム時間
        private Random random;                //ランダムオブジェクト
        #endregion

        /// <summary>
        /// コンストラクタ（アクセス制限状態）
        /// </summary>
        /// <param name="content">コンテンツ管理者</param>
        /// <param name="graphics">グラフィック管理者</param>
        private DeviceManager(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;//引数のコンテンツ管理者オブジェクトで初期化
            this.graphics = graphics;

            //各種デバイスの初期化
            soundManager = new SoundManager(contentManager);
            renderer = new Renderer(contentManager, graphics);
            gameTime = new GameTime();
            random = new Random();
        }

        /// <summary>
        /// インスタンスを生成・取得
        /// (ここからのみDeviceManagerの機能を取得・使用が可能）
        /// </summary>
        /// <returns>DeviceManagerのインスタンスフィールド</returns>
        public static DeviceManager CreateInstance(ContentManager content, GraphicsDevice graphics)
        {
            //唯一のコンストラクタの生成
            if (Instance == null)
                Instance = new DeviceManager(content, graphics);
            return Instance;
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <returns>DeviceManagerクラスのインスタンス</returns>
        public static DeviceManager CreateInstance()
        {
            Debug.Assert(Instance != null,
                "Game1クラスのInitializeメソッド内で引数付きCreateInstanceメソッドを読み出してください。");

            return Instance;
        }

        /// <summary>
        /// 描画管理者の取得
        /// </summary>
        /// <returns>DeviceManagerのRendererフィールド</returns>
        public Renderer GetRenderer()
        {
            return renderer;
        }

        /// <summary>
        /// サウンド管理者の取得
        /// </summary>
        /// <returns>DeviceManagerのSoundManagerフィールド</returns>
        public SoundManager GetSound()
        {
            return soundManager;
        }

        /// <summary>
        /// 乱数オブジェクトの取得
        /// </summary>
        /// <returns>DeviceManagerのRandomフィールド</returns>
        public Random GetRandom()
        {
            return random;
        }
    }
}
