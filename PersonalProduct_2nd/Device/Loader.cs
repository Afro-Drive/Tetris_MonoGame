using Microsoft.Xna.Framework;
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
    /// アセットのロード抽象クラス
    /// 作成者:谷　永吾
    /// </summary>
    abstract class Loader
    {
        protected string[,] resources;//リソースのアセット名を管理する二次元配列
        protected List<KeyValuePair<string, Texture2D>> pixcels; //内部生成したピクセル用ディクショナリ
        protected int counter;//現在登録しているアセット数
        protected int maxNum;//最大アセット登録数
        protected bool isEndFlag;//終了フラグ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="setResouces"></param>
        public Loader(string[,] setResouces)
        {
            resources = setResouces;//外部からの配列の引数で初期化
        }

        /// <summary>
        /// 内部生成ピクセル用コンストラクタ
        /// </summary>
        /// <param name="pixcelPairs"></param>
        public Loader(List<KeyValuePair<string, Texture2D>> pixcelPairs)
        {
            pixcels = pixcelPairs;
        }

        public void Initialze()
        {
            counter = 0;
            isEndFlag = false;
            maxNum = 0;

            //条件(condition)が偽(false)の場合にエラー文(message)を出力
            Debug.Assert(resources != null,
                "リソースデータの登録情報がすでに登録されていますです。");

            //配列から登録上限数を割り当てる
            maxNum = resources.GetLength(0) + pixcels.Capacity;
        }

        /// <summary>
        /// 最大登録数を取得するアクセサ
        /// </summary>
        public int Regist_MAXNum
        {
            get { return maxNum; }
        }

        /// <summary>
        /// 現在登録数を取得するアクセサ
        /// </summary>
        public int CurrentCount
        {
            get { return counter; }
        }

        /// <summary>
        /// 終了フラグを取得するアクセサ
        /// </summary>
        public bool IsEnd
        {
            get { return isEndFlag; }
        }

        #region 抽象メソッド群
        public abstract void Update(GameTime gameTime);//更新
        #endregion

    }
}
