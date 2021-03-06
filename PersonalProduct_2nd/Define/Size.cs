﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Define
{
    /// <summary>
    /// ブロックの共通規格を定めた静的クラス
    /// 作成者:谷 永吾
    /// </summary>
    static class Size
    {
        //高さ
        public static readonly int HEIGHT = 64;
        //横
        public static readonly int WIDTH = 64;

        //描画位置
        public static readonly Vector2 OFFSET = new Vector2(Size.WIDTH * 2, Size.HEIGHT * -5);
    }
}
