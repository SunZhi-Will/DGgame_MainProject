using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Koroshi
//跳水-遊戲資料
//-角色生成的接口有留著~這很可以~讓整合的人可以不用另外生成

//-Canvas不要用那個多個顯示UI，因為CanvasScaler有可以設定統一解析度的地方，切很多就要一個一個調整，一不注意就會有一個漏掉沒調整導致UI跑掉
//基本上只會有一個處理UI的統一解析度調整，跟另外一個顯示模組與UI用的Canvas兩組
//-Find如果再Start只執行一次可能還好，但在Update中跑超耗效能
//-建議可以在Find找不到東西的時候顯示Debug，不然有時候出了問題還要慢慢去找Code
public class GameData_Diving : GameData_system
{
    public FadeInOut_Tool gameScreen_scr;//主遊戲畫面
    public ObGenerator obGenerator_scr;//遊戲系統
    public GameObject reciprocalNum_obj;//倒數編號物件

    [Header("測試用回合數")]
    public int TestRound = 1;

    //獲勝方塊
    private Transform p1_Win_obj, p2_Win_obj;
    private bool pause = true;
    protected override void f_Awake()
    {
        reciprocalNum_obj.SetActive(false);
    }
    protected override void f_Start()
    {
        if (GameMaster_koroshi.s_GameMaster != null)
            obGenerator_scr.SetTotalGames(GameMaster_koroshi.s_GameMaster.data.Round);//設定目前局數
        else
            obGenerator_scr.SetTotalGames(TestRound);

        obGenerator_scr.enabled = true;
        obGenerator_scr.SiteInitialization();//初始化場景資料
    }
    protected override void f_Start_StartTimedEvent()
    {
        gameScreen_scr.gameObject.SetActive(true);//開啟遊戲UI
        gameScreen_scr.FadeInOutState(FadeInOut_Tool.State.fadeIn);
        reciprocalNum_obj.SetActive(true);
        StartCoroutine(obGenerator_scr.StartCountdown(3f));//遊戲倒數開始
    }

    void Update()
    {
        TestFunction();

        if (Time.timeScale == 0)
        {
            if (pause == true) { return; }
            PauseAudio(true);
            pause = true;
        }
        else
        {
            if (pause == false) { return; }
            PauseAudio(false);
            pause = false;
        }
    }

    public WinningMethod winningMethod;//獲勝動作用
    public void ShowSettlement(int _p1,int _p2) 
    {
        Common_scr.ShowSettlement(_p1, _p2);
        if (_p1 != _p2)
            winningMethod.Winner(_p1 > _p2);
    }

    //測試功能
    void TestFunction()
    {
#if UNITY_EDITOR
        float _maxScael = 100;
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (p1_Win_obj == null) 
            {
                p1_Win_obj = GameObject.Find("P1 Win").transform;
            }
            if (p1_Win_obj == null) { Debug.LogError("尚未找到P1獲勝框!");return; }

            p1_Win_obj.localScale = new Vector3(_maxScael, _maxScael, _maxScael);
            Debug.Log("測試功能-1P Win畫面");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            if (p2_Win_obj == null)
            {
                p2_Win_obj = GameObject.Find("P2 Win").transform;
            }
            if (p2_Win_obj == null) { Debug.LogError("尚未找到P2獲勝框!"); return; }

            p2_Win_obj.localScale = new Vector3(_maxScael, _maxScael, _maxScael);
            Debug.Log("測試功能-2P Win畫面");
        }
#endif
    }


}
