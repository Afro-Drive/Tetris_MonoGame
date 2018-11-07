using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Tetris_Block
{
    /// <summary>
    /// Blockクラスに囲まれ、TetriMinoを積み重ねるプレイエリアクラス
    /// 作成者:谷永吾
    /// 作成開始日:2018年11月7日
    /// </summary>
    class LineField
    {
        //ListのListで縦横の２次元配列的構造
        private List<List<Cell>> mapList;
        private DeviceManager deviceManager; //ゲームデバイス

        public LineField(DeviceManager device)
        {
            mapList = new List<List<Cell>>();//マップの実態生成
            this.deviceManager = device;
        }

        /// <summary>
        /// ブロックの追加
        /// </summary>
        /// <param name="lineCnt"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<Cell> addBlock(int lineCnt, string[] line)
        //privateメソッドより先頭を小文字とする
        {
            //コピー元オブジェクト登録用ディクショナリ
            Dictionary<string, Cell> objectDict = new Dictionary<string, Cell>();
            //スペースは0
            objectDict.Add("0", new Space(Vector2.Zero, deviceManager));
            //ブロックは1
            objectDict.Add("1", new Block(Vector2.Zero, deviceManager));

            //作業用リスト
            List<Cell> workList = new List<Cell>();

            int colCnt = 0; //列カウント用
            foreach (var s in line)
            {
                try
                {
                    //ディクショナリから元データを取り出し、クローン機能で複製
                    Cell work = (Cell)objectDict[s].Clone();
                    work.Position = (new Vector2(colCnt * work.GetWidth(), lineCnt * work.GetHeight()));
                    workList.Add(work);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                //列カウンタを増やす
                colCnt += 1;
            }
            return workList;
        }

        /// <summary>
        /// CSVReaderを使ってMapの読み込み
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        public void Load(string filename, string path = "./")
        {
            CSVReader csvReader = new CSVReader();
            csvReader.Read(filename, path);

            var data = csvReader.GetData(); //List<string[]>型で取得

            //1行ごとにmapListに追加していく
            for (int lineCnt = 0; lineCnt < data.Count(); lineCnt++)
            {
                mapList.Add(addBlock(lineCnt, data[lineCnt]));
            }
        }

        /// <summary>
        /// マップリストのクリア
        /// </summary>
        public void Unload()
        {
            mapList.Clear();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var list in mapList)//listはList<Cell>型
            {
                //objがSpaceクラスのオブジェクトなら次へ
                foreach (var obj in list) //objはCell型
                {
                    if (obj is Space)
                        continue;

                    obj.Update(gameTime);
                }
            }
        }

        public void Hit(Cell Cell)
        {
            Point work = Cell.HitArea.Location;//左上の座標を取得
            //配列の何行何列目にいるか計算
            int x = work.X / 32;
            int y = work.Y / 32;

            //移動で食い込んでいる時の修正
            if (x < 1)
                x = 1;
            if (y < 1)
                y = 1;

            Range yRange = new Range(0, mapList.Count() - 1); //行の範囲
            Range xRange = new Range(0, mapList[0].Count() - 1); //列の範囲

            for (int row = y - 1; row <= (y + 1); row++)
            {
                for (int col = x - 1; col <= (x + 1); col++)
                {
                    //配列外なら何もしない
                    //
                    if (xRange.IsOutOfRange(col) || yRange.IsOutOfRange(row))
                        continue;

                    //その場所のオブジェクトを取得
                    Cell obj = mapList[row][col];

                    //objがSpaceクラスのオブジェクトなら次へ
                    if (obj is Space)
                        continue;

                    //衝突判定(CellがBlockの場合)
                    if (obj.IsCollision(Cell))
                        Cell.Hit(obj);
                }
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //すべてのオブジェクト(Block, Space)を要素一つずつ描画していく
            //maplistは二重配列的構造よりループは2重となる
            foreach (var line in mapList) //line is List<Cell>型
            {
                foreach (var cell in line) //cell is Cell型
                    cell.Draw(renderer);
            }
        }
    }
}
