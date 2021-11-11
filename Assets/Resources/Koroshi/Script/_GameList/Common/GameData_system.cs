using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//所有遊戲共通資料
//繼承盡量只保持一層比較好掌握架構~((自幹的話就當廢話
public class GameData_system : MonoBehaviour
{
    [Header("自動開始遊戲 等待時間")]
    public float StartTimed = 1f;
    [Header("目前場景名稱")]
    public string NowSceneName;//重新讀取用

    protected Change change_scr;

    [HideInInspector]
    public GameCommon Common_scr;

    protected AudioSource[] audio_array;
    protected bool[] audio_isPlay_array;

    protected void Awake()
    {
        Common_scr = FindObjectOfType<GameCommon>();
        if (Common_scr == null) { Debug.LogError("Common_scr 不存在!"); }
        change_scr = GetComponent<Change>();
        if (change_scr == null) { change_scr = gameObject.AddComponent<Change>(); }

        //再次遊玩，返回標題
        Common_scr.Set_AgainAndRestart(() => { Restart(); });//再次遊戲

        f_Awake();
    }

    protected virtual void f_Awake() { }

    IEnumerator Start() 
    {
        f_Start();
        yield return new WaitForSeconds(StartTimed);
        f_Start_StartTimedEvent();
        audio_array = FindObjectsOfType<AudioSource>();//因為有的會之後才執行
        audio_isPlay_array = new bool[audio_array.Length];
        Debug.Log("執行 f_Start_StartTimedEvent");
    }
    protected virtual void f_Start() { }
    protected virtual void f_Start_StartTimedEvent() { }


    protected void PauseAudio(bool _pause)
    {
        if (audio_array == null) { return; }
        for (int i = 0; i < audio_array.Length; i++)
        {
            if (_pause)
            {
                audio_isPlay_array[i] = audio_array[i].isPlaying;
                audio_array[i].Pause();
            }
            else
            {
                if(audio_isPlay_array[i])
                    audio_array[i].Play();
            }
        }
    }

    //暫停、結算 -> 重新開始遊戲
    protected void Restart()
    {
        change_scr.ChangeScene(NowSceneName);
        Debug.Log("重新開始遊戲");
    }
}
