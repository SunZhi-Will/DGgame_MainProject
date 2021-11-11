using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    #region Sington
    public static GameManage instance;
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }
    #endregion

    public int GameIndex;
    public int Round = 0;
    public int MinRound = 1;
    public int MaxRound = 9;
    public int Time = 60;
    public int MaxTime = 300;
    Change m_change;
    // Start is called before the first frame update
    void Start()
    {
        m_change = Change.instance;
        GameIndex = 0;
    }

    public void EnterGame()
    {
        switch (GameIndex) {
            case 0:
                Round = 0;
                MinRound = 3;
                MaxRound = 5;
                Time = 60;
                MaxTime = 120;
                break;
            case 1:
                Round = 3;
                MinRound = 3;
                MaxRound = 9;
                break;
            case 2:
                MaxTime = 300;
                break;
            case 3:
                Round = 3;
                MinRound = 3;
                break;
            case 4:
                Round = 3;
                MinRound = 3;
                break;
            case 5:
                Round = 3;
                MinRound = 3;
                break;
            case 6:
                Round = 0;
                MinRound = 1;
                MaxRound = 3;
                Time = 60;
                MaxTime = 90;
                break;

        }
        m_change.ChangeScene("PkMenu");
    }
}
