using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public GameObject UIPanel;
    int time_int = 4;
    public bool GameStart = false;//倒數開始改false
    public GameObject m1;
    public GameObject m2;
    public GameObject m3;
    public GameObject ms;
    public Text MessageText;
    // Start is called before the first frame update
    void Start()
    {
        MessageText.gameObject.SetActive(true);
        InvokeRepeating("timer", 0, 1);
    }

    /*void timer()
    {

        time_int -= 1;

        time_UI.text = time_int + "";

        if (time_int == 0)
        {
            GameStart = true;
            time_UI.text = " ";
            CancelInvoke("timer");
            Destroy(UIPanel);
        }

    }*/

    void timer()
    {
        time_int -= 1;//遞減
        if (time_int > 0)
        {
            MessageText.text = time_int.ToString();//顯示倒數訊息
        }
        if (time_int == 0)
        {
            MessageText.text = "Start";//顯示倒數訊息
        }
        //歸零開始遊戲
        else if (time_int == -1)
        {
            GameStart = true;
            Destroy(UIPanel);
            CancelInvoke("timer");
            MessageText.gameObject.SetActive(false);
        }
        Debug.Log("倒數中:" + time_int);
    }
    //void timer()
    //{    
    //    if (time_int == 3)
    //    {
    //        m3.SetActive(true);
    //    }
    //    else if (time_int == 2)
    //    {
    //        m3.SetActive(false);
    //        m2.SetActive(true);
    //    }
    //    else if (time_int == 1)
    //    {
    //        m2.SetActive(false);
    //        m1.SetActive(true);
    //    }
    //    else if (time_int == 0)
    //    {
    //        m1.SetActive(false);
    //        ms.SetActive(true);

    //    }
    //    else if (time_int == -1)
    //    {
    //        ms.SetActive(false); 
    //        GameStart = true; 
    //        Destroy(UIPanel);
    //        CancelInvoke("timer");
    //    }
    //    time_int -= 1;
    //}
}
