using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Scene
{
    /// <summary>
    /// シーン名の列挙型クラス
    /// 作成者:谷永吾
    /// 作成日:2018年10月18日
    /// </summary>
    enum EScene
    {
        LoadScene, //ロードシーン
        LogoScene, //ロゴシーン
        Title,　　 //タイトルシーン
        Configuration,//コンフィグシーン
        Tutorial,　//チュートリアル
        GameScene, //ゲームプレイシーン
        GameOver,  //ゲームオーバー
        Clear,     //ゲームクリア
    }
}
