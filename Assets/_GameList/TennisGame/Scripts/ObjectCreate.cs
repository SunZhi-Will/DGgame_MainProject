using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

   //target = new Vector3(UnityEngine.Random.Range(-60, -12f), 0f, UnityEngine.Random.Range(-66, -80f));
   //             target = new Vector3(UnityEngine.Random.Range(10f, 56), 0f, UnityEngine.Random.Range(-66, -80f));
public class ObjectCreate : MonoBehaviour
{
    [Header("顯示目前按鍵提示")]
    public GameObject[] KeyTip_array_obj;

    [System.Serializable]
    public class RandomTargetPosRangeSetup
    {
        public Transform miniX, maxX;
        public Transform miniZ, maxZ;
    }
    public RandomTargetPosRangeSetup LefrRandom_tra = new RandomTargetPosRangeSetup();
    public RandomTargetPosRangeSetup RightRandom_tra = new RandomTargetPosRangeSetup();


    public static ObjectCreate instance;
    float time1,time2;
    Vector3 moveDirection;
    public GameObject[] ball,p_b;
    public Animator _throw,boss;
    public GameObject UI_start,UI_gameTime, UI_Description, UI_Game;
    bool leftServe = true;
    public float throw_speed, timeLess=0.02f;
    public Vector3 target;
    public bool IsSwing_1 = false, IsSwing_2 = false, isChange, player1IsLeft, Stop;
    public int swing1 = 0, swing2 = 3, ball_position=0,time_change=60,time_FullLess=45,x;
    public Text startTime_text, gameTime_text;
    public int start_time = 10,game_time;
    public ParticleSystem shot,swing_1,swing_2;
    public ParticleSystem[] loc;
    public AudioSource swing,A_shot,A_Eat,A_cutdownTime;
    ObjectController OC;
    ObjectPool OP;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    void Start()
    {
        throw_speed = 2;
        OP = ObjectPool.instance;
        OC = ObjectController.instance;
        player1IsLeft = true;
        isChange = false;
        p_b = GameObject.FindGameObjectsWithTag("pb");
        gameTime_text.text =(game_time / 60).ToString() + "：" + (game_time - ((game_time / 60) * 60)).ToString("#00");
    }
    private float time;
    // Update is called once per frame
    void Update()
    {
        if (OP == null) { return; }
        if (p_b.Length <= 0) { return; }
        if (p_b[0].transform.position.y ==0) {

            //if (Input.GetKeyDown(KeyCode.C))
            if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1,PlayerKeyName.Confirm))
            {
                IsSwing_1 = true;
                time1 = 0;
                if (player1IsLeft)
                {
                    swing1 = 0;
                }
                else
                {
                    swing1 = 2;
                }
            }
            //else if (Input.GetKeyDown(KeyCode.V))
            else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1,PlayerKeyName.Cancel))
            {
                IsSwing_1 = true;
                time1 = 0;
                if (player1IsLeft)
                {
                    swing1 = 1;
                }
                else
                {
                    swing1 = 3;
                }
            }
            //if (Input.GetKeyDown(KeyCode.O))
            else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2,PlayerKeyName.Confirm))
            {
                IsSwing_2 = true;
                time2 = 0;
                if (!player1IsLeft)
                {
                    swing2 = 0;
                }
                else
                {
                    swing2 = 2;
                }
            }
            //else if (Input.GetKeyDown(KeyCode.P))
            else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2,PlayerKeyName.Cancel))
            {
                IsSwing_2 = true;
                time2 = 0;
                if (!player1IsLeft)
                {
                    swing2 = 1;
                }
                else
                {
                    swing2 = 3;
                }
            }
        }
        throw_speed = Mathf.Clamp(throw_speed, 0.8f, 2.5f);

        time1 += .02f;
        time2 += .02f;
        time1 = Mathf.Clamp(time1, 0f, 2f);
        time2 = Mathf.Clamp(time1, 0f, 2f);
        if (time1 == 2f)
        {
            IsSwing_1 = false;
            time1 = 0;
        }
        if (time2 == 2f)
        {
            IsSwing_2 = false;
            time2 = 0;
        }


    }
    IEnumerator Create() {

        if (!isChange) {
            _throw.SetTrigger("throw");
            A_shot.Play();
            shot.Play();
            //_throw.SetBool("Throw",true);
            ball_position = UnityEngine.Random.Range(0, OP.bulletList.Count-1);
            if (leftServe)
            {
                //target = new Vector3(UnityEngine.Random.Range(-60, -12f), 0f, UnityEngine.Random.Range(-66, -80f));
                target = new Vector3(UnityEngine.Random.Range(LefrRandom_tra.miniX.position.x, LefrRandom_tra.maxX.position.x), 0f,
                    UnityEngine.Random.Range(LefrRandom_tra.miniZ.position.z, LefrRandom_tra.maxZ.position.z));
                leftServe = !leftServe;
                x = 0;
            }
            else
            {
                //target = new Vector3(UnityEngine.Random.Range(10f, 56), 0f, UnityEngine.Random.Range(-66, -80f));
                target = new Vector3(UnityEngine.Random.Range(RightRandom_tra.miniX.position.x, RightRandom_tra.maxX.position.x), 0f,
                   UnityEngine.Random.Range(RightRandom_tra.miniZ.position.z, RightRandom_tra.maxZ.position.z));
                leftServe = !leftServe;
                x = 1;
            }
            //Instantiate(ball[ball_position], transform.position, transform.rotation);
            yield return new WaitForSeconds(0.1f);
            OP.GetBullet(ball_position);
        }
        if (!Stop) {
            foreach (GameObject ball in OP.bulletList)
            {
                if (ball.activeSelf)
                {
                    Stop = false;
                    //break;
                }
                else
                {
                    Stop = true;
                }
            }
        }

        if (OP.energy.fillAmount >= 0.5f)
        {
            throw_speed = 2f;
        }
        else {
            throw_speed = (2.5f - (float.Parse( (0.5f-OP.energy.fillAmount).ToString("#0.0")) )*3) - 0.5f;
        }
        yield return new WaitForSeconds(throw_speed);
        if (game_time == 0 || OP.energy.fillAmount==1)
        {
            StopCoroutine(Create());
        }
        else {
            StartCoroutine(Create());
        }
        



    }
    int st = 0;
    IEnumerator GameStart()
    {
        UI_start.SetActive(true);

        startTime_text.text = start_time.ToString();
        start_time--;
        yield return new WaitForSeconds(1);
        if (start_time<0)
        {
            StopCoroutine(GameStart());
            start_time = 3;
            UI_start.SetActive(false);
            StartCoroutine(Create());
            StartCoroutine(GameTime());
        }
        else
        {
            StartCoroutine(GameStart());
        }

    }

    float waitTime = 1;
    IEnumerator GameTime()
    {
        game_time--;
        UI_gameTime.SetActive(true);
        gameTime_text.text = (game_time / 60).ToString()+"："+(game_time - ((game_time / 60)*60)).ToString("#00");

        if (game_time == 0 || OP.energy.fillAmount == 1)
        {
            if (OP.energy.fillAmount * 100 >= 85)
            {

                yield return new WaitForSeconds(0.5f);
                boss.SetTrigger("Win");
            }
            else {
                yield return new WaitForSeconds(1f);
                boss.SetTrigger("Lose");
            }
            //StopCoroutine(Create());
            StopCoroutine(GameTime());
            
        }
        else
        {
            if (game_time% time_FullLess == 0)
            {
                OP.energy.fillAmount -= timeLess;
                OP.triangle.rectTransform.anchoredPosition = new Vector2(-710 + (OP.energy.fillAmount / 0.1f * 142.8f), 33);
            }
            if (game_time % time_change == 0 )
            {
                bool check=true;
                while(check){
                    if (Stop)
                    {
                        boss.SetTrigger("Jump");
                        yield return new WaitForSeconds(2f);
                        //isChange = true;
                        player1IsLeft = !player1IsLeft;
                        Stop = false;
                        check = false;
                        waitTime = 3f;
                        KeyTipDisplay(player1IsLeft);
                    }
                }
            }
            
            
            yield return new WaitForSeconds(waitTime);
            

            waitTime = 1;
            StartCoroutine(GameTime());
        }
        
    }
    void KeyTipDisplay(bool _enabled)
    {
        int _openNubmer = 0, _closeNumber = 1;
        switch(_enabled)
        {
            case true:
                _openNubmer = 1;
                _closeNumber = 0;
                break;
            case false:
                _openNubmer = 0;
                _closeNumber = 1;
                break;
        }
        KeyTip_array_obj[_openNubmer].SetActive(false);//將原本的關閉
        KeyTip_array_obj[_closeNumber].SetActive(true);//開啟另一邊
        Debug.Log("互換囉!");
    }

    public void playGame() {
        StartCoroutine(GameStart());
        A_cutdownTime.Play();
        UI_Description.SetActive(false);
        UI_Game.SetActive(true);
    }

    
}
