using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAI : MonoBehaviour
{
    public int WinOrLosePref;
    // Start is called before the first frame update
    void Start()
    {
        WinOrLosePref = PlayerPrefs.GetInt("WinOrLose");
        if(WinOrLosePref == 1)
        {
            Debug.Log("贏");
        }
        else
        {
            Debug.Log("輸");
        }
    }


}
