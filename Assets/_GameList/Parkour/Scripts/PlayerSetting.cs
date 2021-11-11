using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetting : MonoBehaviour
{
    public int PlayerNumPref;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Pull1;
    public GameObject Pull2;
    public GameObject Cam1;
    public GameObject Cam2;
    public Text PlayerUI;
    int temp;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerNumPref = PlayerPrefs.GetInt("PlayerNum");
        PlayerNumPref = 2;
        if(PlayerNumPref == 1)
        {
            PlayerUI.text = "Player1";
            Destroy(Player1);
            Destroy(Pull1);
        }
        if(PlayerNumPref == 2)
        {
            PlayerUI.text = "Player1\nPlayer2";
            temp = Random.Range(0, 2);
            if(temp == 0)
            {
                Cam1.SetActive(false);
            }
            if (temp == 1)
            {
                Cam2.SetActive(false);
            }
        }

    }
 
}
