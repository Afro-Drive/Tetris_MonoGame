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
    /// キャラクター継承エネミークラス
    /// 作成者:谷永吾
    /// </summary>
    class Enemy : Character
    {
        //フィールド
        private bool isDeadFlag;//死亡フラグ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mediator">ゲーム仲介者</param>
        public Enemy(IGameMediator mediator)
            : base("", mediator)//使用画像アセット名が決まり次第記入
        {
            isDeadFlag = false;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            //position = new Vector2(, );
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public override void Update(GameTime gameTime)
        {

        }
    }
}
