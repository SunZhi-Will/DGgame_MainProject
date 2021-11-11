using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Frank;

//Koroshi
//拳擊
//如果是跨場景的話除非是像遊戲標題→遊戲畫面，不然小遊戲版，其實不用特定跨場景，像格鬥天王Round1~Round2，如果要跨場景~玩家還要重新讀取場景
//static一個沒注意可能就漏掉忘了清除，然後就要找很久Orz
//如果是跨場景像"標題選像->遊戲畫面"通常統一管理比較好(GameMaster_koroshi，雖然寫得很爛)
//PS:有些東西其實也可以不用特地用算的
public class GameData_Boxing : GameData_system
{
    public CharacterManager characterManager_scr;

    [Header("遊戲進行中關閉物件")]
    public GameObject[] GamingClose_obj_array;
    private KeyboardInput[] KeyboardInput_scr_array;
    public GameManager gameManager_scr;//遊戲系統

    //測試功能
    private HPController[] HPController_scr_array;
    private List<HPController> p1_HPController_scr_list;
    private List<HPController> p2_HPController_scr_list;
    public Timer Timer_scr;//倒數時間
    //---+----

    [System.Serializable]
    public struct Screen_Score 
    {
        public Text p1_Text, p2_Text, round_Text;
    }
    public Screen_Score screen_Score;

    //private bool FirstRound;//第一回合
    public int maxRound;
    public float maxTimed;

    private bool EnterEnd;//進入結算
    private bool pause = true;

    protected override void f_Awake()
    {
        KeyboardInput_scr_array = FindObjectsOfType<KeyboardInput>();
        Timer_scr.gameObject.SetActive(false);

#if UNITY_EDITOR
        HPController_scr_array = FindObjectsOfType<HPController>();
        p1_HPController_scr_list = new List<HPController>();
        p2_HPController_scr_list = new List<HPController>();
        for (int i = 0; i < HPController_scr_array.Length; i++)
        {
            if (HPController_scr_array[i].tag == "Player 1")
                p1_HPController_scr_list.Add(HPController_scr_array[i]);
            else if (HPController_scr_array[i].tag == "Player 2")
                p2_HPController_scr_list.Add(HPController_scr_array[i]);
        }
#endif
        //開始遊戲前
        if (GameManager.g_CurrentRound == 0) 
        {
            if (GameMaster_koroshi.s_GameMaster != null)
            {
                maxRound = GameMaster_koroshi.s_GameMaster.data.Round;
                maxTimed = GameMaster_koroshi.s_GameMaster.data.Timed;
            }
            gameManager_scr.SetRound(maxRound);
            gameManager_scr.SetTime(maxTimed);
            Debug.Log("初次進行遊戲");
        }
        else 
        {
            StartTimed = 0.5f;
            //開始遊戲後關閉物件
            for (int i = 0; i < GamingClose_obj_array.Length; i++)
            {
                GamingClose_obj_array[i].SetActive(false);
            }
        }

        if (GameMaster_koroshi.s_GameMaster != null)
        {
            characterManager_scr.player1Index = GameMaster_koroshi.s_GameMaster.p1_SelectCharacter;
            characterManager_scr.player2Index = GameMaster_koroshi.s_GameMaster.p2_SelectCharacter;
        }

    }
    protected override void f_Start()
    {

    }
    protected override void f_Start_StartTimedEvent()
    {
        Common_scr.Set_AgainAndRestart(() => { gameManager_scr.OnClickRestartButton(); Debug.Log("再次遊玩"); });//再次遊玩
        Common_scr.Set_GiveUpAndBackTitle(() => { gameManager_scr.OnClickReturnButton(); Debug.Log("返回標題"); });//返回標題
        Timer_scr.gameObject.SetActive(true);
        gameManager_scr.OnClickStartButton();//自動開始遊戲
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            if (pause == false)
            {
                for (int i = 0; i < KeyboardInput_scr_array.Length; i++)
                    KeyboardInput_scr_array[i].enabled = false;

                PauseAudio(true);
                pause = true;
            }
        }
        else 
        {
            if (pause == true)
            {
                for (int i = 0; i < KeyboardInput_scr_array.Length; i++)
                    KeyboardInput_scr_array[i].enabled = true;

                PauseAudio(true);
                pause = false;
            }
        }




        TestFunction();
        //遊戲結束
        //if (FirstRound == true) { return; }
        if (gameManager_scr.Game_IsEnd == false) { return; }
        if (EnterEnd == true) { return; }
        StartCoroutine(IE_ShowSettlement());
        EnterEnd = true;
    }
    public float timed = 1.5f;
    IEnumerator IE_ShowSettlement() 
    {
        Debug.Log("執行結算準備");
        Time.timeScale = 1;
        yield return new WaitForSeconds(timed);
        Common_scr.ShowSettlement(GameManager.g_Player1Score, GameManager.g_Player2Score);//獲勝模板
        Debug.Log("遊戲結束");
        gameObject.SetActive(false);
    }

    //測試功能
    void TestFunction()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            for (int i = 0; i < p2_HPController_scr_list.Count; i++)
                p2_HPController_scr_list[i].currentHP = 0;
            Debug.Log("測試功能-1P Win畫面");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            for (int i = 0; i < p1_HPController_scr_list.Count; i++)
                p1_HPController_scr_list[i].currentHP = 0;
            Debug.Log("測試功能-2P Win畫面");
        }
#endif
    }

    private void OnDisable()
    {

    }
}
