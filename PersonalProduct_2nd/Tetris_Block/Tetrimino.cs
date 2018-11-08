using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;
using PersonalProduct_2nd.Utility;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// Blockクラスを継承した操作可能テトリスブロッククラス
    /// 作成者:谷永吾
    /// 作成開始日:2018年11月7日
    /// </summary>
    class Tetrimino : Block
    {
        #region フィールド
        /// <summary>
        /// テトリミノの種類列挙型
        /// </summary>
        private enum Form_mino
        {
            I, T, J, L, S, Z, Test
        }
        private Form_mino form;
        private Cell[,] realRotate_Array = new Cell[4,4]; //回転処理用配列
        private Cell[,] imageRotate_Array; //回転可能か検証する用の配列
        private float GRAVITY = 0.2f; //重力
        private Timer landTimer; //着地後の操作猶予時間
        private SoundManager Sound; //動作音用
        private IGameMediator mediator; //ゲーム仲介者
        #endregion フィールド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mediator">スコア加算用ゲーム仲介者</param>
        public Tetrimino(IGameMediator mediator) 
            : base()
        {
            Sound = DeviceManager.CreateInstance().GetSound();
            form = Form_mino.Test;
            landTimer = new CountDown_Timer(2f);
            this.mediator = mediator;
        }

        /// <summary>
        /// 排出するテトリミノに合わせた描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer)
        {
            switch (form)
            {
                case Form_mino.I:
                    DrawI(renderer);
                    break;

                case Form_mino.J:
                    DrawJ(renderer);
                    break;

                case Form_mino.L:
                    DrawL(renderer);
                    break;

                case Form_mino.S:
                    DrawS(renderer);
                    break;

                case Form_mino.T:
                    DrawT(renderer);
                    break;

                case Form_mino.Z:
                    DrawZ(renderer);
                    break;

                default: //テスト用テトリミノ描画
                    renderer.DrawTexture("tetriMino", Vector2.Zero);
                    break;
            }
        }
        #region テトリミノごとの描画
        public void DrawI(Renderer renderer)
        {

        }

        public void DrawT(Renderer renderer)
        {

        }

        public void DrawJ(Renderer renderer)
        {

        }

        public void DrawL(Renderer renderer)
        {

        }

        public void DrawS(Renderer renderer)
        {

        }

        public void DrawZ(Renderer renderer)
        {

        }
        #endregion テトリミノごとの描画

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// 二次元配列を時計回りに回転
        /// </summary>
        /// <param name="basedArray">回転したい配列</param>
        /// <returns>時計回りに90°回転させた二次元配列</returns>
        public Cell[,] Rotate_Clockwise(Cell[,] basedArray)
        {
            int rows = basedArray.GetLength(0); //列(横)
            int cols = basedArray.GetLength(1); //行(縦)
            imageRotate_Array = new Cell[cols, rows]; //回転前とは行と列を逆にした配列を生成

            for(int y = 0; y < rows; y++)　//回転後の配列の列に回転前の配列の行分要素を用意
            {
                for(int x = 0; x < cols; x++)　//回転後の配列の行に回転前の配列の列分要素を用意
                {
                    //回転後の配列の列は、回転前の配列の行からy(新規配列の列の生成回数)と1を引いたものに一致する
                    //知るかこの野郎
                    imageRotate_Array[x, rows - y - 1] = basedArray[y, x];
                }
            }
            return imageRotate_Array;
        }

        /// <summary>
        /// 二次元配列を反時計回りに回転
        /// </summary>
        /// <param name="basedArray">回転したい二次元配列</param>
        /// <returns>反時計回りに90°回転させた二次元配列</returns>
        public Cell[,] Rotate_AntiClockwise(Cell[,] basedArray)
        {
            int rows = basedArray.GetLength(0); //行(横)
            int cols = basedArray.GetLength(1); //列(縦)
            imageRotate_Array = new Cell[cols, rows];

            for(int y = 0; y < rows; y++)　//回転後の配列の列に回転前の配列の行分だけ要素を用意
            {
                for (int x = 0; x < cols; x++) //回転後の配列の行に回転前の配列の列分だけ要素を用意
                {
                    //回転後の配列の列は回転前の配列の行からx(回転後の列を用意した回数)と1を引いたものに一致する
                    //時計回りとはややこしくなる方が逆になるんだね。知らんけど。
                    imageRotate_Array[rows - x - 1, y] = basedArray[y, x];
                }
            }

            return imageRotate_Array;
        }
    }
}
