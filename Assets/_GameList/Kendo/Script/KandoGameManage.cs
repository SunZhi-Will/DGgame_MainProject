using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KandoGameManage : MonoBehaviour
{
    public GameObject Screen_MainGameUI_obj;//遊戲UI物件
    public static KandoGameManage instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    [SerializeField]
    GameObject[] m_allplayer;
    [SerializeField]
    Transform[] m_allresponsiton;
    [SerializeField]
    GameObject[] m_time;
    [SerializeField]
    GameObject[] ice;
    [SerializeField]
    Text P1Score, P2Score;
    [SerializeField]
    GameObject scenelight;
    GameData_Kendo m_manage;
    playerAI P1AI, P2AI;
    AudioSource m_as;
    public int GO_Time = 0;
    //Win_Lose
    public int P2_point, P1_point;
    public int count = 0;
    int round = 0;
    public int P1_win, P2_win;

    //time
    public int time_g;
    int time_w;
    [SerializeField]
    //private string time_Title = "Time: ";
    private string Timed_tailString = "s";//顯示秒數後墜
    public Text time;

    //ice
    public int[] array12 = new int[5];
    public int number;

    //Light
    Light S_light, P1_light, P2_light;
    Color32 defaultcolor;
    float defaultrange;
    public bool isStart;
    public int weather = 0;

    public int delay;

    public Text Message_Text;

    //設定玩家物件
    public void SetPlayerObject(GameObject _p1_obj, GameObject _p2_obj)
    {
        m_allplayer[0] = _p1_obj;
        m_allplayer[1] = _p2_obj;
    }

    // Start is called before the first frame update
    void Start()
    {
        P1_point = 0;
        P2_point = 0;
        isStart = false;
        time_w = 0;
        m_manage = FindObjectOfType<GameData_Kendo>();
        round = m_manage.round;
        m_as = GetComponent<AudioSource>();
        S_light = scenelight.GetComponent<Light>();
        P1_light = m_allplayer[0].GetComponentInChildren<Light>();
        P2_light = m_allplayer[1].GetComponentInChildren<Light>();
        P1AI = m_allplayer[0].GetComponent<playerAI>();
        P2AI = m_allplayer[1].GetComponent<playerAI>();
        defaultrange = P1_light.range;
        defaultcolor = S_light.color;
        StartGame();//Start
    }

    void StartGame()
    {
        GOtime();
        StartCoroutine(GameGo());
        time.text = time_g + Timed_tailString;
        weather = Random.Range(1,2);
        Debug.Log("weather" + weather);
    }
    IEnumerator GameGo()
    {
        //m_time[0].SetActive(true);
        //yield return new WaitForSeconds(.7f);
        //m_time[0].SetActive(false);
        //m_time[1].SetActive(true);
        //yield return new WaitForSeconds(.7f);
        //m_time[1].SetActive(false);
        //m_time[2].SetActive(true);
        //yield return new WaitForSeconds(.7f);
        //m_time[2].SetActive(false);
        //m_time[3].SetActive(true);
        //yield return new WaitForSeconds(.7f);
        //m_time[3].SetActive(false);
        Message_Text.text = "第" + (m_manage.round - round + 1) + "回合";
        Message_Text.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        m_as.Play();
        Message_Text.text = "3";
        yield return new WaitForSeconds(.7f);
        Message_Text.text = "2";
        yield return new WaitForSeconds(.7f);
        Message_Text.text = "1";
        yield return new WaitForSeconds(.7f);
        Message_Text.text = "Fight";
        yield return new WaitForSeconds(.7f);
        time.gameObject.SetActive(true);//時間倒數開啟
        Message_Text.gameObject.SetActive(false);
        isStart = true;
        PlayerSet(false);//遊戲開始
        StartCoroutine(reciprocal());
    }

    void PlayerSet(bool playerstate)
    {
        P1AI.stop = playerstate;
        P2AI.stop = playerstate;
    }
    void GOtime()
    {
        if (count == 0)
        {
            time_g = m_manage.time;
        }
        if (count == 1)
        {
            Debug.Log("分數重來");
            time_g = 30;
        }
    }

    IEnumerator reciprocal()
    {
        time_w += 1;
        time_g -= 1;
        if (time_g > 0)
        {
            time.text = time_g + Timed_tailString;
            if (time_w >= 25)
            {
                switch (weather)
                {
                    case 1:
                        Ice_wall();
                        break;
                    case 2:
                        BlackDay();
                        break;
                    case 3:
                        if (P1_point > P2_point)
                        {
                            P2AI.jumpcount += 2;
                        }
                        else if (P1_point < P2_point)
                        {
                            P1AI.jumpcount += 2;
                        }
                        else
                        {
                            P1AI.jumpcount += 1;
                            P2AI.jumpcount += 1;
                        }
                        break;
                }
                StartCoroutine(delay_time());
                time_w = 0;
            }
            yield return new WaitForSeconds(1);
            StartCoroutine(reciprocal());
        }
        //這裡進不去,結束的勝負判斷跟回合判斷
        if (time_g == 0)
        {
            Debug.Log("進入判斷");
            WIN_Lose();
        }
        if (delay >= 5)
        {
            close();
        }
    }
    void BlackDay()
    {
        RenderSettings.ambientIntensity = 0;
        S_light.color = new Color32(0, 0, 0, 255);
        P1_light.range = 300;
        P2_light.range = 300;
    }
    public void Score(string player)
    {
        if (player.Contains("Player1"))
        {
            P2_point += 1;
            //P2Score.text = "Score:" + P2_point.ToString();
            P2Score.text = P2_point.ToString();
        }
        else if (player.Contains("Player2"))
        {
            P1_point += 1;
            //P1Score.text = "Score:" + P1_point.ToString();
            P1Score.text = P1_point.ToString();
        }
        //switch (player)
        //{
        //    case "P2":
        //        P1_point += 1;
        //        P1Score.text = "Score:" + P1_point.ToString();
        //        break;
        //    case "P1":
        //        P2_point += 1;
        //        P2Score.text = "Score:" + P2_point.ToString();
        //        break;
        //}
    }


    public void Ice_wall()
    {
        for (int i = 0; i < 5; i++)
        {
            number = Random.Range(0, 12);
            array12[i] = number;
            ice[number].SetActive(true);
            while (!check(number, i))
            {
                number = Random.Range(0, 12);
                array12[i] = number;
                ice[number].SetActive(true);
            }
        }
    }
    bool check(int index, int i)
    {
        for (int j = 0; j < i; j++)
        {
            if (index == array12[j])
            {
                Debug.Log(index);
                Debug.Log("重複");
                return false;
            }
        }
        return true;
    }
    public void close()
    {
        for (int c = 0; c < 5; c++)
        {
            int s = array12[c];
            ice[s].SetActive(false);
        }
        DefaultLight();
        StopCoroutine(delay_time());
        delay = 0;
    }

    void DefaultLight()
    {
        RenderSettings.ambientIntensity = 1;
        S_light.color = defaultcolor;
        P1_light.range = defaultrange;
        P2_light.range = defaultrange;
    }

    IEnumerator delay_time()
    {
        yield return new WaitForSeconds(1);
        delay += 1;
        StartCoroutine(delay_time());
    }

    void WIN_Lose()
    {
        StopAllCoroutines();
        if (P2_point > P1_point)
        {
            Debug.Log("進入判斷1");
            P2_win += 1;
        }
        if (P1_point > P2_point)
        {
            P1_win += 1;
        }
        if (P2_point == P1_point)
        {
            Debug.Log("分數一樣");
            count += 1;
            if (count == 1)
            {
                Debug.Log("重新開始");
                count = 0;
            }
        }

        round -= 1;
        if (round > 0)
        {
            RestartGame();
        }
        if (round <= 0)
        {
            Debug.Log("遊戲結束");
            Screen_MainGameUI_obj.SetActive(false);
            m_manage.Common_scr.ShowSettlement(P1_point, P2_point);
        }

    }
    public void RestartGame()
    {
        m_allplayer[0].transform.position = m_allresponsiton[0].position;
        m_allplayer[0].transform.rotation = m_allresponsiton[0].rotation;
        P1AI.DefaultAni();
        m_allplayer[1].transform.position = m_allresponsiton[1].position;
        m_allplayer[1].transform.rotation = m_allresponsiton[1].rotation;
        P2AI.DefaultAni();
        PlayerSet(true);//下一回合開始準備中，玩家停止
        StartGame();//下一回合
        isStart = false;//回合重置
        Debug.Log("round" + round);
        //重啟倒數 角色位置重製
    }
}
