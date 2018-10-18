using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// シーンオブジェクトの管理者クラス
    /// 作成者:谷永吾
    /// 作成開始日:2018年10月18日
    /// </summary>
    class SceneManager
    {
        private Dictionary<EScene, IScene> collect_Scene;//シーン管理ディクショナリ
        private IScene currentScene = null;//現在シーン(最初は空っぽ)

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SceneManager()
        {
            //ディクショナリの初期化
            collect_Scene = new Dictionary<EScene, IScene>();
        }

        /// <summary>
        /// シーンの追加
        /// </summary>
        /// <param name="addScene">追加するシーンオブジェクト</param>
        /// <param name="eScene">シーンオブジェクトに対応する列挙型</param>
        public void AddScene(EScene eScene, IScene addScene)
        {
            //既に登録済みなら何もしない
            if (collect_Scene.ContainsKey(eScene))
                return;

            //管理ディクショナリに引数で登録
            collect_Scene.Add(eScene, addScene);
        }

        public void SetScene(EScene sceneName)
        {
            if (currentScene != null)//すでに現在シーンに追加済みなら
                currentScene.Shutdown();　//終了処理

            currentScene = collect_Scene[sceneName];//引数に対応するシーンバリュー
            currentScene.Initialize();//シーンの初期化
        }

        /// <summary>
        /// 現在シーンの遷移
        /// </summary>
        public void Next()
        {
            currentScene.Shutdown();
            SetScene(currentScene.Next());
        }

        /// <summary>
        /// 現在シーンの描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            if (currentScene == null)//現在シーンが空なら
                return;　//まだ何もしない

            renderer.Begin(); //シーン管理者の描画メソッドで開始終了を束ねる
            currentScene.Draw(renderer);
            renderer.End();
        }

        /// <summary>
        /// 現在シーンの更新処理
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (currentScene == null) //現在シーンが空っぽなら
                return; //まだ何もしない

            currentScene.Update(gameTime);

            if (currentScene.IsEnd()) //終了フラグが確認されたら
                Next(); //次のシーンへ
        }
    }
}
