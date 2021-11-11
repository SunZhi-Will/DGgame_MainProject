using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

[System.Serializable]
public class BoundaryManager
{
    public float xMin, xMax;
}


public class Player2AI : MonoBehaviour
{
    [SerializeField]
    private GameData_Parkour gameData_Parkour_scr;
    [Range(0, 20)] public float jumpForce;
    [SerializeField] Transform feetPoint;
    [SerializeField] float checkRadius;
    [SerializeField] Animator animator;

    public GameObject Cover;
    public GameObject time_UI;
    private CountDown countDown_scr;//存起來比較省資源
    /*public GameObject PullBox;*/
    public float PlayerSpeed;
    public float InitialSpeed;
    private Rigidbody rb;
    public bool IsFloor;
    bool Move_close;
    public BoundaryManager2 boundaryManager;

    //sound
    public AudioClip jump;
    public AudioClip powerup;
    public AudioClip blip;
    public AudioClip water;
    public AudioClip step;
    AudioSource audiosource;

    //public GameObject Boss;
    public BossAI bossAI_scr;
    private Transform boss_tra;
    private Vector3 v_move;
    private float v_moveXpos, v_moveZpos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        animator.SetBool("Running", false);
        countDown_scr = time_UI.GetComponent<CountDown>();

        boss_tra = bossAI_scr.transform;
        InitialSpeed = gameData_Parkour_scr.data.Original_InitialSpeedSpeed;//初始化速度
    }

    void Update()
    {
        IsFloor = Physics.CheckSphere(feetPoint.position, checkRadius);
        //if (time_UI.GetComponent<CountDown>().GameStart ==true)//********** 
        if (countDown_scr.GameStart == true)//**************************************
        {
            animator.SetBool("Running", false);
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, boundaryManager.xMin, boundaryManager.xMax), rb.position.y, rb.position.z);
            Jumping();
            Moving();
            Pushing();
        }
        //PlayerSpeed = InitialSpeed + transform.position.z/50;
        //快被Boss追到的時候加快一些
        float _bossDistance = Vector3.Distance(boss_tra.localPosition, transform.position);
        float _lerpSpeed = gameData_Parkour_scr.data.LerpSpeed;
        if (_bossDistance < 12)
            //PlayerSpeed = InitialSpeed * 1.5f;
            //PlayerSpeed = InitialSpeed * (_bossDistance * gameData_Parkour_scr.SpecialSpeed);
            PlayerSpeed = Mathf.Lerp(PlayerSpeed, InitialSpeed * (_bossDistance * gameData_Parkour_scr.data.SpecialSpeed), _lerpSpeed);
        else
            PlayerSpeed = Mathf.Lerp(PlayerSpeed, InitialSpeed, _lerpSpeed);
    }

    void FixedUpdate()
    {
        if (countDown_scr.GameStart == true)
        {
            //transform.Translate(v_move);
            rb.velocity += v_move;
        }
    }
    void Moving()
    {
        if (PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Right) && rb.position.x > boundaryManager.xMin)
        {
            animator.SetBool("Running", true);
            v_moveXpos = Vector3.left.x * PlayerSpeed * Time.deltaTime;
        }
        else if (PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Left) && rb.position.x < boundaryManager.xMax)
        {
            animator.SetBool("Running", true);
            v_moveXpos = Vector3.right.x * PlayerSpeed * Time.deltaTime;
        }
        else
        {
            v_moveXpos = 0;
        }
        //animator.SetBool("Running", true);
        //v_moveZpos = Vector3.forward.z * PlayerSpeed * Time.deltaTime;
        if (PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Down))
        {
            animator.SetBool("Running", true);
            v_moveZpos = Vector3.forward.z * PlayerSpeed * Time.deltaTime;
        }
        else if (PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Up))
        {
            animator.SetBool("Running", true);
            v_moveZpos = Vector3.back.z * PlayerSpeed * Time.deltaTime;
        }
        else
        {
            v_moveZpos = 0;
        }
        v_move = new Vector3(v_moveXpos, 0, v_moveZpos);
    }
    //void Moving()
    //{
    //    //if (Input.GetKey(KeyCode.D) && rb.position.x > boundaryManager.xMin)
    //    if (PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Right) && rb.position.x > boundaryManager.xMin)
    //    {
    //        /*audiosource.enabled = true;*/
    //        animator.SetBool("Running", true);
    //        transform.Translate(Vector3.left * PlayerSpeed * Time.deltaTime + Vector3.forward * PlayerSpeed * Time.deltaTime);
    //    }
    //    //else if (Input.GetKey(KeyCode.A) && rb.position.x < boundaryManager.xMax)
    //    if(PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Left) && rb.position.x < boundaryManager.xMax)
    //    {
    //        animator.SetBool("Running", true);
    //        transform.Translate(Vector3.right * PlayerSpeed * Time.deltaTime + Vector3.forward * PlayerSpeed * Time.deltaTime);
    //    }
    //    //else if (Input.GetKey(KeyCode.S))
    //    else if(PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Down))
    //    {
    //        animator.SetBool("Running", true);
    //        transform.Translate(Vector3.forward * PlayerSpeed * Time.deltaTime);
    //    }
    //    //else if (Input.GetKey(KeyCode.W))
    //    else if(PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Up))
    //    {
    //        animator.SetBool("Running", true);
    //        transform.Translate(Vector3.back * PlayerSpeed * Time.deltaTime);
    //    }
    //}
    void Jumping()
    {
        ////if (IsFloor && Input.GetKeyDown(KeyCode.X))
        ////if (IsFloor && PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Cancel))
        //if (IsFloor && PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Cancel))
        //{
        //    audiosource.PlayOneShot(jump);
        //    Debug.Log("跳躍...");
        //    rb.velocity += Vector3.up * jumpForce;
        //    IsFloor = false;
        //}
    }
    void Pushing()
    {
        //if (Input.GetKey(KeyCode.E) && InitialSpeed == 1)
        if (PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Confirm) && InitialSpeed == 1)
        {
            animator.SetBool("Pull", true);
        }
        //else if (Input.GetKey(KeyCode.E))
        if (PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Confirm))
        {
            animator.SetBool("Push", true);
        }
        else
        {
            animator.SetBool("Push", false);
            animator.SetBool("Pull", false);
        }
    }
    void Slow()
    {
        //InitialSpeed = 1;
        InitialSpeed = gameData_Parkour_scr.data.Slow;
        jumpForce = 0;
    }
    void Fast()
    {
        //PlayerSpeed += 2;
        InitialSpeed += gameData_Parkour_scr.data.Fast;
        Invoke("SpeedBack", 1);
    }
    void SpeedBack()
    {
        //PlayerSpeed -= 2;
        InitialSpeed -= gameData_Parkour_scr.data.Fast;
    }
    void Blind()
    {
        //if (Cover.transform.position.y == -125)
        //{
        //    Cover.transform.position += new Vector3(0, 250, 0);
        //}
        //Invoke("CoverBack", 10f);//別用廣播~很難找
        gameData_Parkour_scr.CoverBack();
    }
    //void CoverBack()
    //{
    //    Cover.transform.position -= new Vector3(0, 250, 0);
    //}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boss")
        {
            gameObject.SetActive(false);
        }
        else if (other.tag == "Slow")
        {
            Debug.Log("慢...");
            Slow();
            audiosource.clip = water;
        }
        else if (other.tag == "Blind")
        {
            audiosource.PlayOneShot(blip);
            Destroy(other.gameObject);
            Debug.Log("致盲...");
            Blind();
        }
        else if (other.tag == "Fast")
        {
            audiosource.PlayOneShot(powerup);
            Destroy(other.gameObject);
            Fast();
        }
        else if (other.tag == "Win")
        {
            audiosource.Stop();
            animator.SetBool("Win", true);
            PlayerPrefs.SetInt("WinOrLose", 1);
            //Invoke("EndSence", 10f);
            //Boss.GetComponent<BossAI>().PlayerWin();
            bossAI_scr.PlayerWin();
            //GameObject.Find("Player2").GetComponent<Player2AI>().enabled = false;
            enabled = false;//繼承MonoBehaviour腳本有內建enabled參數可以關閉自身腳本
            gameData_Parkour_scr.ShowSettlement(true);
            gameData_Parkour_scr.ClosePlayerControl();//關閉玩家控制器2P
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Slow")
        {
            audiosource.clip = step;
            //InitialSpeed = 4;
            InitialSpeed = gameData_Parkour_scr.data.Original_InitialSpeedSpeed;
            jumpForce = 7;
        }
    }
    void WalkSound()
    {
        if (audiosource == null) { return; }
        audiosource.Play();
    }
    void WalkSoundStop()
    {
        if (audiosource == null) { return; }
        audiosource.Pause();
    }
    //void EndSence()
    //{
    //    SceneManager.LoadScene("EndScene");
    //}
}
