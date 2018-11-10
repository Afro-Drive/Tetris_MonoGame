﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PersonalProduct_2nd.Tetris_Block;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// ゲームプレイ関連シーン
    /// 作成者:谷永吾
    /// </summary>
    class GameScene : IScene
    {
        private bool isEndFlag;//終了フラグ
        private TetrminoFactory tetriminoFactory;//キャラクター管理者
        private LineField field; //プレイエリアのフィールド

        private DeviceManager device;//デバイス管理者
        private SoundManager sound; //サウンド管理者      

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameScene()
        {
            device = DeviceManager.CreateInstance();
            sound = device.GetSound();

            isEndFlag = false;
            tetriminoFactory = new TetrminoFactory();
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("christmas_dance_tonakai", new Vector2(750, 0));
            field.Draw(renderer);
            tetriminoFactory.Draw(renderer);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEndFlag = false;
            field = new LineField(device);
            field.Load("LineField.csv", "./csv/"); //フィールド元のファイルの読み込み
            tetriminoFactory.Initialize();
        }

        /// <summary>
        /// 終了か？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return isEndFlag;
        }

        /// <summary>
        /// 次のシーンへ
        /// </summary>
        /// <returns>シーンオブジェクトに対応する列挙型</returns>
        public EScene Next()
        {
            return EScene.Result;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (Input.IskeyDown(Keys.Enter))
                isEndFlag = true;
            tetriminoFactory.Update(gameTime);
        }
    }
}
