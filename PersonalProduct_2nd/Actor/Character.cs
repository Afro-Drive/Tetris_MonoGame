using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Actor
{
    /// <summary>
    /// キャラクター抽象クラス
    /// 作成者:谷永吾
    /// </summary>
    abstract class Character
    {
        #region フィールド
        protected string name;　//使用画像のアセット名
        protected Vector2 position;//後ほどvector2型に変換
        protected bool isDeadFlag;　//死亡フラグ
        protected IGameMediator mediator; //ゲーム仲介者
        #endregion フィールド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">使用画像のアセット名</param>
        /// <param name="mediator">ゲーム仲介者</param>
        public Character(string name, IGameMediator mediator)
        {
            isDeadFlag = false;
            //各種メンバの初期化
            this.name = name;
            this.mediator = mediator;
            position = Vector2.Zero; //とりあえず(0, 0)で
        }

        /// <summary>
        /// 死亡したか？
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            return isDeadFlag;
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }

        //抽象メソッド群
        public abstract void Update(GameTime gameTime);//更新処理
        public abstract void Initialize();//初期化処理
    }
}
