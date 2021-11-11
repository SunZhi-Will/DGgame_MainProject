using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//跑酷資料
//-如果要跨場景可以使用static不用PlayerPrefs，這通常是用在存檔的，但是他會存在電腦系統的登陸器，更換電腦存檔就沒了
//-玩家腳本可以不用特地用兩個(PlayerAI,Player2AI)，統一控制器就好(可以參考劍道的玩家控制器playerAI)
//-另外移動、旋轉不要放在Update，另外打一個FixedUpdate，放在裡面(有改良過的可以去看)，因為放在Update撞到牆壁會一直抖動
//-控制的部分左右盡量別用斜前走，如果撞到障礙物玩家想走開就會卡住，往後走就會被Boss抓到
//-結算畫面別放在Boss身上，會很難找，可以利用OnTrigger觸發後回傳給遊戲管理腳本控管(參考腳本：ColliderReturn_Tool)
public class GameData_Parkour : GameData_system
{
    public static GameData_Parkour s_GameData { get { return gameData; } }
    private static GameData_Parkour gameData;

    public UnityEngine.UI.Text 
        player1State_Text, player1StateShadow_Text, 
        player2State_Text, player2StateShadow_Text;//玩家存活訊息顯示與影子(影子拿來淡出入用)

    private CharacterChange characterChange_scr;//角色切換物件
    public Transform LevelTotal_tra;//關卡總物件
    [SerializeField]
    private int LevelNumber = -1;//目前關卡-用公開是為了測試
    private GameObject[] Level_tra_array;//所有關卡場景

    public CountDown countDown_scr;
    private BossAI bossAI_scr;
    private PlayerAI player1AI_scr;
    private Player2AI player2AI_scr;
    [SerializeField]
    private PullBoxAI pullBoxAI_scr;
    [SerializeField]
    private PullBox2 pullBox2_scr;
    private ColliderReturn_Tool[] colliderReturnTool_array_scr;//p1,p2觸發物件
    public FadeInOut_Tool Player_1_FadeInOut_scr, Player_2_FadeInOut_scr;

    public int PlayerNumber = 2;//玩家人數

    [System.Serializable]
    public struct CameraSystem 
    {
        public float LerpSpeed;
        internal Transform main_tra;
        public Transform
            Boss_tra,
            p1_tra,
            p2_tra;
        internal GameObject
            p1_obj,
            p2_obj;
        public Vector3 OffectPos;//攝影機偏移值
    }
    public CameraSystem cam;
    private bool pause = true;

    [System.Serializable]
    public class Data
    {
        public float Slow = 1;//減速速度
        public float Fast = 0.5f;//藥水速度
        public float Original_InitialSpeedSpeed = 4;//角色原始基礎速度
        public float SpecialSpeed = 0.3f;//快被Boss追到時加速
        public float LerpSpeed = 0.2f;
        public float CoverBackTimed = 2;//致盲時間

    }
    public Data data = new Data();

    protected override void f_Awake()
    {
        
        gameData = this;
        countDown_scr.enabled = false;

        //所有關卡場景
        int _maxCount = LevelTotal_tra.childCount;
        Level_tra_array = new GameObject[_maxCount];
        for (int i = 0; i < _maxCount; i++)
        {
            GameObject _level_obj = LevelTotal_tra.GetChild(i).gameObject;
            _level_obj.SetActive(false);
            Level_tra_array[i] = _level_obj;
        }
        int _nowSceneNumber = (LevelNumber > -1) ? LevelNumber : Random.Range(0, Level_tra_array.Length);//隨機或者指定
        LevelTotal_tra.gameObject.SetActive(true);
        Level_tra_array[_nowSceneNumber].SetActive(true);

        characterChange_scr = GetComponent<CharacterChange>();
       
    }
    protected override void f_Start() 
    {

}

    protected override void f_Start_StartTimedEvent()
    {
        //設定推人目標
        pullBoxAI_scr.Player1 = characterChange_scr.p1_ch_obj;
        pullBoxAI_scr.Player2 = characterChange_scr.p2_ch_obj;
        pullBox2_scr.Player1 = characterChange_scr.p1_ch_obj;
        pullBox2_scr.Player2 = characterChange_scr.p2_ch_obj;

        bossAI_scr = cam.Boss_tra.GetComponentInChildren<BossAI>();//擷取BOSS控制物件
        player1AI_scr = characterChange_scr.p1_ch_obj.GetComponent<PlayerAI>();
        player2AI_scr = characterChange_scr.p2_ch_obj.GetComponent<Player2AI>();
        colliderReturnTool_array_scr = new ColliderReturn_Tool[2];
        colliderReturnTool_array_scr[0] = player1AI_scr.GetComponentInChildren<ColliderReturn_Tool>();
        colliderReturnTool_array_scr[1] = player1AI_scr.GetComponentInChildren<ColliderReturn_Tool>();


        //player1AI_scr = FindObjectOfType<PlayerAI>();
        //player2AI_scr = FindObjectOfType<Player2AI>();
        if (bossAI_scr == null) { Debug.LogError(gameObject.name + "\n" + "Boss 腳本不存在"); }
        if (player1AI_scr == null) { Debug.LogError(gameObject.name + "\n" + "player1AI_scr 腳本不存在"); }
        if (player2AI_scr == null) { Debug.LogError(gameObject.name + "\n" + "player2AI_scr 腳本不存在"); }

        cam.p1_tra = player1AI_scr.transform;
        cam.p2_tra = player2AI_scr.transform;

        cam.p1_obj = cam.p1_tra.gameObject;
        cam.p2_obj = cam.p2_tra.gameObject;

        cam.main_tra = Camera.main.transform;
        if (cam.main_tra == null) { Debug.LogError("不存在主攝影機!"); }

        countDown_scr.enabled = true;
    }

    void Update()
    {
        if (cam.main_tra == null) { return; }

        //追蹤攝影機
        CameraTrack();

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

    //遊戲結束
    //關閉玩家控制
    public void ClosePlayerControl()//搜索對象
    {
        player1AI_scr.enabled = false;
        player2AI_scr.enabled = false;
        Debug.Log("關閉所有玩家控制器");
    }
    public void GameOver(bool _win) //正式用
    {
        player1AI_scr.enabled = false;
        player2AI_scr.enabled = false;
        bossAI_scr.enabled = false;//遊戲結束
        Common_scr.ShowSettlement(_win);
        Debug.Log("遊戲結束");
    }

    //呼叫結算演出
    public void ShowSettlement(bool _win)
    {
        Common_scr.ShowSettlement(_win);
    }

    #region 攝影機追蹤 CameraTrack
    void CameraTrack() 
    {
        float _distance_p1AndBoss = Vector3.Distance(cam.p1_tra.localPosition, cam.Boss_tra.localPosition);
        float _distance_p2AndBoss = Vector3.Distance(cam.p2_tra.localPosition, cam.Boss_tra.localPosition);

        Vector3 _camNewPos = Vector3.zero;
        Vector3 _targetPos = cam.main_tra.localPosition;
        string _message = "Dead";
        //雙方皆死亡
        if (!cam.p1_obj.activeInHierarchy && !cam.p2_obj.activeInHierarchy)
        {
            player1State_Text.text = _message;
            player1StateShadow_Text.text = _message;
            player2State_Text.text = _message;
            player2StateShadow_Text.text = _message;
            //playernum_Text.text = "Dead\nDead";

            Player_1_FadeInOut_scr.FadeInOutState(FadeInOut_Tool.State.fadeIn);
            Player_2_FadeInOut_scr.FadeInOutState(FadeInOut_Tool.State.fadeIn);
            BossStop();
        }
        else if (cam.p1_obj.activeInHierarchy == false)
        {
            player1State_Text.text = _message;
            player1StateShadow_Text.text = _message;
            //playernum_Text.text = "Dead\nPlayer2";

            _targetPos = cam.p2_tra.localPosition + cam.OffectPos;
            Player_1_FadeInOut_scr.FadeInOutState(FadeInOut_Tool.State.fadeIn);
            Debug.Log("1P已死");
        }
        //如果p2死亡
        else if (cam.p2_obj.activeInHierarchy == false)
        {
            player2State_Text.text = _message;
            player2StateShadow_Text.text = _message;
            //playernum_Text.text = "Player1\nDead";

            _targetPos = cam.p1_tra.localPosition + cam.OffectPos;
            Player_2_FadeInOut_scr.FadeInOutState(FadeInOut_Tool.State.fadeIn);
            Debug.Log("2P已死");
        }
        //兩者都存在抓最遠的那個
        //else (cam.p1_obj.activeInHierarchy && cam.p2_obj.activeInHierarchy)
        else
        {
            ////1P離Boss比較遠的情況
            //if (_distance_p1AndBoss > _distance_p2AndBoss)
            //{
            //    _targetPos = cam.p1_tra.localPosition + cam.OffectPos;
            //}
            ////2P離Boss比較遠的情況
            //else if (_distance_p2AndBoss > _distance_p1AndBoss)
            //{
            //    _targetPos = cam.p2_tra.localPosition + cam.OffectPos;
            //}
            _targetPos = (cam.p1_tra.localPosition + cam.p2_tra.localPosition) / 2 + cam.OffectPos;//抓兩者之間
            //檢測玩家死亡
            for (int i = 0; i < colliderReturnTool_array_scr.Length; i++)
            {
                if (colliderReturnTool_array_scr[i].nowTargetTag == "Boss")//拿Boss當底下死亡判定框，省去新增Tag
                {
                    if (i == 0) { cam.p1_obj.SetActive(false); }
                    else if (i == 1) { cam.p2_obj.SetActive(false); }
                }
            }
        }

        //Vector3 _offectPos = new Vector3(cam.p1_tra.x, OffectPos.y, OffectPos.z + _targetPos.z);

        Vector3 _camPos = cam.main_tra.localPosition;
        if (Vector3.Distance(_camPos, _targetPos) > test_CamPosSpeed)
            _camNewPos = Vector3.Lerp(cam.main_tra.localPosition, _targetPos, cam.LerpSpeed * 2);//太遠的話加速
        else
            _camNewPos = Vector3.Lerp(cam.main_tra.localPosition, _targetPos, cam.LerpSpeed);

        cam.main_tra.localPosition = _camNewPos;
    }
    public float test_CamPosSpeed = 0.5f;
    #endregion 攝影機追蹤

    public float DelayBossStopTimed = 2.4f;
    private bool DelayBossStop_enabled;
    void BossStop() 
    {
        if (DelayBossStop_enabled) { return; }
        DelayBossStop_enabled = true;
        StartCoroutine(IE_BossStop());
    }
    IEnumerator IE_BossStop() 
    {
        GameOver(false);//雙方玩家皆死亡
        yield return new WaitForSeconds(DelayBossStopTimed);
        bossAI_scr.enabled = false;//讓Boss再爬一會在結束
        Debug.Log("GameOver");
    }

    public FadeInOut_Tool CoverBack_fadeInOut_scr;
    //致盲
    public void CoverBack()
    {
        StartCoroutine(IE_CoverBack());
    }
    IEnumerator IE_CoverBack()
    {
        CoverBack_fadeInOut_scr.gameObject.SetActive(true);
        CoverBack_fadeInOut_scr.FadeInOutState(FadeInOut_Tool.State.fadeIn);//淡入
        CoverBack_fadeInOut_scr.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));//隨機旋轉
        yield return new WaitForSeconds(data.CoverBackTimed);//維持原始遊戲設定
        CoverBack_fadeInOut_scr.FadeInOutState(FadeInOut_Tool.State.fadeOut);//淡出
        yield return new WaitForSeconds(5);//維持原始遊戲設定
        CoverBack_fadeInOut_scr.gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        gameData = null;
    }

    public float ChaseKillZ = 350;//追殺位置
    #region 測試功能 TestFunction
    //測試功能
    void TestFunction()
    {
#if UNITY_EDITOR
        Vector3 _pos = Vector3.zero;
        float EndposZ = 430;//終點位置Z軸
        //float ChaseKillZ = 350;//追殺位置
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _pos = cam.p1_tra.localPosition;
            cam.p1_tra.localPosition = new Vector3(_pos.x, _pos.y, EndposZ);
            Debug.Log("1P抵達終點前");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            _pos = cam.p2_tra.localPosition;
            cam.p2_tra.localPosition = new Vector3(_pos.x, _pos.y, EndposZ);
            Debug.Log("2P抵達終點前");
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            _pos = cam.p2_tra.localPosition;
            cam.p2_tra.localPosition = new Vector3(_pos.x, _pos.y, EndposZ);
            _pos = cam.p1_tra.localPosition;
            cam.p1_tra.localPosition = new Vector3(_pos.x, _pos.y, EndposZ);
            Debug.Log("雙方同時抵達終點前");
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            _pos = cam.p1_tra.localPosition;
            cam.p1_tra.localPosition = new Vector3(_pos.x, _pos.y, cam.Boss_tra.localPosition.z);
            Debug.Log("1P失敗");
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            _pos = cam.p2_tra.localPosition;
            cam.p2_tra.localPosition = new Vector3(_pos.x, _pos.y, cam.Boss_tra.localPosition.z);
            Debug.Log("2P失敗");
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            _pos = cam.p1_tra.localPosition;
            cam.p1_tra.localPosition = new Vector3(_pos.x, _pos.y, cam.Boss_tra.localPosition.z);
            _pos = cam.p2_tra.localPosition;
            cam.p2_tra.localPosition = new Vector3(_pos.x, _pos.y, cam.Boss_tra.localPosition.z);
            Debug.Log("雙方同時失敗");
        }
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            cam.Boss_tra.gameObject.SetActive(!cam.Boss_tra.gameObject.activeInHierarchy);
            Debug.Log("Boss物件開關");
        }
        else if (Input.GetKeyDown(KeyCode.F8))
        {
            _pos = cam.p2_tra.position;
            cam.p2_tra.position = new Vector3(_pos.x, _pos.y, ChaseKillZ);
            _pos = cam.p1_tra.position;
            cam.p1_tra.position = new Vector3(_pos.x, _pos.y, ChaseKillZ);
            _pos = cam.Boss_tra.position;
            cam.Boss_tra.position = new Vector3(_pos.x, _pos.y, ChaseKillZ - 30);
            cam.main_tra.position = cam.p1_obj.transform.position;
            Debug.Log("開始追殺");
        }
#endif
    }
    #endregion 測試功能
}
