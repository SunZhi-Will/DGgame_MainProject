using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Koroshi
//射箭
public class GameData_ShotArchery : GameData_system
{
    public Archery_Sailor player1_Sailor_scr, player2_Sailor_scr;
    public float ReadyTimed = 3;//顯示Text的時間
    private float nowTiming;
    public Text MessageText;

    [System.Serializable]
    public struct PlayerData
    {
        public GameObject MainObj;//主要物件
        internal Archery_Player control_scr;
        internal Archery_Health Health_scr;
        public void Init()
        {
            control_scr = MainObj.GetComponent<Archery_Player>();
            Health_scr = MainObj.GetComponent<Archery_Health>();
        }
        public void OpenCloseFunction(bool _enabled) 
        {
            control_scr.enabled = _enabled;
            Health_scr.enabled = _enabled;
        }
    }
    public PlayerData player1_Data, player2_Data;

    [System.Serializable]
    public struct BossData
    {
        public GameObject MainObj;//主要物件
        internal Archery_Boss scr;
        internal Archery_Health health_scr;
        public void Init() 
        {
            scr = MainObj.GetComponentInChildren<Archery_Boss>();
            health_scr = MainObj.GetComponentInChildren<Archery_Health>();
        }
        public void OpenCloseFunction(bool _enabled)
        {
            scr.enabled = _enabled;
            health_scr.enabled = _enabled;
        }
    }
    public BossData bossData;
    private bool pause = true;

    protected override void f_Awake()
    {
        player1_Sailor_scr.enabled = false;
        player2_Sailor_scr.enabled = false;
        player1_Data.Init();
        player2_Data.Init();
        bossData.Init();
        nowTiming = ReadyTimed + StartTimed;
    }
    protected override void f_Start()
    {
        OpenGameFunction(false);
    }
    protected override void f_Start_StartTimedEvent()
    {
        player1_Sailor_scr.enabled = true;
        player2_Sailor_scr.enabled = true;
    }

    private bool StartGame;
    void Update()
    {
        TestFunction();
        if (StartGame == false)
        {
            nowTiming -= Time.deltaTime;
            if (nowTiming > 1 && nowTiming < ReadyTimed)
            {
                MessageText.gameObject.SetActive(true);
                MessageText.text = nowTiming.ToString("0");
            }
            else if (nowTiming < 1)
            {
                StartGame = true;
                MessageText.gameObject.SetActive(false);
            }
            return;
        }


        bool _TimeStop = (Time.timeScale == 0);
        if (_TimeStop)
        {
            if (pause == true) { return; }
            OpenGameFunction(pause);
            PauseAudio(true);
            pause = true;
        }
        else
        {
            if (pause == false) { return; }
            OpenGameFunction(pause);
            PauseAudio(false);
            pause = false;
        }
    }

    void OpenGameFunction(bool _enabled) 
    {
        player1_Data.OpenCloseFunction(_enabled);
        player2_Data.OpenCloseFunction(_enabled);
        bossData.OpenCloseFunction(_enabled);
    }
    public void ShowSettlement(int p1,int p2)
    {
        OpenGameFunction(false);
        Common_scr.ShowSettlement(p1, p2);
        gameObject.SetActive(false);
    }

    //測試功能
    void TestFunction()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            //player1_Data.control_scr.Shoot();
            player2_Data.Health_scr.health = 0;
            player1_Data.control_scr.ShotObject().position = player2_Data.MainObj.transform.position + new Vector3(0, 0, -1);
            Debug.Log("測試功能-1P Win畫面");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            //player2_Data.control_scr.Shoot();
            player1_Data.Health_scr.health = 0;
            player2_Data.control_scr.ShotObject().position = player1_Data.MainObj.transform.position + new Vector3(0, 0, 1);
            Debug.Log("測試功能-2P Win畫面");
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            bossData.health_scr.health = 0;
            player1_Data.control_scr.ShotObject().position = bossData.MainObj.transform.position + new Vector3(0, 0, -2);
            Debug.Log("測試功能-1P 擊倒BOSS Win畫面");
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            bossData.health_scr.health = 0;
            player2_Data.control_scr.ShotObject().position = bossData.MainObj.transform.position + new Vector3(0, 0, 2);
            Debug.Log("測試功能-2P 擊倒BOSS Win畫面");
        }
#endif
    }
}
