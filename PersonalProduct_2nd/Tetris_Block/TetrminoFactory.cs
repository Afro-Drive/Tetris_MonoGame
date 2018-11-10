using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PersonalProduct_2nd.Tetris_Block.Tetrimino;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// Tetriminoオブジェクトの生成管理クラス
    /// 作成者:谷永吾
    /// 作成開始日:2018年11月8日
    /// </summary>
    class TetrminoFactory
    {
        //形に対応したTetrimino管理用ディクショナリ
        private Dictionary<Form_mino, Tetrimino> minoManage_Dict;
        private Tetrimino fallMino = null; //排出されたテトリミノ
        

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TetrminoFactory()
        {
            //ディクショナリの初期化
            minoManage_Dict = new Dictionary<Form_mino, Tetrimino>();
        }

        /// <summary>
        /// キャラクターオブジェクトの管理リストへの追加
        /// </summary>
        /// <param name="additionMino">追加するTetriminoオブジェクト(管理用コンストラクタを使用)</param>
        public void AddMino(Tetrimino additionMino)
        {
            if (additionMino is null)//追加オブジェクトが空なら
                return;　//何もせず終了

            //テトリミノ管理ディクショナリに格納
            minoManage_Dict.Add(additionMino.Form, additionMino);
        }

        /// <summary>
        /// テトリミノリストの一括更新処理
        /// </summary>
        public void Update(GameTime gameTime)
        {
            foreach (var minoPair in minoManage_Dict)
            {
                minoPair.Value.Update(gameTime);
            }
        }

        /// <summary>
        /// 管理リスト内要素の一括描画
        /// </summary>
        public void Draw(Renderer renderer)
        {
            foreach (var minoPair in minoManage_Dict)
            {
                minoPair.Value.Draw(renderer);
            }
        }

        /// <summary>
        /// 管理リストの一括初期化
        /// </summary>
        public void Initialize()
        {
            foreach (var minoPair in minoManage_Dict)
            {
                minoPair.Value.Initialize();
            }
        }

        /// <summary>
        /// 死亡キャラの削除
        /// (これはいらないか？)
        /// </summary>
        public void Remove_DeadCharacter()
        {
            //blocks.RemoveAll(player => player.IsDead());
            //spaces.RemoveAll(enemy => enemy.IsDead());
        }
    }
}
