using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Koroshi
//角色頭像切換-掛在外部(共通模組)
public class CharacterHeadChange : MonoBehaviour
{
    public PlayerNumber playerNumber;//目前設定編號
    public Image Head_imgae;//頭像

    void Start()
    {
        GameMaster_koroshi _data = GameMaster_koroshi.s_GameMaster;
        if (_data == null) { return; }
        switch (playerNumber)
        {
            case PlayerNumber.p1:
                Head_imgae.sprite = _data.p1_Head;
                break;
            case PlayerNumber.p2:
                Head_imgae.sprite = _data.p2_Head;
                break;
            default:
                Debug.LogError(gameObject.name + "\n" + "錯誤!沒有設定玩家編號!");
                break;
        }
    }
}
