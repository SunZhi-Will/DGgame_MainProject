using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Koroshi
//棒球
//沒問題啦~哪次有問題~
public class GameData_BaseBallGame : GameData_system
{
    public int Round = 1;

    public GameObject screen_MainGame_obj;
    public Text Player1_Score_Text, Player2_Score_Text;
    public GameObject[] ShowSettlementClose_array_obj;//演出結算時，同步關閉物件
    public BaseBallManage baseBallManage_scr;
    public BatterControll BatterControll_scr;//打擊手角色控制
    public PitcherControll PitcherControll_scr;//砲彈手角色控制
    private BarrelControll[] BarrelControll_array_scr;
    [System.Serializable]
    public struct Cam
    {
        public GameObject MainPitcherCam_obj;//主演出攝影機物件(頭手的攝影機)
        public GameObject MainShowShow_obj;//實際演出攝影機
        public Transform[]
            PitcherPosSetup, //砲彈手特寫位置
            BarrelPosSetup;//打擊者特寫位置
    }
    public Cam cam;

    protected override void f_Awake()
    {
        if (baseBallManage_scr == null) { Debug.LogError("baseBallManage_scr 沒有設定!"); }
        if (BatterControll_scr == null) { Debug.LogError("BatterControll_scr 沒有設定!"); }
        if (PitcherControll_scr == null) { Debug.LogError("PitcherControll_scr 沒有設定!"); }

        BarrelControll_array_scr = FindObjectsOfType<BarrelControll>();

        if (GameMaster_koroshi.s_GameMaster != null)
        {
            Round = GameMaster_koroshi.s_GameMaster.data.Round;
        }
    }
    protected override void f_Start()
    {
    }
    protected override void f_Start_StartTimedEvent()
    {
        baseBallManage_scr.gameObject.SetActive(true);
    }

    private bool ShowSettlementEnabled;
    public float ShowSettlementTimed = 2;
    private bool Pause;


    private void FixedUpdate()
    {
        Player1_Score_Text.text = baseBallManage_scr.p1Score.ToString();
        Player2_Score_Text.text = baseBallManage_scr.p2Score.ToString();
    }
    void Update()
    {
        PlayShowOver_Update();

        TestFunction();
        
        //超出回合限制
        if (baseBallManage_scr.Round > Round)
        {
            if (ShowSettlementEnabled == false)
            {
                ShowSettlementEnabled = true;
                GameOver();
            }
        }

        bool _TimeStop = Time.timeScale == 0;
        if (_TimeStop)
        {
            if (Pause) { return; }
            PitcherControll_scr.enabled = !_TimeStop;
            BatterControll_scr.enabled = !_TimeStop;
            for (int i = 0; i < BarrelControll_array_scr.Length; i++)
                BarrelControll_array_scr[i].enabled = !_TimeStop; ;
            Pause = true;
        }
        else
        {
            if (!Pause) { return; }
            PitcherControll_scr.enabled = !_TimeStop;
            BatterControll_scr.enabled = !_TimeStop;
            for (int i = 0; i < BarrelControll_array_scr.Length; i++)
                BarrelControll_array_scr[i].enabled = !_TimeStop;
            Pause = false;
        }
    }

    //得分演出
    //private bool ShowMode = false;
    public enum ShowState
    {
        None,
        Pitcher,//投手
        Barrel,//打擊者
    }
    private ShowState showState;
    private float nowShowWaitTimed = 0;
    private float maxShowWaitTimed = 0;
    private int nowShowPosNumber = -1;//演出位置編號
    Vector3 _showEndPos = Vector3.zero;
    Quaternion _showEndRoat = Quaternion.identity;
    //public float MoveSpeed = 1, RoatSpeed = 1;
    public void PlayShow(bool _isPitcherWin)
    {
        int _camLength = 0;//檢測用
        //是否 投手獲勝
        switch (_isPitcherWin)
        {
            case true:
                _camLength = cam.PitcherPosSetup.Length;
                if (_camLength <= 0) { return; }
                Time.timeScale = SlowTimed_Pitcher;
                showState = ShowState.Pitcher;
                maxShowWaitTimed = maxShowWaitTimed_Pitcher;
                break;
            case false:
                _camLength = cam.BarrelPosSetup.Length;
                if (_camLength <= 0) { return; }
                Time.timeScale = SlowTimed_Barrel;
                showState = ShowState.Barrel;
                maxShowWaitTimed = maxShowWaitTimed_Barrel;
                break;
        }
        if (_camLength <= 0) { return; }
        Debug.LogWarning("得分演出" + "\n" + _isPitcherWin);
        cam.MainPitcherCam_obj.SetActive(false);//關閉投手演出
        nowShowPosNumber = -1;//演出位置編號
        cam.MainShowShow_obj.SetActive(true);//開啟演出攝影機
        nowShowWaitTimed = maxShowWaitTimed;//顯示第一筆位置
    }
    //等待演出結束
    public float SlowTimed_Pitcher = 0.25f, SlowTimed_Barrel = 0.04f;
    public float maxShowWaitTimed_Pitcher = 80, maxShowWaitTimed_Barrel = 15;
    void PlayShowOver_Update()//演出結束
    {
        if (showState == ShowState.None) { return; }
       
        if (nowShowWaitTimed < maxShowWaitTimed)//放在Update不受Time.Scale影響
        {
            nowShowWaitTimed++;
            return;
        }

        //演出結束
        System.Action _showOver = () =>
        {
            Time.timeScale = 1f;
            cam.MainPitcherCam_obj.SetActive(true);//演到一半的~投手演出重新打開
            cam.MainShowShow_obj.SetActive(false);//主演出關閉
            showState = ShowState.None;
            Debug.Log("演出結束");
        };

        switch (showState)
        {
            //砲手
            case ShowState.Pitcher:
                if (nowShowPosNumber < cam.PitcherPosSetup.Length-1)
                {
                    nowShowPosNumber++;
                    _showEndPos = cam.PitcherPosSetup[nowShowPosNumber].position;
                    _showEndRoat = cam.PitcherPosSetup[nowShowPosNumber].rotation;
                    break;
                }
                else
                {
                    _showOver();
                    return;
                }
            //打擊者
            case ShowState.Barrel:
                if (nowShowPosNumber < cam.BarrelPosSetup.Length-1)
                {
                    nowShowPosNumber++;
                    _showEndPos = cam.BarrelPosSetup[nowShowPosNumber].position;
                    _showEndRoat = cam.BarrelPosSetup[nowShowPosNumber].rotation;
                    break;
                }
                else
                {
                    _showOver();
                    return;
                }
        }
        cam.MainShowShow_obj.transform.position = _showEndPos;
        cam.MainShowShow_obj.transform.rotation = _showEndRoat;
        nowShowWaitTimed = 0;//重置時間
    }

    //遊戲結束
    public void GameOver()
    {
        StartCoroutine(IE_GameOver());
    }
    IEnumerator IE_GameOver() 
    {
        yield return new WaitForSeconds(ShowSettlementTimed);
        Debug.Log("遊戲結束");
        if (ShowSettlementClose_array_obj != null)
        {
            for (int i = 0; i < ShowSettlementClose_array_obj.Length; i++)
                ShowSettlementClose_array_obj[i].SetActive(false);
        }
        baseBallManage_scr.gameObject.SetActive(false);
        Common_scr.ShowSettlement(baseBallManage_scr.p1Score, baseBallManage_scr.p2Score, ShowSettlementTimed);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    //測試功能
    void TestFunction()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Round = 1;
            Debug.Log("測試功能-Round 1結束");
        }
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    Debug.Log("測試功能-1P Win畫面");
        //}
        //else if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    Debug.Log("測試功能-2P Win畫面");
        //}
#endif
    }
}
