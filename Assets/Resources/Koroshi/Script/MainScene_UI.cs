using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameKeyTipState { Confirm, Cancel, }

//Koroshi
//主畫面UI
public class MainScene_UI : MonoBehaviour
{
    public Screen_Title screen_Title_scr;
    public Screen_GameSelect screen_GameSelect_scr;

    [System.Serializable]
    public struct GameKeyTip
    {
        public GameObject Confirm;
        public GameObject Cancel;
    }
    public GameObject DownGameKeyTipAll_obj;//下方列表遊戲按鍵提示
    public GameKeyTip
        playerAll_KeyTip,//所有玩家
        player1_KeyTip,//玩家1按鍵提示
        player2_KeyTip;//玩家2按鍵提示

    public void OpenKeyTip(bool _enabled)
    {
        DownGameKeyTipAll_obj.SetActive(_enabled);
    }
    public void OpenKeyTip(PlayerNumber _playerNumber, GameKeyTipState gameKeyTipState, bool _enabled)
    {
        switch (gameKeyTipState)
        {
            case GameKeyTipState.Confirm:
                if (_playerNumber == PlayerNumber.p1)
                    player1_KeyTip.Confirm.SetActive(_enabled);
                else if (_playerNumber == PlayerNumber.p2)
                    player2_KeyTip.Confirm.SetActive(_enabled);
                else
                    playerAll_KeyTip.Confirm.SetActive(_enabled);
                break;
            case GameKeyTipState.Cancel:
                if (_playerNumber == PlayerNumber.p1)
                    player1_KeyTip.Cancel.SetActive(_enabled);
                else if (_playerNumber == PlayerNumber.p2)
                    player2_KeyTip.Cancel.SetActive(_enabled);
                else
                    playerAll_KeyTip.Cancel.SetActive(_enabled);
                break;
        }
    }

    void Start()
    {
        //如果有執行過遊戲，返回時回到遊戲選擇畫面
        if (GameMaster_koroshi.s_GameMaster.data.nowSelectGame != null) 
        {
            screen_Title_scr.gameObject.SetActive(false);
            screen_GameSelect_scr.gameObject.SetActive(true);
            OpenKeyTip(true);
        }
    }

    void Update()
    {
        
    }
}
