using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Koroshi
//Pk設定選單
public class Screen_PkSetupMenu : MonoBehaviour
{
    public Transform nowSelect_tra;//PK設定選單
    private Vector3 Original;

    [System.Serializable]
    public struct SetupMenu
    {
        public Transform AllObj_tra;//對準選擇或開啟設定用(有些不能設定回合或時間)
        public Text Display_Text;//顯示文字
        public ButtonMoveInOutEvent_Tool Left_btn;
        public ButtonMoveInOutEvent_Tool Right_btn;
    }
    public SetupMenu 
        Setup_Round, //回合數
        Setup_Timed; //時間

    public Text Help_text;//規則說明
    public Image Background_image;//背景替換

    public FadeInOut_Tool BlackScreen_fade_scr;//黑幕淡出入
    public Screen_GameSelect screen_GameSelect;//返回遊戲選擇中

    private GameIntroduction_ScripbObject nowGameData_scrObj;//目前遊戲資訊

    //目前設定回合與時間
    private int nowSetupRound;//目前設定
    private int maxSetupRound;//最大選項
    private int miniSetupRound;//最小選項

    private float nowSetupTimed;//目前設定
    private float maxSetupTimed;//最大選項
    private float miniSetupTimed;//最小選項

    //目前選擇
    public enum State
    {
        None,
        Round,
        Timed,
    }
    public State state;

    private bool StartGameReadyLoading = false;

    //--學生--
    private Change change;
    //--------
    private void Awake()
    {
        change = FindObjectOfType<Change>();
        Original = nowSelect_tra.localPosition;//紀錄初始位置
    }

    private void OnEnable()
    {
        Init();
    }

    //初始化
    void Init()
    {
        if (GameMaster_koroshi.s_GameMaster == null) { Debug.LogError("GameMaster_koroshi.s_GameMaster 沒有設定!"); }
        //Pk設定-取得遊戲資訊
        nowGameData_scrObj = GameMaster_koroshi.s_GameMaster.data.nowSelectGame;

        //顯示規則說明
        Help_text.text = nowGameData_scrObj.GameDetails;
        RectTransform _rectTra = Help_text.rectTransform;
        //Help_text.rectTransform.sizeDelta = new Vector2(_rectTra.sizeDelta.x, Help_text.preferredHeight);//高度設定
        //Help_text.rectTransform.anchoredPosition = Vector2.zero;//位置重置
        //背景替換
        Background_image.sprite = nowGameData_scrObj.MainsSreen;

        //State _initState = State.Round;
        int _nowSetup = 0;//不設定

        //回合設定
        if (nowGameData_scrObj.RoundSetup.enabled)
        {
            Setup_Round.AllObj_tra.gameObject.SetActive(true);
            //預設數值
            nowSetupRound = nowGameData_scrObj.RoundSetup.DefaultValue;
            //設定最小/最大選項
            miniSetupRound = nowGameData_scrObj.RoundSetup.miniValue;
            maxSetupRound = nowGameData_scrObj.RoundSetup.maxValue;
            //顯示內容
            Setup_Round.Display_Text.text = nowSetupRound.ToString();
            _nowSetup++;
            state = State.Round;//初始化選擇
        }
        else
        {
            //預設數值
            nowSetupRound = nowGameData_scrObj.RoundSetup.DefaultValue;
            Setup_Round.AllObj_tra.gameObject.SetActive(false);
        }

        //時間設定
        if (nowGameData_scrObj.TimedSetup.enabled)
        {
            Setup_Timed.AllObj_tra.gameObject.SetActive(true);
            //預設數值
            nowSetupTimed = nowGameData_scrObj.TimedSetup.DefaultValue;
            //設定最小/最大選項
            miniSetupTimed = nowGameData_scrObj.TimedSetup.miniValue;
            maxSetupTimed = nowGameData_scrObj.TimedSetup.maxValue;
            //顯示內容
            Setup_Timed.Display_Text.text = nowSetupTimed.ToString();
            _nowSetup++;
            if (!CheckState(State.Round))
                state = State.Timed;//初始化選擇
        }
        else
        {
            Setup_Timed.AllObj_tra.gameObject.SetActive(false);
        }


        if (CheckState(State.None))
        {
            nowSelect_tra.gameObject.SetActive(false);
        }
        else
        {
            nowSelect_tra.localPosition = Original;//初始化位置
            nowSelect_tra.gameObject.SetActive(true);
        }
        ////不進行任何設定
        //if (_nowSetup <= 0)
        //{
        //    Debug.LogError("錯誤!不進行任何遊戲前準備設定!");
        //}
        //else
        //{
        //    //初始化選項
        //    UpDownChangeSelect(_initState);
        //    Debug.Log("初始化選項");
        //}
    }

    void Start()
    {

    }

    void Update()
    {
        if (StartGameReadyLoading) { return; }

        
        if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Confirm))
        {
            ConfirmSelect();
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Cancel) || PlayerControl_koroshi.Escape_keyDown)
        {
            screen_GameSelect.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        //無任何設定選項
        if (CheckState(State.None)) { return; }
        if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Up))
        {
            UpDownChangeSelect(State.Round);
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Down))
        {
            UpDownChangeSelect(State.Timed);
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Left))
        {
            LeftRightChangeSelect(-1);
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Right))
        {
            LeftRightChangeSelect(+1);
        }
    }

    //確認選擇
    void ConfirmSelect()
    {
        StartCoroutine(StartGame());
        Debug.Log("確認選擇");
    }

    IEnumerator StartGame()
    {
        StartGameReadyLoading = true;//防止重複執行
        BlackScreen_fade_scr.FadeInOutState(FadeInOut_Tool.State.fadeIn);

        GameMaster_koroshi.s_GameMaster.data.Round = nowSetupRound;//確認選項設定
        GameMaster_koroshi.s_GameMaster.data.Timed = nowSetupTimed;//確認選項設定
        yield return new WaitForSeconds(1f);
        change.ChangeScene(nowGameData_scrObj.SceneName);//確認選項轉換場景

    }

    //上下各只有一格
    void UpDownChangeSelect(State _newState)
    {
        Vector3 _pos = Vector3.zero;
        switch (_newState)
        {
            case State.Round:
                if (nowGameData_scrObj.RoundSetup.enabled == false) { return; }
                _pos = nowSelect_tra.position;
                nowSelect_tra.position = new Vector3(_pos.x, Setup_Round.AllObj_tra.position.y, _pos.z);
                AudioManager.s_AudioManager.PlayAudio_Move();
                break;
            case State.Timed:
                if (nowGameData_scrObj.TimedSetup.enabled == false) { return; }
                _pos = nowSelect_tra.position;
                nowSelect_tra.position = new Vector3(_pos.x, Setup_Timed.AllObj_tra.position.y, _pos.z);
                AudioManager.s_AudioManager.PlayAudio_Move();
                break;
        }
        state = _newState;
    }

    //左右切換選項
    void LeftRightChangeSelect(int _updateNumber)
    {
        switch (state)
        {
            case State.Round:
                int _updateInt = 0;
                _updateInt = nowSetupRound + (nowGameData_scrObj.RoundSetup.IntervalValue * _updateNumber);//預測更新值
                Debug.Log("Number:" + _updateNumber + "\n" + "Update:" + _updateInt + "\n" + "Mini:" + miniSetupRound +" / "+ "Max:" + maxSetupRound);
                if (_updateNumber < 0)
                {
                    if (_updateInt >= miniSetupRound) { nowSetupRound = _updateInt; }//大於最小值才往前
                }
                else if (_updateNumber > 0)
                {
                    if (_updateInt <= maxSetupRound) { nowSetupRound = _updateInt; }//小於最大值才往後
                }
                Setup_Round.Display_Text.text = nowSetupRound.ToString();
                AudioManager.s_AudioManager.PlayAudio_Click();
                break;
            case State.Timed:
                float _updateFloat = 0;
                _updateFloat = nowSetupTimed + (nowGameData_scrObj.TimedSetup.IntervalValue * _updateNumber);//預測更新值
                Debug.Log("Number:" + _updateNumber + "\n" + "Update:" + _updateFloat + "\n" + "Mini:" + miniSetupTimed + " / " + "Max:" + maxSetupTimed);
                if (_updateNumber < 0)
                {
                    if (_updateFloat >= miniSetupTimed) { nowSetupTimed = _updateFloat; }//大於最小值才往前
                }
                else if (_updateNumber > 0)
                {
                    if (_updateFloat <= maxSetupTimed) { nowSetupTimed = _updateFloat; }//小於最大值才往後
                }
                Setup_Timed.Display_Text.text = nowSetupTimed.ToString();
                AudioManager.s_AudioManager.PlayAudio_Click();
                break;
        }
    }

    private bool CheckState(State _newState)
    {
        return state == _newState;
    }

    private void OnDisable()
    {
        state = State.None;
    }
}
