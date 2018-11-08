using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Device
{
    /// <summary>
    /// CSV形式ファイルの読み込みクラス
    /// 作成者:谷　永吾
    /// 作成日:2018年10月15日
    /// </summary>
    class CSVReader
    {
        //CSV形式ファイルで読み込んだ1行分ずつマップを格納するリスト
        private List<string[]> mapData = new List<string[]>();

        /// <summary>
        /// コンストラクタ
        /// (特に何もしない)
        /// </summary>
        public CSVReader()
        { }

        /// <summary>
        /// CSVファイルの読み込み
        /// </summary>
        /// <param name="filename">CSVファイル名</param>
        /// <param name="path">ファイルの格納してあるディレクトリ名</param>
        public void Read(string filename, string path = "./")
        {
            Clear(); //まずは所有するリストの中身を掃除する

            //例外処理
            try
            {
                //開くCSVファイルのパスを指定、開く
                using (var sr = new System.IO.StreamReader(@"Content/" + path + filename))
                {
                    //ストリーム末尾まで読み込みつづける
                    while (!sr.EndOfStream)
                    {
                        //1行読み込む
                        var line = sr.ReadLine();
                        //対象の文字列をカンマごとに分けて配列に格納する
                        var values = line.Split(',');

                        //リストに読み込んだ1行を追加(二重配列的構造にする)
                        mapData.Add(values);

#if DEBUG
                        //出力
                        foreach (var v in values)
                        {
                            Console.Write("{0}", v);
                        }
                        Console.WriteLine();
#endif
                    }
                }
            }
            catch (System.Exception e)
            {
                //ファイルオープンが失敗したら
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 管理用リストの要素を全削除
        /// </summary>
        public void Clear()
        {
            mapData.Clear();
        }

        /// <summary>
        /// マップ用文字列型リスト取得メソッド
        /// </summary>
        /// <returns>フィールド内文字列型リスト</returns>
        public List<string[]> GetData()
        {
            return mapData;
        }

        /// <summary>
        /// 二次元配列化されたマップデータの取得
        /// </summary>
        /// <returns>文字列型の二次元ジャグ配列に変換されたフィールド内のmapData</returns>
        public string[][] GetArrayData()
        {
            return mapData.ToArray();
        }

        /// <summary>
        /// 読み込んだCSVファイル内のデータを整数型ジャグ配列で取得
        /// </summary>
        /// <returns>CSVファイル内のデータを整数型に変換した二次元配列</returns>
        public int[][] GetIntData()
        {
            #region 返却用データの枠組みを作る
            //先に整数型の二次元配列を作るための枠組みを作る
            var data = GetArrayData(); //文字列型二次元配列の取得
            int height = data.Count();

            //int y = data.GetLength(0); //行の数
            //int x = data.GetLength(1); //列の数

            ////Int型のジャグ配列生成(縦の長さを指定)
            int[][] map_intData = new int[height][];
            #endregion 返却用データの枠組みを作る
            //List<int> intMapList = new List<int>();
            //foreach (var data in mapData)

            #region 返却用データに反映
            //読み込んだ要素数に対応する列の配列をそれぞれ生成(横の長さを指定)
            for (int i = 0; i < height; i++)
            {
                //文字列型配列の横の長さを取得
                int width = data[i].Count();
                //取得した横の長さで配列を生成(横の長さを指定)
                map_intData[i] = new int[width];
            }
            //　縦のループ
            for (int y = 0; y < height; y++)
            {
                //横ループ
                for (int x = 0; x < map_intData[y].Count(); x++)
                {
                    map_intData[y][x] = int.Parse(data[y][x]); //取り出したリスト内の要素内の文字列型配列を一つずつ数字化
                    //intMapList.Add(intData);
                }
            }
            #endregion 返却用データに反映
            //変換が終わったデータを返却
            return map_intData;
        }

        /// <summary>
        /// 横の要素数が等しい多次元配列を返却する
        /// </summary>
        /// <returns>文字列型Matrix配列</returns>
        public string[,] GetStringMatrix()
        {
            #region 返却用のデータの枠組みを作る
            var data = GetArrayData();
            //今回は横の要素数が等しいいわゆる「Matrix型」、縦横の長さを取得
            int height = data.Count();
            int width = data[0].Count();

            //縦横の長さが等しい配列を生成
            string[,] matrix = new string[height, width];
            #endregion 返却用データの枠組みを作る

            #region 返却用データに反映
            //縦のループ
            for (int y = 0; y < height; y++)
            {
                //横のループ
                for (int x = 0; x < width; x++)
                {
                    //文字列型時に対応する座標に代入
                    matrix[y, x] = data[y][x];
                }
            }
            //結果を返却
            return matrix;
            #endregion 返却用データに反映
        }

        /// <summary>
        /// 横の長さが等しい整数型の二次元配列を取得する
        /// </summary>
        /// <returns>整数型Matrix配列</returns>
        public int[,] GetIntMatrix()
        {
            #region 返却用データの枠組みを作る
            var data = GetArrayData();//文字列型配列を取得
            //縦横が等しいMatrix配列を作るために長さを取得
            int height = data.Count(); //縦
            int width = data[1].Count(); //横

            //返却用データを生成
            int[,] matrix = new int[height, width];
            #endregion 返却用データの枠組みを作る

            #region 返却用データに反映させる
            //縦
            for (int y = 0; y < height; y++)
            {
                //横
                for (int x = 0; x < width; x++)
                {
                    //対応する要素を整数値に変換した後、返却用データに代入
                    matrix[y, x] = int.Parse(data[y][x]);
                }
            }
            return matrix;
            #endregion 返却用データに反映
        }
    }
}
