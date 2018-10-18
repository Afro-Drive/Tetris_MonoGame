using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProduct_2nd.Device
{
    /// <summary>
    /// BGM・SE関連管理クラス
    /// 作成者:谷 永吾:谷 永吾
    /// 作成開始日:2018年10月8日
    /// </summary>
    class SoundManager
    {
        #region フィールド
        //コンテンツ管理者
        private ContentManager contentManager;
        //BGM管理ディクショナリ
        private Dictionary<string, Song> dict_BGM;
        //SE管理ディクショナリ
        private Dictionary<string, SoundEffect> dict_SE;
        //SEインスタンス管理ディクショナリ
        private Dictionary<string, SoundEffectInstance> dict_SEInstance;
        //再生中SE管理ディクショナリ
        private Dictionary<string, SoundEffectInstance> dict_PlayingSE;
        private string onBGM;//現在のBGM
        #endregion フィールド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">コンテンツ管理者</param>
        public SoundManager(ContentManager content)
        {
            contentManager = content;
            MediaPlayer.IsRepeating = true;//リピート再生をオン
            //ちなみにMediaPlayerクラスは静的公開クラスのため生成は必要ない

            //各種管理ディクショナリを初期化
            dict_BGM = new Dictionary<string, Song>();
            dict_SE = new Dictionary<string, SoundEffect>();
            dict_SEInstance = new Dictionary<string, SoundEffectInstance>();
            dict_PlayingSE = new Dictionary<string, SoundEffectInstance>();
            onBGM = null;//現在再生中の音楽をカラっぽに
        }

        /// <summary>
        /// 各種管理ディクショナリの削除
        /// </summary>
        public void Unload()
        {
            dict_BGM.Clear();
            dict_SE.Clear();
            dict_PlayingSE.Clear();
            dict_PlayingSE.Clear();
        }

        #region BGM（MP3）関連
        /// <summary>
        /// BGM（MP3形式）の読み込み
        /// </summary>
        /// <param name="assetName">MP3ファイルの名前</param>
        /// <param name="filepath">MP3ファイルが格納されているディレクトリ名</param>
        public void LoadBGM(string assetName, string filepath = "./")
        {
            //すでに登録済みなら何もしない
            if (dict_BGM.ContainsKey(assetName))
            {
                return;
            }
            //音楽名をKeyとしてBGMファイルをValueで紐づけし、管理ディクショナリに登録
            dict_BGM.Add(assetName, contentManager.Load<Song>(filepath + assetName));
        }

        /// <summary>
        /// BGMが再生中か？
        /// </summary>
        /// <returns>MediaPlayerクラスの静的公開フィールド
        /// State（列挙型MediaStateクラス）がPlaying（再生中）かどうか</returns>
        public bool IsPlayingBGM()
        {
            return MediaPlayer.State == MediaState.Playing;
        }

        /// <summary>
        /// BGMが停止中か？
        /// </summary>
        /// <returns>MediaPlayerクラスの静的公開フィールド
        /// State（MediaState列挙型クラス）がStoppedかどうか</returns>
        public bool IsStoppedBGM()
        {
            return MediaPlayer.State == MediaState.Stopped;
        }

        /// <summary>
        /// BGMが一時停止中か？
        /// </summary>
        /// <returns>MediaPlayerクラス静的公開フィールド
        /// State（MediaState列挙型クラス）がPausedかどうか</returns>
        public bool IsPausedBGM()
        {
            return MediaPlayer.State == MediaState.Paused;
        }

        /// <summary>
        /// BGMを止める
        /// </summary>
        public void StopBGM()
        {
            MediaPlayer.Stop();
            onBGM = null;//再生中BGMを空っぽにする
        }

        /// <summary>
        /// BGMを再生
        /// </summary>
        /// <param name="BGM_name">再生したい管理ディクショナリのKey</param>
        public void PlayBGM(string BGM_name)
        {
            Debug.Assert(dict_BGM.ContainsKey(BGM_name), "後ほどメソッドでまとめる予定");

            if (onBGM == BGM_name) //すでに現在BGMに指定済みなら何もしない
                return;
            if (IsPlayingBGM()) //すでに再生中なら止める
                StopBGM();

            MediaPlayer.Volume = 1.0f;//0.5の音量で指定
            onBGM = BGM_name;　//現在BGMに引数を代入
            MediaPlayer.Play(dict_BGM[onBGM]);　//指定BGMを再生
        }

        /// <summary>
        /// BGMの再生（音量指定）
        /// </summary>
        /// <param name="BGM_name">再生する音楽ファイルに対応する管理ディクショナリのKey</param>
        /// <param name="volume">再生音量</param>
        public void PlayBGM(string BGM_name, float volume)
        {
            Debug.Assert(dict_BGM.ContainsKey(BGM_name), "後ほどメソッドでまとめる予定");

            if (onBGM == BGM_name) //すでに現在BGMに指定済みなら何もしない
                return;
            if (IsPlayingBGM()) //すでに再生中なら止める
                StopBGM();

            MediaPlayer.Volume = volume;//0.5の音量で指定
            onBGM = BGM_name;　//現在BGMに引数を代入
            MediaPlayer.Play(dict_BGM[onBGM]);　//指定BGMを再生
        }

        /// <summary>
        /// BGMの一時停止
        /// </summary>
        public void PauseBGM()
        {
            if (IsPlayingBGM())
                MediaPlayer.Pause();
        }

        /// <summary>
        /// BGMを停止時点から再度再生
        /// </summary>
        public void ResumeBGM()
        {
            if (IsPausedBGM())
                MediaPlayer.Resume();
        }

        /// <summary>
        /// BGMループフラグを変更・指定
        /// </summary>
        /// <param name="loopFlag">変更したいフラグ値（true or false）</param>
        public void ChangeBGMLoopFlag(bool loopFlag)
        {
            MediaPlayer.IsRepeating = loopFlag;
        }
        #endregion

        #region SE(WAV)関連
        /// <summary>
        /// SE（wav形式）の読み込み
        /// </summary>
        /// <param name="assetName">アセット名</param>
        /// <param name="filepath">ロードしたいSEファイルがあるディレクトリ</param>
        public void LoadSE(string assetName, string filepath = "./")
        {
            if (dict_SE.ContainsKey(assetName))
                return;
            dict_SE.Add(assetName, contentManager.Load<SoundEffect>(filepath + assetName));
        }

        /// <summary>
        /// SE（WAV形式）の再生
        /// </summary>
        /// <param name="SE_name">再生したいSEファイルに対応するKey</param>
        public void PlaySE(string SE_name)
        {
            Debug.Assert(dict_SE.ContainsKey(SE_name), "後ほど実装予定");

            dict_SE[SE_name].Play();
        }

        /// <summary>
        /// SE(WAV形式)の再生（音量指定）
        /// </summary>
        /// <param name="SE_name">再生するSEファイルに対応するKey</param>
        /// <param name="volume">再生音量（0.0～1.0）</param>
        public void PlaySE(string SE_name, float volume)
        {
            Debug.Assert(dict_SE.ContainsKey(SE_name), "後ほど実装予定");

            //指定ボリュームで再生
            dict_SE[SE_name].Play(volume, 0.0f, 0.0f);
        }
        #endregion

        #region SEインスタンス関連
        /// <summary>
        /// 任意のSEの詳細な操作のためのインスタンスの取得
        /// </summary>
        /// <param name="name">詳細操作をしたいSE名</param>
        public void SE_Instance(string name)
        {
            if (dict_SEInstance.ContainsKey(name))
                return; //すでに同一インスタンスを取得済みなら何もしない

            Debug.Assert(dict_SE.ContainsKey(name), "先に" + name + "の読み込み処理を行ってください。");

            //SEインスタンス管理ディクショナリに　引数名をKey、
            //それに対応するSE管理ディクショナリのValueのインスタンスをValueとして登録
            dict_SEInstance.Add(name, dict_SE[name].CreateInstance());
        }

        /// <summary>
        /// SEインスタンスでのSE再生
        /// </summary>
        /// <param name="name">再生するSEインスタンス名</param>
        /// <param name="no">同時再生時の管理用番号</param>
        /// <param name="loopFlag">ループ状態の指定</param>
        public void PlaySEInstance(string name, int no, bool loopFlag = false)
        {
            Debug.Assert(dict_SEInstance.ContainsKey(name), "後ほど実装予定");

            if (dict_PlayingSE.ContainsKey(name + no))
                return;　//すでに再生中SEディクショナリに登録済みなら何もしない

            var SE_data = dict_SEInstance[name]; //引数に対応するSEインスタンスのValueを取り出す
            //Value(SoundEffectInstance型)の公開フィールドisLoopFlagの値をfalseに変更（非ループ状態にする）
            SE_data.IsLooped = loopFlag;
            SE_data.Play(); //非ループ状態でSEを再生する
            //再生中SE管理ディクショナリに追加→これにより複数のSEを再生できる
            dict_PlayingSE.Add(name + no, SE_data);
        }

        /// <summary>
        /// SEの停止（指定）
        /// </summary>
        /// <param name="name">停止させたいSEファイル名</param>
        /// <param name="no">管理番号</param>
        public void StoppedSE(string name, int no)
        {
            if (dict_PlayingSE.ContainsKey(name + no) == false)
                return; //ディクショナリに登録されてなければ何もしない
            if (dict_PlayingSE[name + no].State == SoundState.Playing)
                dict_PlayingSE[name + no].Stop();　//再生中ディクショナリの該当要素のSEを停止
        }

        /// <summary>
        /// 再生中SEの全停止
        /// </summary>
        public void StoppedSE()
        {
            foreach (var se in dict_PlayingSE)
                if (se.Value.State == SoundState.Playing)
                    se.Value.Stop();
        }

        /// <summary>
        /// SEの削除
        /// </summary>
        /// <param name="name">削除したいSEファイル名</param>
        /// <param name="no">管理用番号</param>
        public void RemoveSE(string name, int no)
        {
            if (dict_PlayingSE.ContainsKey(name + no) == false)
                return;
            dict_PlayingSE.Remove(name + no);//一致するKeyに対応するValueを削除
        }

        /// <summary>
        /// SEの一時停止（指定）
        /// </summary>
        /// <param name="name">一時停止したいSEファイル</param>
        /// <param name="no">管理番号</param>
        public void PauseSE(string name, int no)
        {
            if (dict_PlayingSE.ContainsKey(name + no) == false)
                return;
            if (dict_PlayingSE[name + no].State == SoundState.Playing)
                dict_PlayingSE[name + no].Pause();
        }

        /// <summary>
        /// 再生中SEのすべてを一時停止
        /// </summary>
        public void PauseSE()
        {
            foreach (var se in dict_PlayingSE)
                if (se.Value.State == SoundState.Playing)
                    se.Value.Pause();
        }

        /// <summary>
        /// 停止中のSEを続きから再生（指定）
        /// </summary>
        /// <param name="name">再生したいSEファイル</param>
        /// <param name="no">管理番号</param>
        public void ResumeSE(string name, int no)
        {
            if (dict_PlayingSE.ContainsKey(name + no) == false)
                return;
            if (dict_PlayingSE[name + no].State == SoundState.Paused)
                dict_PlayingSE[name + no].Resume();
        }

        /// <summary>
        /// 停止中のSEをすべて続きから再生
        /// </summary>
        public void ResumeSE()
        {
            foreach (var se in dict_PlayingSE)
                if (se.Value.State == SoundState.Paused)
                    se.Value.Resume();
        }

        /// <summary>
        /// SEインスタンスは再生中か？
        /// </summary>
        /// <param name="name">再生中か確かめたいSEファイル名</param>
        /// <param name="no">管理番号</param>
        /// <returns></returns>
        public bool IsPlayingSEInstance(string name, int no)
        {
            return dict_PlayingSE[name + no].State == SoundState.Playing;
        }

        /// <summary>
        /// SEインスタンスは一時停止中か？
        /// </summary>
        /// <param name="name">一時停止中か確かめたいSEファイル</param>
        /// <param name="no">管理番号</param>
        /// <returns></returns>
        public bool IsPauseSEInstance(string name, int no)
        {
            return dict_PlayingSE[name + no].State == SoundState.Paused;
        }
        #endregion
    }
}
