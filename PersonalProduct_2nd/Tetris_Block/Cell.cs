﻿using Microsoft.Xna.Framework;
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
        /// <summary>
        /// 衝突した矩形(マス)の面の列挙型
        /// </summary>
        public enum  Direction { Top, Bottom, Right, Left }
        protected string assetName;　//使用画像のアセット名
        protected Vector2 position;
        //マス目関連
        protected Rectangle CellArea; //マス目の矩形
        public static readonly int WIDTH = 32;　//縦(静的メンバ)
        public static readonly int HEIGHT = 32;　//横（静的メンバ）
        #endregion フィールド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">使用画像のアセット名</param>
        public Cell(string name)
        {
            //各種メンバの初期化
            this.assetName = name;
            position = Vector2.Zero;
            //マス目を生成
            CellArea = new Rectangle(
                new Point((int)position.X, (int)position.Y),
                new Point(WIDTH, HEIGHT));
        }

        /// <summary>
        /// 位置のプロパティ
        /// get→フィールドpositionを取得
        /// set→フィールドpositionを設定
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// 当たり判定の横幅の取得プロパティ
        /// </summary>
    　　public int Width
        {
            get { return WIDTH; }
        } 

        /// <summary>
        /// 当たり判定の縦の大きさの取得プロパティ
        /// </summary>
        public int Height
        {
            get { return HEIGHT; }
        }

        /// <summary>
        /// 矩形当たり判定の取得プロパティ
        /// </summary>
        public Rectangle CellRect
        {
            get { return CellArea; }
        }

        public Direction CheckDirection(Cell otherCell)
        {
            Point thisCenter = this.CellArea.Center;//自分の中心位置を代入
            Point otherCenter = otherCell.CellArea.Center;//相手の中心位置を代入

            //向きのベクトルを取得
            Vector2 dir =
                new Vector2(thisCenter.X, thisCenter.Y) -
                new Vector2(otherCenter.X, otherCenter.Y);

            //当たっている側面をリターン
            //X成分とY成分でどちらのほうが量が多いか
            if (Math.Abs(dir.X) > Math.Abs(dir.Y))
            {
                //Xの向きが正の時
                if (dir.X > 0)
                {
                    return Direction.Right;
                }
                return Direction.Left; //Xの向きが負の時
            }

            //Y成分が大きく、正の値か？
            if (dir.Y > 0)
            {
                return Direction.Bottom;
            }
            //ブロックに乗った
            return Direction.Top;
        }

        /// <summary>
        /// 自分と相手の当たり判定
        /// </summary>
        /// <param name="otherCell">衝突してくる別のCellオブジェクト</param>
        /// <returns></returns>
        public bool IsCollision(Cell otherCell)
        {
            return CellArea.Intersects(otherCell.CellArea);
        }

        /// <summary>
        /// 衝突処理
        /// (なんか音でも鳴らす予定)
        /// </summary>
        /// <param name="other"></param>
        public virtual void Hit(Cell other)
        {
            
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(assetName, position);
        }

        //抽象メソッド群
        public abstract void Update(GameTime gameTime);//更新処理
        public abstract void Initialize();//初期化処理
        public abstract object Clone(); //オブジェクトのクローン
    }
}
