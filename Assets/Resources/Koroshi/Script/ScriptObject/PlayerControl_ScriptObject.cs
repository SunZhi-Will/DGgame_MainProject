using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//學生的ScriptableObject修改後
[CreateAssetMenu(fileName = "New Player", menuName = "Koroshi/PlayerControll")]
public class PlayerControl_ScriptObject : ScriptableObject
{
    public PlayerNumber playerNumber;
    #region Input
    [Header("Input")]
    public string keyUp = "Up";
    public string keyDown = "Down";
    public string keyLeft = "Left";
    public string keyRight = "Right";
    public string keyConfirm = "O";
    public string keyCancel = "P";
    #endregion
}
