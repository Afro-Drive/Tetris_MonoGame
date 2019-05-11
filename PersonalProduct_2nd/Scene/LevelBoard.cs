using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;
using PersonalProduct_2nd.Tetris_Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// ライン消去数に応じたレベル計測・表示クラス
    /// </summary>
    class LevelBoard
    {
        //フィールド
        //重複レベルアップ防止用ディクショナリ
        Dictionary<int, bool> unMultiple;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LevelBoard()
        {
            Level = 1;

            //今回はレベル10までを想定する
            unMultiple = new Dictionary<int, bool>();
            for (int i = 1; i <= GameScene.CLEAR_LEVEL; i++)
                unMultiple.Add(i, true);
            unMultiple[1] = false;
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("level", new Vector2(1400, 570));
            renderer.DrawNumber("number", new Vector2(1400, 664), Level);
        }

        /// <summary>
        /// レベルアップ
        /// </summary>
        public void LevelUP()
        {
            //対応するレベルがまだアップされていなければレベルアップ
            Level++;
            //レベルに該当するディクショナリにレベルアップ不可のチェックを入れる
            unMultiple[Level] = false;
        }

        /// <summary>
        /// レベルのプロパティ
        /// (割り当ては不可)
        /// </summary>
        public int Level
        {
            get; private set;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
            Level = 1;
        }

        /// <summary>
        /// 重複レベル上昇防止確認
        /// </summary>
        /// <param name="levelKey">確認したいレベル</param>
        public bool JudgeMultiple(int levelKey)
        {
            return unMultiple[levelKey];
        }
    }
}
