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
    /// 描画専用クラス
    /// </summary>
    class Renderer
    {
        //フィールド
        private ContentManager contentManager;//コンテンツ管理者（映したい物の管理）
        private GraphicsDevice graphicsDevice;//グラフィック機器（物を映す）
        private SpriteBatch spriteBatch;//描画用オブジェクト

        //複数画像管理用変数(画像とその画像の名前（アセット名）をDictionaryを用いて紐づける）
        //<アセット名，描画する画像>
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">Game1クラスのコンテンツ管理者</param>
        /// <param name="graphics">Game1クラスのグラフィック機器</param>
    　　public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphicsDevice); //描画時にspriteBatch.～と打つ必要はなくなる（Rendererクラスが所有しているため）
        }

        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="assetName">アセット名（画像名）</param>
        /// <param name="filepath">画像へのパス（ファイルの階層の案内）</param>
        public void LoadContent(string assetName, string filepath = "./")
        {
            //すでにキー（アセット名）が登録されているとき
            if (textures.ContainsKey(assetName))
            {
#if DEBUG //DEBUGモードの時のみ下記エラー文章をコンソールに表示
                Console.WriteLine(assetName + "はすでに読み込まれています。\nプログラムを確認してください。");
#endif
                return;//終了
            }

            //画像の読み込みとDictionaryへアセット名と画像を登録
            //アセット名、それに対応する画像をコンテンツ管理者が読み込み、それぞれを辞書に登録
            textures.Add(assetName, contentManager.Load<Texture2D>(filepath + assetName));
        }

        /// <summary>
        /// Texture2D型のアセットの読み込み
        /// </summary>
        /// <param name="assetName">テクスチャー管理ディクショナリのKey</param>
        /// <param name="texture">assetName（Key）に対応するテクスチャオブジェクト</param>
        public void LoadContent(string assetName, Texture2D texture)
        {
            if (textures.ContainsKey(assetName))
            {
#if DEBUG 
                Console.WriteLine("アセット" + assetName + "はすでに読み込まれています。\nプログラムを確認してください。");
#endif
                return;
            }

            textures.Add(assetName, texture);
        }

        /// <summary>
        /// コンテンツの開放
        /// </summary>
        public void Unload()
        {
            textures.Clear();//所有する画像情報をすべて削除
        }

        /// <summary>
        /// 描画開始
        /// （Renderer型のオブジェクトから直接開始できるようにするためのメソッド）
        /// </summary>
        public void Begin()
        {
            spriteBatch.Begin();//SpriteBatchのメソッドを拝借
        }

        /// <summary>
        /// 描画終了
        /// （Renderer型のオブジェクトから直接終了できるようにするため）
        /// </summary>
        public void End()
        {
            spriteBatch.End();//SpriteBatchのメソッドを拝借
        }

        #region DrawTexture系統
        /// <summary>
        /// 画像の描画（画像サイズそのまま）
        /// </summary>
        /// <param name="assetName">アセット名（画像名）</param>
        /// <param name="position">描画する位置</param>
        /// <param name="alpha">透明値（1.0f:不透明　0.0f:透明）</param>
        public void DrawTexture(string assetName, Vector2 position, float alpha = 1.0f)
        {
            //textures.ContainsKey(assetName):がfalseの場合、第二引数の文章をコンソール出力する
            Debug.Assert(textures.ContainsKey(assetName),
            "描画時にアセット名の指定を間違えたか、画像の読み込み自体できていません。");

            //texturesのassetNameというKeyに対応するValueを描画
            spriteBatch.Draw(textures[assetName], position, Color.White * alpha);
        }

        /// <summary>
        /// 画像の描画（画像を指定範囲内だけ描画）
        /// </summary>
        /// <param name="assetName">アセット名</param>
        /// <param name="position">描画位置</param>
        /// <param name="rectangle">指定範囲（上記のDrawTextureとの違い）</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture(string assetName, Vector2 position, Rectangle rectangle, float alpha = 1.0f)
        {
            Debug.Assert(textures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、画像の読み込み自体できていません。");

            spriteBatch.Draw(
                textures[assetName], 
                position, 
                rectangle, 
                Color.White * alpha);
        }

        /// <summary>
        /// 画像の描画（描画色、透明度を指定）
        /// </summary>
        /// <param name="assetName">描画したい画像名</param>
        /// <param name="position">描画位置</param>
        /// <param name="color">描画色</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture(string assetName, Vector2 position, Color color, float alpha = 1.0f)
        {
            Debug.Assert(textures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、画像の読み込み自体できていません。");

            spriteBatch.Draw(
                textures[assetName],
                position,
                color * alpha);
        }

        /// <summary>
        /// 画像の描画（描画スケールを指定）
        /// </summary>
        /// <param name="assetName">描画するアセット名</param>
        /// <param name="position">描画位置</param>
        /// <param name="sourceRect">切り取り範囲（Nullでもよい）</param>
        /// <param name="scale">描画倍率（縦横等倍）</param>
        /// <param name="origin">画像の中心点</param>
        /// <param name="rotation">回転量（基本0）</param>
        /// <param name="effects">描画のX,Y軸に対する反転の有無（基本なし）</param>
        /// <param name="depth">スプライト深度（知らん）</param>
        public void DrawTexture(
            string assetName,
            Vector2 position, 
            Rectangle? sourceRect,
            float scale,
            Vector2 origin,
            float rotation = 0.0f,
            SpriteEffects effects = SpriteEffects.None,
            float depth = 1.0f)
        {
             spriteBatch.Draw(
                 textures[assetName],
                 position,　　　　　 
                 sourceRect,
                 Color.White, 
                 rotation,
                 origin, 
                 scale, 
                 effects,
                 depth);
        }

        /// <summary>
        /// 画像の描画（描画スケールを指定２）
        /// </summary>
        /// <param name="assetName">描画するアセット名</param>
        /// <param name="position">描画位置</param>
        /// <param name="sourceRect">切り取り範囲（Nullでもよい）</param>
        /// <param name="scale">描画倍率（縦倍率・横倍率指定）</param>
        /// <param name="origin">画像の中心点(描画の基準点を設定できる、基本は(0, 0)のまま)</param>
        /// <param name="rotation">回転量（基本0）</param>
        /// <param name="effects">描画のX,Y軸に対する反転の有無（基本なし）</param>
        /// <param name="depth">スプライト深度（知らん）</param>
        public void DrawTexture(
            string assetName,
            Vector2 position,
            Rectangle? sourceRect,
            Vector2 scale,
            Vector2 origin,
            float rotation = 0.0f,
            SpriteEffects effects = SpriteEffects.None,
            float depth = 1.0f)
        {
            spriteBatch.Draw(
                textures[assetName],
                position,
                sourceRect,
                Color.White,
                rotation,
                origin,
                scale,
                effects,
                depth);
        }
        #endregion　DrawTexture系統

        /// <summary>
        /// 数字の描画（整数）
        /// </summary>
        /// <param name="assetName">使用画像名</param>
        /// <param name="position">描画位置</param>
        /// <param name="number">表示する数字</param>
        /// <param name="alpha">透明度（1.0=不透明,　0.0=透明）</param>
        public void DrawNumber(
            string assetName,
            Vector2 position,
            int number,
            float alpha = 1.0f)
        {
            //デバッグモード時のみ、描画時に使用する画像の有無を確認
            Debug.Assert(textures.ContainsKey(assetName),
                "描画時にアセット名を間違えたか、" +
                "Game1において画像の読み込み自体できていません。");

            if (number < 0) number = 0;//負の場合は0にする

            int width = 32;//数字1文字当たりの横幅を指定
                           
            //数字を文字化、一桁ごとに矩形を並べて描画していく
            foreach (var drawNum in number.ToString())
            {
                spriteBatch.Draw(
                    textures[assetName],//数字の連番画像名
                    position,//描画位置(基準は右上）
                    new Rectangle((drawNum - '0') * width, 0, width, 64),//文字コードでの引き算を行い、連番内の描画する数字を決定
                    Color.Black);

                //1文字描画後、1桁分右にずらす（次の桁を描画する準備）
                position.X += width;
            }
        }
    }
}
