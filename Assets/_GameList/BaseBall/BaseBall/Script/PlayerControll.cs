using UnityEngine;
[CreateAssetMenu(fileName = "New PlayerControll", menuName = "PlayerControll/PlayerControll")]
public class PlayerControll : ScriptableObject
{
    #region Input
    [Header("Input")]
    public string keyLeft = "left";
    public string keyRight = "right";
    public string keyUp = "up";
    public string keyDown = "down";
    #endregion

    public int characterIndex = 0;
}
