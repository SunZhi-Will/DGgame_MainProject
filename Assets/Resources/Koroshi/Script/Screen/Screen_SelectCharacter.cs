using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//遊戲選擇
public class Screen_SelectCharacter : MonoBehaviour
{
    public GameObject MainCamera_obj;//主攝影機
    public MainScene_UI mainScene_UI;

    public enum State
    {
        Selecting,//角色選擇中
        SelectOver,//選擇結束
    }
    //玩家資訊
    [System.Serializable]
    public struct PlayerData
    {
        public State state;//目前選擇狀態
        public int nowSelectNumber;//目前 選擇 角色編號
        public Transform Character_totalObj;//角色列表總物件
        [HideInInspector]
        public GameObject[] Character_Lsit;//角色列表
        public Transform SelectHead_tra;//目前選擇頭像位置
        public GameObject Confirm_obj;//確認抉擇角色訊息
    }
    public PlayerData
        p1_data,
        p2_data;

    public Screen_Title screen_Title_scr;//標題場景
    public Screen_GameSelect screen_GameSelect_scr;//角色選擇場景

    public GameObject Background;//底圖背景，因為背景UI要在模型後面

    public Transform SelectCharacterHead_tra;//選擇角色頭項-總物件
    private Transform[] SelectCharacterHead_array;//選擇角色頭項-總物件

    private void Awake()
    {
        //取得角色列表 P1
        int _maxCount = p1_data.Character_totalObj.childCount;
        p1_data.Character_Lsit = new GameObject[_maxCount];
        for (int i = 0; i < _maxCount; i++)
        {
            GameObject _obj = p1_data.Character_totalObj.GetChild(i).gameObject;
            _obj.SetActive(false);
            p1_data.Character_Lsit[i] = _obj;
        }
        p1_data.Character_Lsit[p1_data.nowSelectNumber = 0].SetActive(true);//初始化預設角色

        //取得角色列表 P2
        _maxCount = p2_data.Character_totalObj.childCount;
        p2_data.Character_Lsit = new GameObject[_maxCount];
        for (int i = 0; i < _maxCount; i++)
        {
            GameObject _obj = p2_data.Character_totalObj.GetChild(i).gameObject;
            _obj.SetActive(false);
            p2_data.Character_Lsit[i] = _obj;
        }
        p2_data.Character_Lsit[p2_data.nowSelectNumber = 1].SetActive(true);//初始化預設角色

        //取得頭像列表
        _maxCount = SelectCharacterHead_tra.childCount;
        SelectCharacterHead_array = new Transform[_maxCount];
        for (int i = 0; i < _maxCount; i++)
        {
            SelectCharacterHead_array[i] = SelectCharacterHead_tra.GetChild(i);
        }

        Init();
    }

    void Init()
    {
        //初始化角色顯示
        p1_data.Character_Lsit[p1_data.nowSelectNumber].SetActive(false);//把上一次開啟的角色物件關閉
        p1_data.Character_Lsit[p1_data.nowSelectNumber = 0].SetActive(true);//初始化預設角色
        p1_data.Confirm_obj.SetActive(false);

        p2_data.Character_Lsit[p2_data.nowSelectNumber].SetActive(false);//把上一次開啟的角色物件關閉
        p2_data.Character_Lsit[p2_data.nowSelectNumber = 1].SetActive(true);//初始化預設角色
        p2_data.Confirm_obj.SetActive(false);

        if (Background == false) { return; }
        Background.SetActive(false);//關閉底圖

        //預設目前顯示選項
        p1_data.SelectHead_tra.position = SelectCharacterHead_array[p1_data.nowSelectNumber].position;
        p2_data.SelectHead_tra.position = SelectCharacterHead_array[p2_data.nowSelectNumber].position;

        //初始化選擇狀態
        ChangeState(ref p1_data.state, State.Selecting);
        ChangeState(ref p2_data.state, State.Selecting);
    }

    private void OnEnable()
    {
        Background.SetActive(true);
        MainCamera_obj.SetActive(false);
    }
    void Start()
    {
        
    }

    void Update()
    {
        ///雙方準備就緒後，剩餘1P鍵開始
        //bool _openBtn = CheckState(p1_data.state, State.SelectOver) && CheckState(p2_data.state, State.SelectOver);
        //mainScene_UI.OpenKeyTip(PlayerNumber.p2, GameKeyTipState.Confirm, !_openBtn);
        //mainScene_UI.OpenKeyTip(PlayerNumber.p2, GameKeyTipState.Cancel, !_openBtn);

        Player_1_Control();
        Player_2_Control();
    }

    #region 1P控制器 Player_1_Control
    //1P控制器
    void Player_1_Control()
    {
        //if(PlayerControl_koroshi.KeyDown(PlayerNumber.p1,PlayerKeyName.Up))
        //{
        //}
        //else if(PlayerControl_koroshi.KeyDown(PlayerNumber.p1,PlayerKeyName.Down))
        //{
        //}
        if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Left))
        {
            if (CheckState(p1_data.state, State.Selecting))//確認在角色選擇中
            {
                ChangeCharacter_p1(ref p1_data.nowSelectNumber, -1);
            }
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Right))
        {
            if (CheckState(p1_data.state, State.Selecting))//確認在角色選擇中
            {
                ChangeCharacter_p1(ref p1_data.nowSelectNumber, +1);
            }
        }
        //確認
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Confirm))
        {
            //當雙都確認選擇後，由1P決定前往下一頁
            if (CheckState(p1_data.state, State.SelectOver) && CheckState(p2_data.state, State.SelectOver))
            {
                screen_GameSelect_scr.gameObject.SetActive(true);
                gameObject.SetActive(false);
                Debug.Log("雙方準備就緒，前往遊戲選擇!");
                AudioManager.s_AudioManager.PlayAudio_Click();
            }
            //確認在角色選擇中   
            else if (CheckState(p1_data.state, State.Selecting))
            {
                Debug.Log("1P確認角色");
                p1_data.Confirm_obj.SetActive(true);
                ChangeState(ref p1_data.state, State.SelectOver);//確認角色
                //GameMaster_koroshi.s_GameMaster.data.p1_SelectCharacter = p1_data.nowSelectNumber;
                GameMaster_koroshi.s_GameMaster.SetSelectCharacter(PlayerNumber.p1, p1_data.nowSelectNumber);
                AudioManager.s_AudioManager.PlayAudio_Click();
            }
        }
        //取消
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Cancel) || PlayerControl_koroshi.KeyDown(PlayerKeyName.Mouse1) || PlayerControl_koroshi.Escape_keyDown)
        {
            //如果在選擇角色中
            if (CheckState(p1_data.state, State.SelectOver))
            {
                p1_data.Confirm_obj.SetActive(false);
                ChangeState(ref p1_data.state, State.Selecting);//返回選角
            }
            //否則回到標題
            else
            {
                screen_Title_scr.gameObject.SetActive(true);
                gameObject.SetActive(false);

                //GameMaster_koroshi.s_GameMaster.data.p1_SelectCharacter = 0;
                //GameMaster_koroshi.s_GameMaster.data.p2_SelectCharacter = 0;
                GameMaster_koroshi.s_GameMaster.SetSelectCharacter(PlayerNumber.p1, 0);
                GameMaster_koroshi.s_GameMaster.SetSelectCharacter(PlayerNumber.p2, 0);
            }
        }
    }
    #endregion 1P控制器

    #region 2P控制器 Player_2_Control
    //2P控制器
    void Player_2_Control()
    {
        if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Right))
        {
            if (CheckState(p2_data.state, State.Selecting) == false) { return; }//確認在角色選擇中
            ChangeCharacter_p2(ref p2_data.nowSelectNumber, +1);//2P頭像
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Left))
        {
            if (CheckState(p2_data.state, State.Selecting) == false) { return; }//確認在角色選擇中
            ChangeCharacter_p2(ref p2_data.nowSelectNumber, -1);//2P頭像
        }
        //確認
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Confirm))
        {
            if (CheckState(p2_data.state, State.Selecting) == false) { return; }//確認在角色選擇中
            p2_data.Confirm_obj.SetActive(true);
            ChangeState(ref p2_data.state, State.SelectOver);//確認選擇角色
            //GameMaster_koroshi.s_GameMaster.data.p2_SelectCharacter = p2_data.nowSelectNumber;
            GameMaster_koroshi.s_GameMaster.SetSelectCharacter(PlayerNumber.p2, p2_data.nowSelectNumber);
            Debug.Log("2P 確認 選擇角色");
            AudioManager.s_AudioManager.PlayAudio_Click();
        }
        //取消
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Cancel))
        {
            if (CheckState(p2_data.state, State.SelectOver) == false) { return; }//確認在角色選擇中
            p2_data.Confirm_obj.SetActive(false);
            ChangeState(ref p2_data.state, State.Selecting);//返回選角
            Debug.Log("2P 取消 選擇角色");
        }
    }
    #endregion 2P控制器


    //切換
    void ChangeState(ref State _target, State _newState)
    {
        _target = _newState;
    }
    //檢查
    bool CheckState(State _target,State _newState)
    {
        return _target == _newState;
    }

    #region 角色切換頭像-1p ChangeCharacter_p1
    //切換角色頭像-1P
    void ChangeCharacter_p1(ref int _changeNumber, int _updateNumber)
    {
        int _predictionUpdate = _changeNumber + _updateNumber;//更新預測，檢測是否超出陣列
        //如果目前選項已經被另外一名玩家選擇過，禁止選擇
        if (_predictionUpdate == p2_data.nowSelectNumber)
        {
            if (_updateNumber < 0)
            {
                if (_predictionUpdate < 1) { return; }//不能選擇到第一格
                else { _predictionUpdate--; }//相同位置，再往前一格
            }
            else if (_updateNumber > 0)
            {
                if (_predictionUpdate >= SelectCharacterHead_array.Length - 1) { return; }//不能選到最後一格
                else { _predictionUpdate++; }//相同位置，再往後一格
            }
        }
        //不重複的情況
        else if (_predictionUpdate >= SelectCharacterHead_array.Length)  { return; }//超出最大選擇
        else if (_predictionUpdate < 0) { return; }//低於0
        ChangeCharacter(p1_data.Character_Lsit, _changeNumber, false);//將舊角色的關閉
        _changeNumber = _predictionUpdate;//同步更新位置
        p1_data.SelectHead_tra.position = SelectCharacterHead_array[_changeNumber].position;//顯示頭像位置
        ChangeCharacter(p1_data.Character_Lsit, _changeNumber, true);//將新角色開啟
        AudioManager.s_AudioManager.PlayAudio_Move();
    }
    #endregion 角色切換頭像-1p

    #region 切換角色頭像-2p ChangeCharacter_p2
    //切換角色頭像-2P
    void ChangeCharacter_p2(ref int _changeNumber, int _updateNumber)
    {
        int _predictionUpdate = _changeNumber + _updateNumber;//更新預測，檢測是否超出陣列
        //如果目前選項已經被另外一名玩家選擇過，禁止選擇
        if (_predictionUpdate == p1_data.nowSelectNumber)
        {
            if (_updateNumber < 0)
            {
                if (_predictionUpdate < 1) { return; }//不能選擇到第一格
                else { _predictionUpdate--; }//相同位置，再往前一格
            }
            else if (_updateNumber > 0)
            {
                if (_predictionUpdate >= SelectCharacterHead_array.Length - 1) { return; }//不能選到最後一格
                else { _predictionUpdate++; }//相同位置，再往後一格
            }
        }
        //不重複的情況
        else if (_predictionUpdate >= SelectCharacterHead_array.Length) { return; }//超出最大選擇
        else if (_predictionUpdate < 0) { return; }//低於0
        ChangeCharacter(p2_data.Character_Lsit, _changeNumber, false);//將舊角色的關閉
        _changeNumber = _predictionUpdate;//同步更新位置
        p2_data.SelectHead_tra.position = SelectCharacterHead_array[_changeNumber].position;//顯示頭像位置
        ChangeCharacter(p2_data.Character_Lsit, _changeNumber, true);//將新角色開啟
        AudioManager.s_AudioManager.PlayAudio_Move();
    }
    #endregion 角色切換頭像-2p

    //開關角色顯示
    void ChangeCharacter(GameObject[] _obj_array, int _changeNumber, bool _enabled)
    {
        if (_changeNumber < 0 || _changeNumber >= _obj_array.Length) { Debug.LogError("超出陣列"); return; } 
        _obj_array[_changeNumber].SetActive(_enabled);
    }

    private void OnDisable()
    {
        MainCamera_obj.SetActive(true);
        Init();
    }
}