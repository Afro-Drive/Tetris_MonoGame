// このファイルで必要なライブラリのnamespaceを指定
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Define;
using PersonalProduct_2nd.Device;
using PersonalProduct_2nd.Scene;
using PersonalProduct_2nd.Tetris_Block;

/// <summary>
/// プロジェクト名がnamespaceとなります
/// </summary>
namespace PersonalProduct_2nd
{
    /// <summary>
    /// ゲームの基盤となるメインのクラス
    /// 親クラスはXNA.FrameworkのGameクラス
    /// </summary>
    public class Game1 : Game
    {
        // フィールド（このクラスの情報を記述）
        private GraphicsDeviceManager graphicsDeviceManager;//グラフィックスデバイスを管理するオブジェクト
        private SpriteBatch spriteBatch;//画像をスクリーン上に描画するためのオブジェクト

        #region 各種管理者
        private SceneManager sceneManager;//シーン管理者
        private DeviceManager deviceManager;//デバイス関連管理者
        private SoundManager soundManager;//サウンド関連管理者
        private Renderer renderer;//描画管理者
        #endregion 各種管理者

        /// <summary>
        /// コンストラクタ
        /// （new で実体生成された際、一番最初に一回呼び出される）
        /// </summary>
        public Game1()
        {
            //グラフィックスデバイス管理者の実体生成
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            //コンテンツデータ（リソースデータ）のルートフォルダは"Contentに設定
            Content.RootDirectory = "Content";

            Window.Title = "1年後期_第二回個人制作";

            //画面サイズの決定
            graphicsDeviceManager.PreferredBackBufferHeight = Screen.HEIGHT;　//横の反映
            graphicsDeviceManager.PreferredBackBufferWidth = Screen.WIDTH; //縦の反映
        }

        /// <summary>
        /// 初期化処理（起動時、コンストラクタの後に1度だけ呼ばれる）
        /// </summary>
        protected override void Initialize()
        {
            // この下にロジックを記述
            #region　デバイスの取得
            deviceManager = DeviceManager.CreateInstance(Content, GraphicsDevice);
            soundManager = deviceManager.GetSound();
            renderer = deviceManager.GetRenderer();
            #endregion

            #region　シーンの追加、設定
            sceneManager = new SceneManager();
            sceneManager.AddScene(EScene.LoadScene, new LoadScene());
            sceneManager.AddScene(EScene.LogoScene, new LogoScene());
            sceneManager.AddScene(EScene.Title,     new Title());
            sceneManager.AddScene(EScene.Tutorial,  new Tutorial());
            sceneManager.AddScene(EScene.GameScene, new GameScene());
            sceneManager.AddScene(EScene.Result,    new Result());
            sceneManager.SetScene(EScene.LoadScene); //最初はロードシーンにする
            #endregion シーンの追加、設定

            // この上にロジックを記述
            base.Initialize();// 親クラスの初期化処理呼び出し。絶対に消すな！！
        }

        /// <summary>
        /// コンテンツデータ（リソースデータ）の読み込み処理
        /// （起動時、１度だけ呼ばれる）
        /// </summary>
        protected override void LoadContent()
        {
            // 画像を描画するために、スプライトバッチオブジェクトの実体生成
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // この下にロジックを記述
            renderer.LoadContent("christmas_dance_tonakai"); //シーン遷移確認用画像
            renderer.LoadContent("black");

            #region テトリミノの色データ(後で用意)
            Texture2D tetriMino = new Texture2D(GraphicsDevice, 4, 4); //4×4のピクセルデータを生成(中身はまだ何もなし)
            Color[] test_Mino = new Color[tetriMino.Height * tetriMino.Width];
            for (int i = 0; i < test_Mino.Length; i++)
                test_Mino[i] = Color.Blue;

            tetriMino.SetData(test_Mino);
            renderer.LoadContent("tetriMino", tetriMino);
            #endregion テトリミノの色データ

            // この上にロジックを記述
        }

        /// <summary>
        /// コンテンツの解放処理
        /// （コンテンツ管理者以外で読み込んだコンテンツデータを解放）
        /// </summary>
        protected override void UnloadContent()
        {
            // この下にロジックを記述


            // この上にロジックを記述
        }

        /// <summary>
        /// 更新処理
        /// （1/60秒の１フレーム分の更新内容を記述。音再生はここで行う）
        /// </summary>
        /// <param name="gameTime">現在のゲーム時間を提供するオブジェクト</param>
        protected override void Update(GameTime gameTime)
        {
            // ゲーム終了処理（ゲームパッドのBackボタンかキーボードのエスケープボタンが押されたら終了）
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                 (Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                Exit();
            }

            // この下に更新ロジックを記述
            Input.Update();

            sceneManager.Update(gameTime);

            // この上にロジックを記述
            base.Update(gameTime); // 親クラスの更新処理呼び出し。絶対に消すな！！
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="gameTime">現在のゲーム時間を提供するオブジェクト</param>
        protected override void Draw(GameTime gameTime)
        {
            // 画面クリア時の色を設定
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // この下に描画ロジックを記述
            sceneManager.Draw(renderer);

            //この上にロジックを記述
            base.Draw(gameTime); // 親クラスの更新処理呼び出し。絶対に消すな！！
        }
    }
}
