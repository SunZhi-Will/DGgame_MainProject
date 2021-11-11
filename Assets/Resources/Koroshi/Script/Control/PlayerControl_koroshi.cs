using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerNumber { none, p1, p2, }
public enum PlayerKeyName { Up,Down,Left,Right, Confirm, Cancel,Mouse0, Mouse1,Axis_Horizontal, Axis_Vertical }
//public enum GameKeyTipState { Confirm, Cancel, }

//Koroshi
//遊戲控制器
public class PlayerControl_koroshi : MonoBehaviour
{
    //[System.Serializable]
    //public struct GameKeyTip
    //{
    //    public GameObject Confirm;
    //    public GameObject Cancel;
    //}
    //public GameObject DownGameKeyTipAll_obj;//下方列表遊戲按鍵提示
    //public GameKeyTip
    //    playerAll_KeyTip,//所有玩家
    //    player1_KeyTip,//玩家1按鍵提示
    //    player2_KeyTip;//玩家2按鍵提示

    public static PlayerControl_koroshi s_control { get { return playerControl_koroshi; } }
    private static PlayerControl_koroshi playerControl_koroshi;

    public PlayerControl_ScriptObject p1_scrObj;
    public PlayerControl_ScriptObject p2_scrObj;

    internal static bool
        Escape_keyDown,

        //W_keyDown,
        //S_keyDown,
        //A_keyDown,
        //D_keyDown,
        //Confirm_1p_keyDown,
        //Cancel_1p_keyDown,

        //UpArrow_keyDown,
        //DownArrow_keyDown,
        //LeftArrow_keyDown,
        //RightArrow_keyDown,
        //Confirm_2p_keyDown,
        //Cancel_2p_keyDown,

        //Mouse1_KeyDown,//右鍵按下

        Enter_keyDown;
    
    void Awake()
    {
        if (playerControl_koroshi == null)
        {
            playerControl_koroshi = this;
            if (p1_scrObj == null) { Debug.LogError("p1沒有設定按鍵內容!"); }
            if (p2_scrObj == null) { Debug.LogError("p2沒有設定按鍵內容!"); }
        }
    }

    void Update()
    {
        Escape_keyDown = Input.GetKeyDown(KeyCode.Escape);

        //W_keyDown = Input.GetKeyDown(KeyCode.W);
        //S_keyDown = Input.GetKeyDown(KeyCode.S);
        //A_keyDown = Input.GetKeyDown(KeyCode.A);
        //D_keyDown = Input.GetKeyDown(KeyCode.D);

        //Confirm_1p_keyDown = Input.GetKeyDown(KeyCode.C);
        //Cancel_1p_keyDown = Input.GetKeyDown(KeyCode.V);

        //UpArrow_keyDown = Input.GetKeyDown(KeyCode.UpArrow);
        //DownArrow_keyDown = Input.GetKeyDown(KeyCode.DownArrow);
        //LeftArrow_keyDown = Input.GetKeyDown(KeyCode.LeftArrow);
        //RightArrow_keyDown = Input.GetKeyDown(KeyCode.RightArrow);

        //Confirm_2p_keyDown = Input.GetKeyDown(KeyCode.O);
        //Cancel_2p_keyDown = Input.GetKeyDown(KeyCode.P);

        //Mouse1_KeyDown = Input.GetKeyDown(KeyCode.Mouse1);

        Enter_keyDown = Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return);
    }

    //public void OpenKeyTip(bool _enabled)
    //{
    //    DownGameKeyTipAll_obj.SetActive(_enabled);
    //}
    //public void OpenKeyTip(PlayerNumber _playerNumber, GameKeyTipState gameKeyTipState, bool _enabled)
    //{
    //    switch (gameKeyTipState)
    //    {
    //        case GameKeyTipState.Confirm:
    //            if (_playerNumber == PlayerNumber.p1)
    //                player1_KeyTip.Confirm.SetActive(_enabled);
    //            else if (_playerNumber == PlayerNumber.p2)
    //                player2_KeyTip.Confirm.SetActive(_enabled);
    //            else
    //                playerAll_KeyTip.Confirm.SetActive(_enabled);
    //            break;
    //        case GameKeyTipState.Cancel:
    //            if (_playerNumber == PlayerNumber.p1)
    //                player1_KeyTip.Cancel.SetActive(_enabled);
    //            else if (_playerNumber == PlayerNumber.p2)
    //                player2_KeyTip.Cancel.SetActive(_enabled);
    //            else
    //                playerAll_KeyTip.Cancel.SetActive(_enabled);
    //            break;
    //    }
    //}

    #region KeyDown(PlayerKeyName)
    public static bool KeyDown(PlayerKeyName _key)
    {
        SpawnData();
        if (_key == PlayerKeyName.Up)
        {
            return KeyDown(s_control.p1_scrObj.keyUp) || KeyDown(s_control.p2_scrObj.keyUp);
        }
        else if (_key == PlayerKeyName.Down)
        {
            return KeyDown(s_control.p1_scrObj.keyDown) || KeyDown(s_control.p2_scrObj.keyDown);
        }
        else if (_key == PlayerKeyName.Left)
        {
            return KeyDown(s_control.p1_scrObj.keyLeft) || KeyDown(s_control.p2_scrObj.keyLeft);
        }
        else if (_key == PlayerKeyName.Right)
        {
            return KeyDown(s_control.p1_scrObj.keyRight) || KeyDown(s_control.p2_scrObj.keyRight);
        }
        else if (_key == PlayerKeyName.Confirm)
        {
            return KeyDown(s_control.p1_scrObj.keyConfirm) || KeyDown(s_control.p2_scrObj.keyConfirm);
        }
        else if (_key == PlayerKeyName.Cancel)
        {
            return KeyDown(s_control.p1_scrObj.keyCancel) || KeyDown(s_control.p2_scrObj.keyCancel);
        }
        else if (_key == PlayerKeyName.Mouse0) { return KeyDown("Mouse0"); }
        else if (_key == PlayerKeyName.Mouse1) { return KeyDown("Mouse1"); }
        else { return false; }
    }
    #endregion KeyDown
    #region KeyDown(PlayerNumber,PlayerKeyName)
    public static bool KeyDown(PlayerNumber _p1, PlayerKeyName _key)
    {
        SpawnData();
        switch (_p1)
        {
            case PlayerNumber.p1:
                if (_key == PlayerKeyName.Up)
                {
                    return KeyDown(s_control.p1_scrObj.keyUp);
                }
                else if (_key == PlayerKeyName.Down)
                {
                    return KeyDown(s_control.p1_scrObj.keyDown);
                }
                else if (_key == PlayerKeyName.Left)
                {
                    return KeyDown(s_control.p1_scrObj.keyLeft);
                }
                else if (_key == PlayerKeyName.Right)
                {
                    return KeyDown(s_control.p1_scrObj.keyRight);
                }
                else if (_key == PlayerKeyName.Confirm)
                {
                    return KeyDown(s_control.p1_scrObj.keyConfirm);
                }
                else if (_key == PlayerKeyName.Cancel)
                {
                    return KeyDown(s_control.p1_scrObj.keyCancel);
                }
                else { return false; }
            case PlayerNumber.p2:
                if (_key == PlayerKeyName.Up)
                {
                    return KeyDown(s_control.p2_scrObj.keyUp);
                }
                else if (_key == PlayerKeyName.Down)
                {
                    return KeyDown(s_control.p2_scrObj.keyDown);
                }
                else if (_key == PlayerKeyName.Left)
                {
                    return KeyDown(s_control.p2_scrObj.keyLeft);
                }
                else if (_key == PlayerKeyName.Right)
                {
                    return KeyDown(s_control.p2_scrObj.keyRight);
                }
                else if (_key == PlayerKeyName.Confirm)
                {
                    return KeyDown(s_control.p2_scrObj.keyConfirm);
                }
                else if (_key == PlayerKeyName.Cancel)
                {
                    return KeyDown(s_control.p2_scrObj.keyCancel);
                }
                else { return false; }
            default:
                return false;
        }
    }
    #endregion KeyDown
    #region KeyDown(string)
    public static bool KeyDown(string keyName)
    {
        //Debug.Log("key:" + keyName);
        if (keyName == "Up" || keyName == "up")
        {
            return Input.GetKeyDown(KeyCode.UpArrow);
        }
        else if (keyName == "Down" || keyName == "down")
        {
            return Input.GetKeyDown(KeyCode.DownArrow);
        }
        else if (keyName == "Left" || keyName == "left")
        {
            return Input.GetKeyDown(KeyCode.LeftArrow);
        }
        else if (keyName == "Right" || keyName == "right")
        {
            return Input.GetKeyDown(KeyCode.RightArrow);
        }
        else if (keyName == "O" || keyName == "o")
        {
            return Input.GetKeyDown(KeyCode.O);
        }
        else if (keyName == "P" || keyName == "p")
        {
            return Input.GetKeyDown(KeyCode.P);
        }

        else if (keyName == "W" || keyName == "w")
        {
            return Input.GetKeyDown(KeyCode.W);
        }
        else if (keyName == "S" || keyName == "s")
        {
            return Input.GetKeyDown(KeyCode.S);
        }
        else if (keyName == "A" || keyName == "a")
        {
            return Input.GetKeyDown(KeyCode.A);
        }
        else if (keyName == "D" || keyName == "d")
        {
            return Input.GetKeyDown(KeyCode.D);
        }
        else if (keyName == "C" || keyName == "c")
        {
            return Input.GetKeyDown(KeyCode.C);
        }
        else if (keyName == "V" || keyName == "v")
        {
            return Input.GetKeyDown(KeyCode.V);
        }
        else if (keyName == "Mouse0")
        {
            return Input.GetKeyDown(KeyCode.Mouse0);
        }
        else if (keyName == "Mouse1")
        {
            return Input.GetKeyDown(KeyCode.Mouse1);
        }
        else { return false; }
    }
    #endregion KeyDown
    #region Key(PlayerNumber,PlayerKeyName)
    public static bool Key(PlayerNumber _p1, PlayerKeyName _key)
    {
        SpawnData();
        switch (_p1)
        {
            case PlayerNumber.p1:
                if (_key == PlayerKeyName.Up)
                {
                    return Key(s_control.p1_scrObj.keyUp);
                }
                else if (_key == PlayerKeyName.Down)
                {
                    return Key(s_control.p1_scrObj.keyDown);
                }
                else if (_key == PlayerKeyName.Left)
                {
                    return Key(s_control.p1_scrObj.keyLeft);
                }
                else if (_key == PlayerKeyName.Right)
                {
                    return Key(s_control.p1_scrObj.keyRight);
                }
                else if (_key == PlayerKeyName.Confirm)
                {
                    return Key(s_control.p1_scrObj.keyConfirm);
                }
                else if (_key == PlayerKeyName.Cancel)
                {
                    return Key(s_control.p1_scrObj.keyCancel);
                }
                else { return false; }
            case PlayerNumber.p2:
                if (_key == PlayerKeyName.Up)
                {
                    return Key(s_control.p2_scrObj.keyUp);
                }
                else if (_key == PlayerKeyName.Down)
                {
                    return Key(s_control.p2_scrObj.keyDown);
                }
                else if (_key == PlayerKeyName.Left)
                {
                    return Key(s_control.p2_scrObj.keyLeft);
                }
                else if (_key == PlayerKeyName.Right)
                {
                    return Key(s_control.p2_scrObj.keyRight);
                }
                else if (_key == PlayerKeyName.Confirm)
                {
                    return Key(s_control.p2_scrObj.keyConfirm);
                }
                else if (_key == PlayerKeyName.Cancel)
                {
                    return Key(s_control.p2_scrObj.keyCancel);
                }
                else { return false; }
            default:
                return false;
        }
    }
    #endregion Key
    #region Key(string)
    public static bool Key(string keyName)
    {
        //Debug.Log("key:" + keyName);
        if (keyName == "Up" || keyName == "up")
        {
            return Input.GetKey(KeyCode.UpArrow);
        }
        else if (keyName == "Down" || keyName == "down")
        {
            return Input.GetKey(KeyCode.DownArrow);
        }
        else if (keyName == "Left" || keyName == "left")
        {
            return Input.GetKey(KeyCode.LeftArrow);
        }
        else if (keyName == "Right" || keyName == "right")
        {
            return Input.GetKey(KeyCode.RightArrow);
        }
        else if (keyName == "O" || keyName == "o")
        {
            return Input.GetKey(KeyCode.O);
        }
        else if (keyName == "P" || keyName == "p")
        {
            return Input.GetKey(KeyCode.P);
        }

        else if (keyName == "W" || keyName == "w")
        {
            return Input.GetKey(KeyCode.W);
        }
        else if (keyName == "S" || keyName == "s")
        {
            return Input.GetKey(KeyCode.S);
        }
        else if (keyName == "A" || keyName == "a")
        {
            return Input.GetKey(KeyCode.A);
        }
        else if (keyName == "D" || keyName == "d")
        {
            return Input.GetKey(KeyCode.D);
        }
        else if (keyName == "C" || keyName == "c")
        {
            return Input.GetKey(KeyCode.C);
        }
        else if (keyName == "V" || keyName == "v")
        {
            return Input.GetKey(KeyCode.V);
        }
        else if (keyName == "Mouse0")
        {
            return Input.GetKey(KeyCode.Mouse0);
        }
        else if (keyName == "Mouse1")
        {
            return Input.GetKey(KeyCode.Mouse1);
        }
        else { return false; }
    }
    #endregion KeyDown
    #region KeyUp(PlayerNumber,PlayerKeyName)
    public static bool KeyUp(PlayerNumber _p1, PlayerKeyName _key)
    {
        SpawnData();
        switch (_p1)
        {
            case PlayerNumber.p1:
                if (_key == PlayerKeyName.Up)
                {
                    return KeyUp(s_control.p1_scrObj.keyUp);
                }
                else if (_key == PlayerKeyName.Down)
                {
                    return KeyUp(s_control.p1_scrObj.keyDown);
                }
                else if (_key == PlayerKeyName.Left)
                {
                    return KeyUp(s_control.p1_scrObj.keyLeft);
                }
                else if (_key == PlayerKeyName.Right)
                {
                    return KeyUp(s_control.p1_scrObj.keyRight);
                }
                else if (_key == PlayerKeyName.Confirm)
                {
                    return KeyUp(s_control.p1_scrObj.keyConfirm);
                }
                else if (_key == PlayerKeyName.Cancel)
                {
                    return KeyUp(s_control.p1_scrObj.keyCancel);
                }
                else { return false; }
            case PlayerNumber.p2:
                if (_key == PlayerKeyName.Up)
                {
                    return KeyUp(s_control.p2_scrObj.keyUp);
                }
                else if (_key == PlayerKeyName.Down)
                {
                    return KeyUp(s_control.p2_scrObj.keyDown);
                }
                else if (_key == PlayerKeyName.Left)
                {
                    return KeyUp(s_control.p2_scrObj.keyLeft);
                }
                else if (_key == PlayerKeyName.Right)
                {
                    return KeyUp(s_control.p2_scrObj.keyRight);
                }
                else if (_key == PlayerKeyName.Confirm)
                {
                    return KeyUp(s_control.p2_scrObj.keyConfirm);
                }
                else if (_key == PlayerKeyName.Cancel)
                {
                    return KeyUp(s_control.p2_scrObj.keyCancel);
                }
                else { return false; }
            default:
                return false;
        }
    }
    #endregion KeyUp
    #region KeyUp(string)
    public static bool KeyUp(string keyName)
    {
        //Debug.Log("key:" + keyName);
        if (keyName == "Up" || keyName == "up")
        {
            return Input.GetKeyUp(KeyCode.UpArrow);
        }
        else if (keyName == "Down" || keyName == "down")
        {
            return Input.GetKeyUp(KeyCode.DownArrow);
        }
        else if (keyName == "Left" || keyName == "left")
        {
            return Input.GetKeyUp(KeyCode.LeftArrow);
        }
        else if (keyName == "Right" || keyName == "right")
        {
            return Input.GetKeyUp(KeyCode.RightArrow);
        }
        else if (keyName == "O" || keyName == "o")
        {
            return Input.GetKeyUp(KeyCode.O);
        }
        else if (keyName == "P" || keyName == "p")
        {
            return Input.GetKeyUp(KeyCode.P);
        }

        else if (keyName == "W" || keyName == "w")
        {
            return Input.GetKeyUp(KeyCode.W);
        }
        else if (keyName == "S" || keyName == "s")
        {
            return Input.GetKeyUp(KeyCode.S);
        }
        else if (keyName == "A" || keyName == "a")
        {
            return Input.GetKeyUp(KeyCode.A);
        }
        else if (keyName == "D" || keyName == "d")
        {
            return Input.GetKeyUp(KeyCode.D);
        }
        else if (keyName == "C" || keyName == "c")
        {
            return Input.GetKeyUp(KeyCode.C);
        }
        else if (keyName == "V" || keyName == "v")
        {
            return Input.GetKeyUp(KeyCode.V);
        }
        else if (keyName == "Mouse0")
        {
            return Input.GetKeyUp(KeyCode.Mouse0);
        }
        else if (keyName == "Mouse1")
        {
            return Input.GetKeyUp(KeyCode.Mouse1);
        }
        else { return false; }
    }
    #endregion GetKeyUp
    #region GetAxis(PlayerNumber,PlayerKeyName)
    public static float GetAxis(PlayerNumber _playerNumber, PlayerKeyName _playerKeyName)
    {
        float _value = 0;
        switch (_playerNumber)
        {
            case PlayerNumber.p1:
                if (_playerKeyName == PlayerKeyName.Axis_Horizontal) { _value = Input.GetAxis("Horizontal_p1")*-1; }
                else if (_playerKeyName == PlayerKeyName.Axis_Vertical) { _value = Input.GetAxis("Vertical_p1"); }
                break;
            case PlayerNumber.p2:
                if (_playerKeyName == PlayerKeyName.Axis_Horizontal) { _value = Input.GetAxis("Horizontal_p2")*-1; }
                else if (_playerKeyName == PlayerKeyName.Axis_Vertical) { _value = Input.GetAxis("Vertical_p2"); }
                break;
        }
        return _value;
    }
    #endregion GetAxis

    #region 測試用 SpawnData
    static void SpawnData()
    {
        if (playerControl_koroshi == null)
        {
            GameObject _obj = new GameObject();
            _obj.name = "-PlayerControl-";
            playerControl_koroshi = _obj.AddComponent<PlayerControl_koroshi>();
            Debug.LogWarning("測試功能!生成物件!");

            PlayerControl_ScriptObject[] _control_array = Resources.LoadAll<PlayerControl_ScriptObject>("");
            s_control.p1_scrObj = _control_array[0];
            s_control.p2_scrObj = _control_array[1];
        }
    }
    #endregion 測試用
}
