using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Define;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// ステージ構成要素の抽象クラス
    /// 作成者:谷永吾
    /// 作成開始日:2018年11月7日
    /// </summary>
    abstract class Cell : ICloneable
    {
        #region フィールド
        protected string assetName;　//使用画像のアセット名
        #endregion フィールド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">使用画像のアセット名</param>
        public Cell()
        {
            //各種メンバの初期化
            //this.assetName = name;
            Position = Vector2.Zero;
        }

        /// <summary>
        /// 引数アリのコンストラクタ
        /// </summary>
        /// <param name="assetName"></param>
        public Cell(string assetName)
        {
            this.assetName = assetName;
        }

        /// <summary>
        /// 位置のプロパティ
        /// get→positionを取得
        /// set→positionを設定
        /// </summary>
        public Vector2 Position
        {
            get; set;
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(assetName, Position);
        }

        //抽象メソッド群
        public abstract void Update(GameTime gameTime);//更新処理
        public abstract void Initialize();//初期化処理
        public abstract object Clone(); //オブジェクトのクローン
    }
}
