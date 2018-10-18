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
    /// シーンクラスに実装するシーンインターフェイス
    /// 作成者:谷永吾
    /// 作成開始日:谷永吾
    /// </summary>
    interface IScene
    {
        EScene Next(); //次のシーンへ
        bool IsEnd();　//終了トリガー
        void Shutdown();　//終了処理
        void Initialize();　//初期化
        void Draw(Renderer renderer);　//描画処理
        void Update(GameTime gameTime);　//更新処理
    }
}
