using Microsoft.Xna.Framework;
using PersonalProduct_2nd.Device;
using PersonalProduct_2nd.Utility;
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
        private TetrminoFactory factory; //Tetriminoオブジェクト生産者
        private IGameMediator mediator; //ゲーム仲介者

        public LineField(DeviceManager device, IGameMediator mediator)
        {
            mapList = new List<List<Cell>>();//マップの実態生成
            this.mediator = mediator;
            deviceManager = device;
            factory = new TetrminoFactory(mediator);
        }

        /// <summary>
        /// CSVファイルに合わせてブロックの追加
        /// </summary>
        /// <param name="lineCnt"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<Cell> addBlock(int lineCnt, string[] line)
        //privateメソッドより先頭を小文字とする
        {
            //コピー元オブジェクト登録用ディクショナリ
            Dictionary<string, Cell> cellDict = new Dictionary<string, Cell>();
            //スペースは0
            cellDict.Add("0", new Space());
            //ブロックは1
            cellDict.Add("1", new Block());

            //作業用リスト
            List<Cell> workList = new List<Cell>();

            int colCnt = 0; //列カウント用
            foreach (var s in line)
            {
                try
                {
                    //ディクショナリから元データを取り出し、クローン機能で複製
                    Cell work = (Cell)cellDict[s].Clone();
                    //1列の要素を１ブロックずつ配置する
                    work.Position = (new Vector2(colCnt * work.Width, lineCnt * work.Height));
                    workList.Add(work);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                //列カウンタを増やす。次の列へ移動
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

        /// <summary>
        /// フィールド内の空白要素以外を更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (var list in mapList)//listはList<Cell>型
            {
                foreach (var cell in list) //cellはCell型
                {
                    //objがSpaceクラスのオブジェクトなら次へ
                    if (cell is Space)
                        continue;

                    cell.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// 衝突判定・衝突後処理
        /// </summary>
        /// <param name="cell">基準となるセルオブジェクト</param>
        public void Hit(Cell cell)
        {
            Point work = cell.CellRect.Location;//左上の座標を取得
            //配列の何行何列目にいるか計算
            int x = work.X / 32;
            int y = work.Y / 32;

            //移動で食い込んでいる時の修正
            if (x < 1)
                x = 1;
            if (y < 1)
                y = 1;

            Range yRange = new Range(0, mapList.Count() - 1); //行の範囲(配列番号に対応)
            Range xRange = new Range(0, mapList[0].Count() - 1); //列の範囲(配列番号に対応)

            //引数cellの周りの8つのセルに衝突対象がないかどうか確認(テトリミノのブロックごとに改良する必要ありか？)
            for (int row = y - 1; row <= (y + 1); row++)　//自分の上と下のセル
            {
                for (int col = x - 1; col <= (x + 1); col++)　//自分の右と左のセル
                {
                    //配列外なら何もしない
                    //
                    if (xRange.IsOutOfRange(col) || yRange.IsOutOfRange(row))
                        continue;

                    //その場所のオブジェクトを取得(調べる相手)
                    Cell cll = mapList[row][col];

                    //objがSpaceクラスのオブジェクトなら次へ
                    if (cll is Space)
                        continue;

                    //衝突判定(CellがBlockの場合)
                    if (cll.IsCollision(cell))
                        cell.Hit(cll);
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
