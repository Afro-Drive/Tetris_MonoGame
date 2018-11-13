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
        //形に対応したTetrimino管理用リスト
        private List<Tetrimino> mino_List;
        private Tetrimino currentMino = null; //排出されたテトリミノ
        private Random rand;//排出するMinoの決定用
        private IGameMediator mediator;//リストの再設定のために所有

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mediator"></param>
        public TetrminoFactory(IGameMediator mediator)
        {
            //リストの初期化
            mino_List = new List<Tetrimino>();
            rand = DeviceManager.CreateInstance().GetRandom();
            this.mediator = mediator;
        }

        /// <summary>
        /// キャラクターオブジェクトの管理ディクショナリへの追加
        /// </summary>
        /// <param name="additionMino">追加するTetriminoオブジェクト(管理用コンストラクタを使用)</param>
        public void AddMino(Tetrimino additionMino)
        {
            if (additionMino is null)//追加オブジェクトが空なら
                return;　//何もせず終了

            //テトリミノ管理Listに格納(Minoの形も指定されている状態)
            mino_List.Add(additionMino);
        }

        /// <summary>
        /// 排出中のテトリミノの更新処理
        /// </summary>
        public void Update(GameTime gameTime)
        {
            #region フィールドに排出されていないため不要
            //foreach (var minoPair in minoManage_Dict) 
            //{
            //    minoPair.Value.Update(gameTime);
            //}
            #endregion フィールドに排出されていないため不要

            Emission(); //落下させるMinoを決定
            if (currentMino == null) return;
            else currentMino.Update(gameTime);　//落下中のMinoの動作処理
        }

        /// <summary>
        /// 管理リスト内要素の一括描画
        /// </summary>
        public void Draw(Renderer renderer)
        {
            #region フィールドに排出されていないため不要
            //foreach (var minoPair in minoManage_Dict)　
            //{
            //    minoPair.Value.Draw(renderer);
            //}
            #endregion フィールドに排出されていないため不要
            if (currentMino == null) return;

            currentMino.Draw(renderer);
        }

        /// <summary>
        /// 管理リストの一括初期化
        /// </summary>
        public void Initialize()
        {
            #region フィールドに排出されていないため不要
            //foreach (var minoPair in minoManage_Dict)　
            //{
            //    minoPair.Value.Initialize();
            //}
            #endregion フィールドに排出されていないため不要
            if (currentMino == null) return;

            currentMino.Initialize();
        }

        /// <summary>
        /// テトリミノの排出
        /// </summary>
        public void Emission()
        {
            //すでにテトリミノが落下中の場合は何もしない
            if (currentMino != null)
                return;
            //ランダムで管理ListからMinoを選択
            //リストの要素数内で乱数を生成
            int index = rand.Next(0, mino_List.Count);
            //リストの要素が0以下になったら何もしない
            if (mino_List.Count <= 0)
                return;
            else
                currentMino = mino_List[index]; //落下状態のミノに代入
            mino_List.RemoveAt(index); //一度排出したミノはリストから削除する
        }

        public void ResetList()
        {
            //まだ管理リストに要素が残っていれば何もしない
            if (mino_List.Count > 0) return;
            //管理リストを元通りにする
            //Minoの形を指定できる管理用コンストラクタで初期化
            mino_List.Add(new Tetrimino(Form_mino.Test, mediator));
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

        /// <summary>
        /// 現在落下中のテトリミノ取得のプロパティ
        /// </summary>
        public Tetrimino FallingMino
        {
            get { return currentMino; }
        }
    }
}
