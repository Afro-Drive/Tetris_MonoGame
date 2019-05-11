using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// Oikake授業内で作成した内部スコアクラス
    /// </summary>
    class Score
    {
        /// <summary>
        /// フィールド
        /// </summary>
        private int poolScore;
        private int score;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Score()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            score = 0;
            poolScore = 0;
        }

        /// <summary>
        /// スコアの加算(引数なし)
        /// </summary>
        public void Add()
        {
            poolScore++;
        }    

        /// <summary>
        /// スコアの加算(引数あり)
        /// </summary>
        /// <param name="num">加算得点</param>
        public void Add(int num)
        {
            poolScore += num;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if(poolScore > 0)
            {
                score += 10;
                poolScore -= 10;
            }
            else if(poolScore < 0)
            {
                score -= 10;
                poolScore += 10;
            }
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("score", new Vector2(1400, 400));
            renderer.DrawNumber("number", new Vector2(1400, 470), score);
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
            score += poolScore;
            if(score < 0)
            {
                score = 0;
            }
            poolScore = 0;
        }

        /// <summary>
        /// 合計スコアを取得（エンディングの分岐につなげる）
        /// </summary>
        /// <returns>現在の総スコア</returns>
        public int GetScore()
        {
            int currentScore = score + poolScore;
            if(currentScore < 0)
            {
                currentScore = 0;
            }
            return currentScore;
        }
    }
}
