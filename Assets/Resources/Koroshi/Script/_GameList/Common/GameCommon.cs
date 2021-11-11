using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//Koroshi
//每款遊戲通用流程
//暫停、結算
public class GameCommon : MonoBehaviour
{
    [Header("結算選單出現是否關閉結算演出")]
    public bool MenuOpen_CloseSettlementShow = false;
    public FadeInOut_Tool black_FadeInOut_scr;

    private int nowSelectNumber;//目前選擇

    private Change change_scr;
    public enum PauseSelect { Continues, Restart, GiveUp }
    public enum SettlementSelect { Again, BackTitle }

    public enum State
    {
        None,
        Pause,
        SettlementShow,//結算畫面演出
        SettlementMenuSelect,//結算畫面選擇
        ExitGame,//離開遊戲(返回標題or重新開始)
    }
    private State state;

    [Header("目前選項")]
    public Transform NowSelect_tra;

    //結算畫面開啟中
    public bool SettlementEnabled
    {
        get
        {
            return settlement.MainScreen_obj.activeInHierarchy ||
                settlement.pkMode.WinLose_obj.gameObject.activeInHierarchy || settlement.pkMode.Tie_obj.gameObject.activeInHierarchy;
        }
    }

    #region 結算畫面
    [System.Serializable]
    public struct Settlement
    {
        //結算畫面
        internal GameObject MainScreen_obj;//結算主畫面
        public GameObject Menu_obj;//結算選項UI
        [Header("結算演出時 開啟物件")]
        public GameObject[] array_obj;//主要共通開啟複數物件
        [Header("結算演出時 關閉物件")]
        public GameObject[] array_close_obj;//主要共通開啟複數物件

        [Header("顯示模型位置")]
        public Transform Win_Model_tra;

        //Pk模式用結算
        [System.Serializable]
        public struct PkMode
        {
            public FadeInOut_Tool
                WinLose_obj,          
                Tie_obj;//平手

            //玩家頭像顯示
            public Image Win_PlayerHead_image,
                Lose_PlayerHead_image;

            //玩家名稱顯示
            public Text
                Win_PlayerName_Text,
                Lose_PlayerName_Text;

            //得分
            public Text
                Win_Score_Text, 
                Lose_Score_Text;

            public Text[] Tie_PlayerName_Text;//平手
            public Text[] Tie_Score_Text;//平手
        }
        public PkMode pkMode;

        //合作模式用結算
        [System.Serializable]
        public struct Cooperation
        {
            public FadeInOut_Tool
                GameClear_scr,//過關畫面
                GameOver_scr;//失敗畫面
        }
        public Cooperation cooperation;
    }
    public Settlement settlement;
    #endregion 結算畫面

    //按鍵提示
    public GameObject KeyTip;

    [SerializeField]
    private GameObject Menu_Pause_obj;//暫停UI
    [SerializeField][Header("暫停時同步關閉物件")]
    private GameObject[] PauseClose_array_obj;//暫停時關閉物件

    //暫停
    [SerializeField]
    private Button
        Continues_btn,//繼續遊戲
        Restart_btn,//重新開始遊戲
        GiveUp_btn;//放棄遊戲，返回標題

    public Action
        Continues_event,//繼續遊戲
        AgainRestart_event,//重新開始遊戲
        GiveUpBackTitle_event;//放棄返回標題

    //結算
    [SerializeField]
    private Button
        Again_btn,//再次遊玩當前遊戲
        BackTitle_btn;//返回標題

    private void Awake()
    {
        change_scr = FindObjectOfType<Change>();
        if (change_scr == null) { change_scr = gameObject.AddComponent<Change>(); }
        Set_GiveUpAndBackTitle(() =>
        {
            //暫停、結算 -> 放棄遊戲返回標題
            change_scr.ChangeScene(GameMaster_koroshi.s_GameMaster.TitleSceneName);
            Debug.Log("放棄遊戲，返回標題");

        });//放棄遊戲返回標題
        Set_Continues(() => 
        {
            PauseContinuesGame(false);
            Debug.Log("繼續遊戲");
        });//繼續遊戲

        //取得主畫面物件(單純懶得多放東西)
        settlement.MainScreen_obj = settlement.Menu_obj.transform.parent.gameObject;
    }
    private float nowWaitSettlementControlTime;//等待結算控制時間，不然太快按到
    private float maxWaitSettlementControlTimed = 3;//等待結算控制時間，不然太快按到
    private void Update()
    {
        if (CheckState(State.ExitGame)) { return; }//封鎖所有控制
        //暫停或繼續
        if (PlayerControl_koroshi.Escape_keyDown && settlement.MainScreen_obj.activeInHierarchy == false)
        {
            PauseContinuesGame(Time.timeScale == 1);//砸瓦魯多!!!
        }

        switch (state)
        {
            case State.SettlementShow:
                if (nowWaitSettlementControlTime < maxWaitSettlementControlTimed)
                {
                    nowWaitSettlementControlTime += Time.deltaTime;
                    return;
                }
                if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Confirm) ||
                    PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Cancel) ||
                    PlayerControl_koroshi.Escape_keyDown)
                {
                    OpenSettlement_Menu();
                    Debug.Log("前往結算選項");
                }
                break;
            default:
                if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Up)) { ChangeSelect(-1); }
                else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Down)) { ChangeSelect(+1); }
                else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Confirm)) { ConfirmSelect(); }
                break;
        }
    }
    #region 切換選擇 ChangeSelect
    //切換選擇
    void ChangeSelect(int _number)
    {
        //Debug.Log("目前狀態:"+ state + "\n"+"進行選擇:" + _number);
        switch (state)
        {
            case State.Pause:
                if (_number > 0)
                {
                    if (nowSelectNumber < (int)PauseSelect.GiveUp) { nowSelectNumber += _number; }
                }
                else if (_number < 0)
                {
                    if (nowSelectNumber > 0) { nowSelectNumber += _number; }
                }

                if (nowSelectNumber == (int)PauseSelect.Continues) { NowSelect_tra.position = Continues_btn.transform.position; }
                else if (nowSelectNumber == (int)PauseSelect.Restart) { NowSelect_tra.position = Restart_btn.transform.position; }
                else if (nowSelectNumber == (int)PauseSelect.GiveUp) { NowSelect_tra.position = GiveUp_btn.transform.position; }
                break;
            case State.SettlementMenuSelect:
                if (_number > 0)
                {
                    if (nowSelectNumber < (int)SettlementSelect.BackTitle) { nowSelectNumber += _number; }
                }
                else if (_number < 0)
                {
                    if (nowSelectNumber > 0) { nowSelectNumber += _number; }
                }

                if (nowSelectNumber == (int)SettlementSelect.Again) { NowSelect_tra.position = Again_btn.transform.position; }
                else if (nowSelectNumber == (int)SettlementSelect.BackTitle) { NowSelect_tra.position = BackTitle_btn.transform.position; }
                break;
        }
    }
    #endregion 切換選擇

    #region 確認選擇 ConfirmSelect
    //確認選擇
    void ConfirmSelect()
    {
        switch (state)
        {
            case State.Pause:
                if (nowSelectNumber == (int)PauseSelect.Continues) { Continues_event(); }
                else if (nowSelectNumber == (int)PauseSelect.Restart) { AgainRestart_event(); }
                else if (nowSelectNumber == (int)PauseSelect.GiveUp) { GiveUpBackTitle_event(); }
                break;
            case State.SettlementMenuSelect:
                if (nowSelectNumber == (int)SettlementSelect.Again) { AgainRestart_event(); }
                else if (nowSelectNumber == (int)SettlementSelect.BackTitle) { GiveUpBackTitle_event(); }
                break;
        }
    }
    #endregion 確認選擇



    #region 結算演出設定 ShowSettlement
    //結算共通開啟物件
    void Settlement_CommonOpen()
    {
        ChangeState(State.SettlementShow);
        KeyTip.SetActive(true);//按鍵提示
        if (settlement.array_obj != null)
        {
            //顯示結算共通物件
            for (int i = 0; i < settlement.array_obj.Length; i++)
                settlement.array_obj[i].SetActive(true);
        }
        //同步關閉
        if (settlement.array_close_obj != null)
        {
            for (int i = 0; i < settlement.array_close_obj.Length; i++)
                settlement.array_close_obj[i].SetActive(false);
        }
        settlement.MainScreen_obj.SetActive(true);//結算總物件
    }
    //合作模式結算
    public void ShowSettlement(bool Win)
    {
        Settlement_CommonOpen();
        //---
        Cooperation_Mode(Win);
    }
    //Pk模式結算
    public void ShowSettlement(int p1_Score, int p2_Score, float _delayShowTimed=0)
    {
        //Pk結算只會顯示一名玩家
        if (settlement.pkMode.WinLose_obj.gameObject.activeInHierarchy ||
            settlement.pkMode.Tie_obj.gameObject.activeInHierarchy)
        {
            return;
        }
        System.Action _action = () =>
          {
              Settlement_CommonOpen();
            //---
            Settlement_Pk_Mode(p1_Score, p2_Score);
          };
        if (_delayShowTimed == 0)
        {
            if (_action != null)
                _action();
        }
        else 
        {
            StartCoroutine(IE_ShowSettlement(_delayShowTimed, _action));
        }
    }
    IEnumerator IE_ShowSettlement(float _waitTimed,System.Action _action) 
    {
        yield return new WaitForSeconds(_waitTimed);
        if (_action != null) 
            _action();
    }

    [Header("生成測試用模型")]
    public GameObject Test_Win_Model;
    //生成獲勝角色
    void SpawModel(PlayerNumber _playerNumber) 
    {
        if (settlement.Win_Model_tra == null) { Debug.LogError("沒有設定獲勝模型位置!"); }
        GameObject _obj = null;
        GameMaster_koroshi _gameData_scr = GameMaster_koroshi.s_GameMaster;
        Transform _tra = null;
        if (_gameData_scr == null)
        {
            _obj = Test_Win_Model;
            _tra = Instantiate(_obj).transform;
        }
        else 
        {
            _tra = _gameData_scr.NowSelectCharacter_Spawn(_playerNumber).transform;
        }
        //
        _tra.SetParent(settlement.Win_Model_tra);
        _tra.localPosition = Vector3.zero;
        _tra.localRotation = Quaternion.identity;
        Vector3 _scale = _tra.parent.localScale;
        _tra.localScale = new Vector3(_scale.x, _scale.y, _scale.z);
    }
    #region 結算畫面-PK模式 Settlement_Pk_Mode
    void Settlement_Pk_Mode(int p1_Score, int p2_Score)
    {
        GameMaster_koroshi _gameData = GameMaster_koroshi.s_GameMaster;
        //得分顯示
        if (p1_Score > p2_Score)
        {
            settlement.pkMode.Win_PlayerHead_image.sprite = (_gameData != null) ? _gameData.p1_Head : null;//設定頭像
            settlement.pkMode.Win_PlayerName_Text.text = "Player1";
            settlement.pkMode.Win_Score_Text.text = p1_Score.ToString();

            settlement.pkMode.Lose_PlayerHead_image.sprite = (_gameData != null) ? _gameData.p2_Head : null;//設定頭像
            settlement.pkMode.Lose_PlayerName_Text.text = "Player2";
            settlement.pkMode.Lose_Score_Text.text = p2_Score.ToString();
            SpawModel(PlayerNumber.p1);
        }
        else if (p1_Score < p2_Score)
        {
            settlement.pkMode.Win_PlayerHead_image.sprite = (_gameData != null) ? _gameData.p2_Head : null;//設定頭像
            settlement.pkMode.Win_PlayerName_Text.text = "Player2";
            settlement.pkMode.Win_Score_Text.text = p2_Score.ToString();

            settlement.pkMode.Lose_PlayerHead_image.sprite = (_gameData != null) ? _gameData.p1_Head : null;//設定頭像
            settlement.pkMode.Lose_PlayerName_Text.text = "Player1";
            settlement.pkMode.Lose_Score_Text.text = p1_Score.ToString();
            SpawModel(PlayerNumber.p2);
        }
        //平手
        else
        {
            settlement.pkMode.Tie_PlayerName_Text[0].text = "Player1";
            settlement.pkMode.Tie_PlayerName_Text[1].text = "Player2";
            settlement.pkMode.Tie_Score_Text[0].text = p1_Score.ToString();
            settlement.pkMode.Tie_Score_Text[1].text = p2_Score.ToString();
        }
        //是否平手，開啟對應顯示
        switch (p1_Score == p2_Score)
        {
            case false:
                settlement.pkMode.Tie_obj.gameObject.SetActive(false);
                settlement.pkMode.WinLose_obj.gameObject.SetActive(true);
                settlement.pkMode.WinLose_obj.FadeInOutState(FadeInOut_Tool.State.fadeIn);
                break;
            case true:
                settlement.pkMode.WinLose_obj.gameObject.SetActive(false);
                settlement.pkMode.Tie_obj.gameObject.SetActive(true);
                settlement.pkMode.Tie_obj.FadeInOutState(FadeInOut_Tool.State.fadeIn);
                break;
        }
    }
    #endregion 結算畫面-PK模式

    #region 結算畫面-合作模式 Cooperation_Mode
    void Cooperation_Mode(bool Win)
    {
        switch (Win)
        {
            case true:
                settlement.cooperation.GameClear_scr.gameObject.SetActive(true);
                settlement.cooperation.GameClear_scr.FadeInOutState(FadeInOut_Tool.State.fadeIn);
                break;
            case false:
                settlement.cooperation.GameOver_scr.gameObject.SetActive(true);
                settlement.cooperation.GameOver_scr.FadeInOutState(FadeInOut_Tool.State.fadeIn);
                break;
        }
    }
    #endregion 結算畫面-PK模式

    #endregion 結算演出設定

    #region 開關結算選單 OpenSettlement_Menu
    /// <summary>
    /// 開關結算畫面
    /// </summary>
    /// <param name="Tie">平手</param>
    public void OpenSettlement_Menu()
    {
        //是否關閉結算演出的物件，只出現選單
        if (MenuOpen_CloseSettlementShow) 
        {
            //共通物件
            if (settlement.array_obj != null)
            {
                //顯示結算共通物件
                for (int i = 0; i < settlement.array_obj.Length; i++)
                    settlement.array_obj[i].SetActive(true);
            }
            //Pk模式物件
            settlement.pkMode.WinLose_obj.gameObject.SetActive(false);
            settlement.pkMode.Tie_obj.gameObject.SetActive(false);
            //合作模式物件
            settlement.cooperation.GameClear_scr.gameObject.SetActive(false);
            settlement.cooperation.GameOver_scr.gameObject.SetActive(false);
        }

        ChangeState(State.SettlementMenuSelect);
        //選項設定
        NowSelect_tra.SetParent(Again_btn.transform.parent);
        NowSelect_tra.SetSiblingIndex(1);
        NowSelect_tra.gameObject.SetActive(true);

        ChangeSelect(nowSelectNumber = 0);//選項切換

        settlement.Menu_obj.SetActive(true);
    }
    #endregion 開關結算選單

    #region 繼續遊戲 Set_Continues
    /// <summary>
    /// 繼續遊戲
    /// </summary>
    public void Set_Continues(Action _action)
    {
        Continues_btn.onClick.RemoveAllListeners();
        Continues_btn.onClick.AddListener(() => 
        {
            _action();
        });
        Continues_event = _action;
    }
    //暫停 繼續遊戲
    private float OriginalTimed = 1;
    public void PauseContinuesGame(bool _enabled)
    {
        Debug.Log("遊戲暫停 或 繼續");

        Time.timeScale = (_enabled == false) ? 1 : 0;
        //Time.timeScale = (_enabled == false) ? OriginalTimed : 0;

        if (_enabled)
        {
            ChangeState(State.Pause);
            NowSelect_tra.SetParent(Continues_btn.transform.parent);
            NowSelect_tra.SetSiblingIndex(1);
            NowSelect_tra.gameObject.SetActive(true);
        }
        else
            ChangeState(State.None);

        KeyTip.SetActive(_enabled);
        ChangeSelect(nowSelectNumber = 0);
        Menu_Pause_obj.SetActive(_enabled);
        //暫停時，關閉指定物件
        if (PauseClose_array_obj != null)
        {
            for (int i = 0; i < PauseClose_array_obj.Length; i++)
                PauseClose_array_obj[i].SetActive(!_enabled);
        }

        Debug.Log("暫停畫面:" + _enabled);
    }
    #endregion 繼續遊戲

    #region 重新開始，再次遊戲 Set_AgainAndRestart
    /// <summary>
    /// 重新開始，再次遊戲
    /// </summary>
    public void Set_AgainAndRestart(Action _action)
    {
        Action _changeEvent = () => { ChangBlackScreen(_action); };
        Again_btn.onClick.RemoveAllListeners();//清除事件
        Again_btn.onClick.AddListener(() => 
        {
            _changeEvent();
        });
        Restart_btn.onClick.RemoveAllListeners();//清除事件
        Restart_btn.onClick.AddListener(() => 
        {
            _changeEvent();
        });

        AgainRestart_event = _changeEvent;
    }
    #endregion 重新開始，再次遊戲

    #region 放棄遊戲，返回標題 Set_GiveUpAndBackTitle
    /// <summary>
    /// 放棄遊戲，返回標題
    /// </summary>
    public void Set_GiveUpAndBackTitle(Action _action)
    {
        Action _changeEvent = () => { ChangBlackScreen(_action); };
        BackTitle_btn.onClick.RemoveAllListeners();//清除事件
        BackTitle_btn.onClick.AddListener(() => 
        {
            _changeEvent();
        });
        GiveUp_btn.onClick.RemoveAllListeners();//清除事件
        GiveUp_btn.onClick.AddListener(() =>
        {
            _changeEvent();
        });

        Time.timeScale = 1;
        GiveUpBackTitle_event = _changeEvent;
    }
    #endregion 放棄遊戲，返回標題

    bool CheckState(State _checkState)
    {
        return state == _checkState;
    }
    void ChangeState(State _newState)
    {
        state = _newState;
    }

    #region 黑屏轉換
    void ChangBlackScreen(Action _action)
    {
        ChangeState(State.ExitGame);
        StartCoroutine(IE_ChangBlackScreen(_action));
    }
    IEnumerator IE_ChangBlackScreen(Action _action)
    {
        Time.timeScale = 1;
        black_FadeInOut_scr.FadeInOutState(FadeInOut_Tool.State.fadeIn);
        yield return new WaitForSeconds(1f);
        _action();
        Debug.Log("執行事件");
    }
    #endregion 黑屏轉換
}
