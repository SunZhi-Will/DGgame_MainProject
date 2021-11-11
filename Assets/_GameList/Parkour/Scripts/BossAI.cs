using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    public GameData_Parkour gameData_Parkour_scr;
    public float Ground_speed;
    public float InitialSpeed=4;
    public GameObject time_UI;
    public Text PlayerUI;
    //public GameObject Cam1;
    //public GameObject Cam2;
    int PlayerAmount = 2;
    public bool PlayerNotWin = true;

    AudioSource audiosource;
    Vector3 place;
    public GameObject effect;
    public AudioClip bomb;

    private CountDown countDown_scr;

    void Start()
    {
        //PlayerAmount = PlayerPrefs.GetInt("PlayerNum");
        audiosource = GetComponent<AudioSource>();
        countDown_scr = time_UI.GetComponent<CountDown>();
    }
    void Update()
    {
        if(countDown_scr.GameStart == true && PlayerNotWin)//*************
        {
            Move();
        }
    }
    void Move()
    {
        transform.Translate(Vector3.forward * Ground_speed * Time.deltaTime);
        //Ground_speed = InitialSpeed + transform.position.z / 50;
        Ground_speed = InitialSpeed + transform.position.z / Test_Speed;
    }
    public float Test_Speed = 100;
    void OnTriggerEnter(Collider other)
    {
       if (other.tag == "Player1")
        { 
           PlayerAmount--;
           PlayerPrefs.SetInt("PlayerNum", PlayerAmount);
            if (PlayerAmount == 1)
           {    
                PlayerUI.text = "Dead\nPlayer2";
                //Cam2.SetActive(true);
           }
            else if(PlayerAmount <= 0)
            {
                gameData_Parkour_scr.ShowSettlement(false);
                //SceneManager.LoadScene("EndScene");
            }
        }
        else if (other.tag == "Player2")
        {
            PlayerAmount--;
            PlayerPrefs.SetInt("PlayerNum", PlayerAmount);
            if (PlayerAmount == 1)
            {
                PlayerUI.text = "Player1\nDead";
                //Cam1.SetActive(true);
            }
            else if (PlayerAmount <= 0)
            {
                //SceneManager.LoadScene("EndScene");
                gameData_Parkour_scr.ShowSettlement(false);
            }
        }
        else if (other.tag == "Win")
        {
            Debug.Log("輸");
            PlayerPrefs.SetInt("WinOrLose", 0);
            gameData_Parkour_scr.ShowSettlement(false);
            //SceneManager.LoadScene("EndScene");
        }

    }
   public void PlayerWin()
    {
        PlayerNotWin = false;
        place = this.transform.position;
        Instantiate(effect, place, Quaternion.Euler(new Vector3(0, -90, 0)));
        audiosource.PlayOneShot(bomb);
    }

}
