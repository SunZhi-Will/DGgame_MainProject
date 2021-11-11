using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadAI : MonoBehaviour
{
    public Text PlayerUI;
    public GameObject Cam1;
    public GameObject Cam2;
    int PlayerAmount = 2;
    public GameData_Parkour gameData_Parkour_scr;
    void Start()
    {
        //PlayerAmount = PlayerPrefs.GetInt("PlayerNum");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1")
        {
            PlayerAmount--;
            PlayerPrefs.SetInt("PlayerNum", PlayerAmount);
            if (PlayerAmount == 1)
            {
                PlayerUI.text = "Dead\nPlayer2";
                Cam2.SetActive(true);

            }
            else if (PlayerAmount <= 0)
            {
                //SceneManager.LoadScene("EndScene");
                gameData_Parkour_scr.ShowSettlement(false);
            }
        }
        else if (other.tag == "Player2")
        {
            PlayerAmount--;
            PlayerPrefs.SetInt("PlayerNum", PlayerAmount);
            if (PlayerAmount == 1)
            {
                PlayerUI.text = "Player1\nDead";
                Cam1.SetActive(true);

            }
            else if (PlayerAmount <= 0)
            {
                gameData_Parkour_scr.ShowSettlement(false);
                //SceneManager.LoadScene("EndScene");
            }
        }
    }
}

