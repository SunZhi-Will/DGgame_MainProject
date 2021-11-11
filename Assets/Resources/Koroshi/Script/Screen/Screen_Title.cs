using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Koroshi
public class Screen_Title : MonoBehaviour
{
    public Screen_Option screen_Option_scr;
    public MainScene_UI mainScene_UI;

    private int nowSelectNumber;//目前選擇項目
    public Transform NowSelectFrame_tra;//目前選擇顯示框
    public Transform ButtonTotal_tra;//按鈕總物件
    private enum State { StartGame,Option, QuitGame}
    //private Button[] MenuBtn_Array;

    private ButtonMoveInOutEvent_Tool[] MenuBtn_array;

    public Screen_SelectCharacter screen_SelectCharacter_scr;//選擇角色

    private void OnEnable()
    {
        if (PlayerControl_koroshi.s_control == null) { return; }
        //mainScene_UI.OpenKeyTip(false);//標題-重新開啟時關閉
        mainScene_UI.OpenKeyTip(true);//標題-顯示確認按鍵KeyTip
    }

    IEnumerator Start()
    {
        int _maxCount = ButtonTotal_tra.childCount;
        MenuBtn_array = new ButtonMoveInOutEvent_Tool[_maxCount];
        for (int i = 0; i < _maxCount; i++)
        {
            MenuBtn_array[i] = ButtonTotal_tra.GetChild(i).GetComponent<ButtonMoveInOutEvent_Tool>();
            if (MenuBtn_array[i] == null) { Debug.LogError(gameObject.name + "\n" + MenuBtn_array[i].name + " 沒有按鈕組件!"); }
        }

        //滑鼠點用
        MenuBtn_array[(int)State.StartGame].SetClickEvent(() => { StartGame(); });
        MenuBtn_array[(int)State.StartGame].SetMoveInEvent(() => { MoveSelect((int)State.StartGame); });

        MenuBtn_array[(int)State.Option].SetClickEvent(() =>{ Option(); });
        MenuBtn_array[(int)State.Option].SetMoveInEvent(() =>{ MoveSelect((int)State.Option); });


        MenuBtn_array[(int)State.QuitGame].SetClickEvent(() => { QuitGame(); });
        MenuBtn_array[(int)State.QuitGame].SetMoveInEvent(() => { MoveSelect((int)State.QuitGame); ; });

        NowSelectFrame_tra.position = MenuBtn_array[0].transform.position;
        yield return new WaitForSeconds(0.01f);
    }

    void Update()
    {
        if(PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Up) || PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Up))
        {
            AudioManager.s_AudioManager.PlayAudio_Move();
            ChangeSelect(-1);
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Down) || PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Down))
        {
            AudioManager.s_AudioManager.PlayAudio_Move();
            ChangeSelect(+1);
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Confirm) || PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Confirm))
        {
            AudioManager.s_AudioManager.PlayAudio_Click();
            ConfirmSelect();//確認選項
        }
    }
    //移動至指定選項
    void MoveSelect(int _number)
    {
        nowSelectNumber = _number;
        NowSelectFrame_tra.position = MenuBtn_array[_number].transform.position;//移動至指定位置
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
            if (_predictionUpdate < 0 ) { return; }//超出列表最大數
        }
        nowSelectNumber = _predictionUpdate;//同步更新
        NowSelectFrame_tra.position = MenuBtn_array[nowSelectNumber].transform.position;//移動至指定位置
    }

    //確認選擇
    void ConfirmSelect()
    {
        switch ((State)nowSelectNumber)
        {
            case State.StartGame:
                StartGame();//按鍵用
                break;
            case State.Option:
                Option();//按鍵用
                break;
            case State.QuitGame:
                QuitGame();//按鍵用
                break;
        }
    }

    void StartGame()
    {
        Debug.Log("開始遊戲");
        screen_SelectCharacter_scr.gameObject.SetActive(true);
        gameObject.SetActive(false);
        mainScene_UI.OpenKeyTip(true);//標題-開始新遊戲恢復按鍵KeyTip
    }
    void Option()
    {
        screen_Option_scr.OpenScreen(gameObject);
        gameObject.SetActive(false);
        Debug.Log("遊戲選項開啟");
    }
    void QuitGame()
    {
        Application.Quit();
        Debug.Log("離開遊戲");
    }
}
