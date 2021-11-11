using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//Koroshi
//選項設定內容
public class Screen_Option : MonoBehaviour
{
    private GameObject nowOpenScreen_obj;
    private bool FullMode = true;//全螢幕模式
    [System.Serializable]
    public struct SetupOption
    {
        public Button preBtn,nextBtn;
        private Color preBtn_OriginalColor, nextBtn_OriginalColor;//原始顏色
        public Text DisplayValueText;//顯示數值
        private Color DisplayValueText_OriginalColor;//原始顏色
        //紀錄初始顏色
        internal void Init()
        {
            preBtn_OriginalColor = preBtn.image.color;
            nextBtn_OriginalColor = nextBtn.image.color;
            DisplayValueText_OriginalColor = DisplayValueText.color;
        }
        //切換顏色
        public void ChangeColor(bool _enabled)
        {
            if (_enabled)
            {
                preBtn.image.color = Color.white;
                nextBtn.image.color = Color.white;
                DisplayValueText.color = Color.white;
            }
            else
            {
                preBtn.image.color = preBtn_OriginalColor;
                nextBtn.image.color = nextBtn_OriginalColor;
                DisplayValueText.color = DisplayValueText_OriginalColor;
            }
        }
    }
    public SetupOption
        Setup_Audio,
        Setup_FullWindown;
    
    [SerializeField]
    private AudioMixer Game_AudioMixer;
    private GameMaster_koroshi _gameData;

    private int nowSelectNumber = 0;//目前選擇項目
    public Transform NowSelectFrame_tra;//目前選擇顯示框
    public Transform ButtonTotal_tra;//按鈕總物件
    private enum State { Audio, FullWindown }
    private State state;
    private ButtonMoveInOutEvent_Tool[] MenuBtn_array;

    public AudioClip MoveAudio, ClickAudio;

    //目前設定參數
    private float 
        nowAudioValue = 5, 
        miniAudioValue = 0, 
        maxAudioValue = 10;
    private float nowMiniValue = -5;//預設音量最小值
    //[SerializeField]
    //private float nowAudioMedian = 5;//中間值
    //[SerializeField]
    //private float SeveralMultiples = 10;//音量修改倍數
    private bool nowFullWindown = true;

    private void Awake()
    {
        _gameData = GameMaster_koroshi.s_GameMaster;
        int _maxCount = ButtonTotal_tra.childCount;
        MenuBtn_array = new ButtonMoveInOutEvent_Tool[_maxCount];
        for (int i = 0; i < _maxCount; i++)
        {
            MenuBtn_array[i] = ButtonTotal_tra.GetChild(i).GetComponent<ButtonMoveInOutEvent_Tool>();
            if (MenuBtn_array[i] == null) { Debug.LogError(gameObject.name + "\n" + MenuBtn_array[i].name + " 沒有按鈕組件!"); }
        }

        //全螢幕與視窗化
        nowAudioValue = _gameData.data.AudioValue;
        Setup_Audio.preBtn.onClick.AddListener(() => { AudioValue(-1); });
        Setup_Audio.nextBtn.onClick.AddListener(() => { AudioValue(+1); });
        AudioValue(0);//初始化

        //全螢幕與視窗化
        nowFullWindown = Screen.fullScreen = _gameData.data.FullOrWindowns;
        //System.Action _action = () => { Screen.fullScreen = !Screen.fullScreen; };//全螢幕與視窗化切換 事件
        Setup_FullWindown.preBtn.onClick.AddListener(() => { FullWindownChange(); });
        Setup_FullWindown.nextBtn.onClick.AddListener(() => { FullWindownChange(); });
        DisplayState_FullWindownChange(_gameData.data.FullOrWindowns);//初始化

        //初始化內容設定
        Setup_Audio.Init();
        Setup_FullWindown.Init();
        NowSelectUpdate();//初始化選項設定
    }

    public void OpenScreen(GameObject _OpenObj)
    {
        gameObject.SetActive(true);
        nowOpenScreen_obj = _OpenObj;
    }

    private void OnEnable()
    {
        ChangeSelect(0);
    }

    void Update()
    {
        if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Up) || PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Up))
        {
            AudioManager.s_AudioManager.PlayAudio_Move();
            ChangeSelect(-1);
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Down) || PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Down))
        {
            AudioManager.s_AudioManager.PlayAudio_Move();
            ChangeSelect(+1);
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Left) || PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Left))
        {
            OptionSetupChange(-1);
            AudioManager.s_AudioManager.PlayAudio_Click();
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Right) || PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Right))
        {
            OptionSetupChange(+1);
            AudioManager.s_AudioManager.PlayAudio_Click();
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Cancel) || PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Cancel) || PlayerControl_koroshi.Escape_keyDown)
        {
            gameObject.SetActive(false);
        }
    }
    //切換選項
    void ChangeSelect(int _updateNmber)
    {
        int _predictionUpdate = nowSelectNumber + _updateNmber;//更新預測，檢測是否超出陣列
        if (_updateNmber > 0)
        {
            if (_predictionUpdate >= ButtonTotal_tra.childCount) { return; }//超出列表最大數
        }
        else if (_updateNmber < 0)
        {
            if (_predictionUpdate < 0) { return; }//超出列表最大數
        }
        nowSelectNumber = _predictionUpdate;//同步更新
        NowSelectUpdate();
    }
    //目前選向位置更新
    void NowSelectUpdate()
    {
        if (nowSelectNumber == (int)State.Audio)
        {
            NowSelectFrame_tra.position = Setup_Audio.DisplayValueText.transform.position;//移動至指定位置
            Setup_Audio.ChangeColor(true);
            Setup_FullWindown.ChangeColor(false);
        }
        else if (nowSelectNumber == (int)State.FullWindown)
        {
            NowSelectFrame_tra.position = Setup_FullWindown.DisplayValueText.transform.position;//移動至指定位置
            Setup_Audio.ChangeColor(false);
            Setup_FullWindown.ChangeColor(true);
        }
    }
    //修改設定
    void OptionSetupChange(int _number = 0)
    {
        switch ((State)nowSelectNumber)
        {
            case State.Audio:
                AudioValue(_number);
                break;
            case State.FullWindown:
                FullWindownChange();
                break;
        }
    }
    //音量大小
    void AudioValue(float _value)
    {
        float _updateValue = nowAudioValue + _value;
        Debug.Log("nowAudioValue:" + nowAudioValue + "\n" + "_updateValue:" + _updateValue);
        if (_value > 0)
        {
            if (_updateValue > maxAudioValue) { return; }
        }
        else if (_value < 0)
        {
            if (_updateValue < miniAudioValue) { return; }
        }

        nowAudioValue = _updateValue;//更新 遊戲顯示值

        float _audioMixer = 0;//實際音量大小
        if (_updateValue <= 0)
        {
            _audioMixer = -100;
        }
        else
        {
            //_audioMixer = nowAudioValue * SeveralMultiples - nowAudioMedian* SeveralMultiples;//目前顯示數值-中間值(0~5~10) * 倍數
            _audioMixer = nowMiniValue + nowAudioValue;//更新數值
        }
        //_audioMixer = nowAudioValue * SeveralMultiples - nowAudioMedian * SeveralMultiples;//目前顯示數值-中間值(0~5~10) * 倍數
        Game_AudioMixer.SetFloat("AllValue", _audioMixer);//修改遊戲音量

        Setup_Audio.DisplayValueText.text = nowAudioValue.ToString();//遊戲中 顯示數值
        Debug.Log("更新音量:" + nowAudioValue + "\n" + "實際音量:" + _audioMixer);
    }
    //切換視窗
    void FullWindownChange()
    {
        nowFullWindown = !nowFullWindown;
        Screen.fullScreen = nowFullWindown;
        DisplayState_FullWindownChange(nowFullWindown);
        Debug.Log(nowFullWindown);
    }
    //顯示狀態
    void DisplayState_FullWindownChange(bool _enabled)
    {
        switch (_enabled)
        {
            case true:
                Setup_FullWindown.DisplayValueText.text = "是";
                break;
            case false:
                Setup_FullWindown.DisplayValueText.text = "否";
                break;
        }
    }

    private void OnDisable()
    {
        _gameData.data.AudioValue = nowAudioValue;
        _gameData.data.FullOrWindowns = nowFullWindown;
        ///把開啟的對應畫面開啟
        if (nowOpenScreen_obj != null)
            nowOpenScreen_obj.SetActive(true);
    }
}
