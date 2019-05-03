using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Device
{
    /// <summary>
    /// ゲームの操作関連の仲介者
    /// 作成者:谷永吾
    /// 作成開始日:2018年10月18日
    /// </summary>
    interface IGameMediator
    {
        //スコアの加算
        void AddScore(int num);

        //スコアの取得
        int GetScore();

        //消去したライン数の取得
        int GetRemoveLineValue();

        //消去したラインを加算
        void AddRemoveLine();
    }
}
