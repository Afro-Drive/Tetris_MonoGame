using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Actor
{
    class CharacterGenerater
    {
        private List<Player> players;//キャラクター管理リスト
        private List<Enemy> enemies;//エネミー管理リスト
        private List<Character> additionalCharas;//追加キャラクターの一時管理用リスト

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharacterGenerater()
        {
            //各種リストの初期化
            players = new List<Player>();
            enemies = new List<Enemy>();
            additionalCharas = new List<Character>();
        }

        /// <summary>
        /// キャラクターオブジェクトの管理リストへの追加
        /// </summary>
        /// <param name="additionChara">追加キャラクターオブジェクト</param>
        public void AddChara(Character additionChara)
        {
            if (additionChara is null)//追加オブジェクトが空なら
                return;　//何もせず終了

            //追加キャラ管理リストに一時的に追加
            additionalCharas.Add(additionChara);
        }

        /// <summary>
        /// キャラクターのリスト割り振り、更新処理
        /// </summary>
        public void Update(GameTime gameTime)
        {
            #region キャラクターの割り振り
            foreach (var chara in additionalCharas)
            {
                if (chara is Player)
                    players.Add((Player)chara);
                else if (chara is Enemy)
                    enemies.Add((Enemy)chara);
            }

            additionalCharas.Clear();//一時管理リストをすべて削除
            #endregion キャラクターの割り振り

            #region 各種リストの更新処理
            players.ForEach(player => player.Update(gameTime));
            enemies.ForEach(enemy => enemy.Update(gameTime));
            #endregion 各種リストの更新処理
        }

        /// <summary>
        /// 管理リスト内要素の一括描画
        /// </summary>
        public void Draw(Renderer renderer)
        {
            players.ForEach(player => player.Draw(renderer));
            enemies.ForEach(enemy => enemy.Draw(renderer));
        }

        /// <summary>
        /// 各種管理リストの一括初期化
        /// </summary>
        public void Initialize()
        {
            foreach (var player in players)
                player.Initialize();

            foreach (var enemy in enemies)
                enemy.Initialize();
        }

        /// <summary>
        /// 死亡キャラの削除
        /// </summary>
        public void Remove_DeadCharacter()
        {
            players.RemoveAll(player => player.IsDead());
            enemies.RemoveAll(enemy => enemy.IsDead());
        }
    }
}
