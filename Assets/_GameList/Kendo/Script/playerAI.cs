using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerAI : MonoBehaviour
{
    KandoGameManage m_kandomanage;
    public int speed;
    public float timer_i = 0;
    public bool state = false;
    Rigidbody rb;
    AudioSource m_as;

    [SerializeField]
    AudioClip[] m_ac;
    [SerializeField]
    GameObject m_Ptobj, m_power, m_rotatePtobj;
    [SerializeField]
    Image power_image;//能量條
    [SerializeField]
    GameObject m_jumpptobj;
    public ParticleSystem m_pt, m_rotatePt, m_topPt;
    ParticleSystem m_jumppt;

    Animator animator;
    public int delay;
    public Transform Reposition;
    public bool stop;
    bool grounded;
    public int jumpcount = 0;
    bool isHit;
    //fired
    private bool fired = false;
    private bool rotate = false;
    public bool up = true;
    private float minForce = 10;
    private float maxForce = 50;
    //private float forceSpeed = 5;
    [SerializeField]
    float forceSpeed = 0.5f;
    private float crtForce = 1;
    [SerializeField]
    private GameObject Charact;
    public GameObject Dir;

    [Header("Input")]
    //public KandoPYControl m_playercontroll;
    public PlayerControl_ScriptObject m_playercontroll;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_kandomanage = KandoGameManage.instance;
        animator = Charact.GetComponent<Animator>();
        m_as = Charact.GetComponent<AudioSource>();
        m_pt = m_Ptobj.GetComponent<ParticleSystem>();
        m_jumppt = m_jumpptobj.GetComponent<ParticleSystem>();
        m_rotatePt = m_rotatePtobj.GetComponent<ParticleSystem>();
        stop = true;
        isHit = false;
        grounded = true;
        state = false;
    }

    private void FixedUpdate()
    {
        ////重置Rigidbody
        //Vector3 _velocity = rb.velocity;
        //rb.velocity = new Vector3(_velocity.x, 0, _velocity.z);//鎖死Y軸
    }
    void Update()
    {
        control();
        move();
    }

    public void move()
    {
        if (m_kandomanage.isStart)
        {
            if (transform.position.y < 8)
            {
                //初始化數值玩家比較好操控
                if (PlayerControl_koroshi.KeyDown(m_playercontroll.keyConfirm))
                {
                    power_image.fillAmount = 0;
                    up = true;
                }
                //power_image.fillAmount
                if (PlayerControl_koroshi.Key(m_playercontroll.keyConfirm))
                {
                    if (!fired)
                    {
                        //Debug.Log(crtForce);                  
                        rotate = true;
                        if (up)
                        {
                            //crtForce += forceSpeed * Time.deltaTime;
                            //Vector3 powerScale = new Vector3(m_power.transform.localScale.x, m_power.transform.localScale.y, m_power.transform.localScale.z * 0.9f + forceSpeed * Time.deltaTime);

                            //m_power.transform.localScale += Vector3.forward*Time.deltaTime*forceSpeed;
                            power_image.fillAmount += Time.deltaTime * forceSpeed;
                        }
                        else if (!up)
                        {
                            //crtForce -= forceSpeed * Time.deltaTime;
                            //Vector3 powerScale = new Vector3(m_power.transform.localScale.x, m_power.transform.localScale.y, m_power.transform.localScale.z * 0.9f - forceSpeed  * Time.deltaTime);

                            //m_power.transform.localScale -= Vector3.forward * Time.deltaTime * forceSpeed;
                            power_image.fillAmount -= Time.deltaTime * forceSpeed;

                        }
                        //if (m_power.transform.localScale.z > 2f&&up )
                        //{
                        //    up = false;
                        //}
                        //else if (m_power.transform.localScale.z < 0.1f&&!up)
                        //{
                        //    up = true;
                        //}
                        float _powerValue = power_image.fillAmount;
                        if (_powerValue >= 1) { up = false; }
                        else if (_powerValue <= 0) { up = true; }
                    }
                }
                if(PlayerControl_koroshi.Key(m_playercontroll.keyLeft))
                {
                    Debug.Log("test");
                    Dir.gameObject.transform.Rotate(new Vector3(0f, -5f, 0f));

                }
                if(PlayerControl_koroshi.Key(m_playercontroll.keyRight))
                {
                    Dir.gameObject.transform.Rotate(new Vector3(0f, 5f, 0f));
                }

                if(PlayerControl_koroshi.KeyUp(m_playercontroll.keyConfirm))
                {
                    StartCoroutine(Fire());
                    //transform.position += transform.forward * Time.deltaTime * 10;
                }
            }
            else
            {
                Debug.Log("撞到天花板");
                //rb.velocity = -transform.up * 10f;
                //重置高度
                Vector3 _pos = transform.localPosition;
                transform.localPosition = new Vector3(_pos.x, Reposition.localPosition.y, _pos.z);
                rb.velocity = Vector3.zero;
                m_topPt.Play();
            }
        }
    }


    public float PowerCorrectionValue = 20;
    IEnumerator Fire()
    {
        //rb.velocity =  * crtForce;
        //rb.velocity = Dir.transform.forward *m_power.transform.localScale.z * 20f;
        rb.velocity = Dir.transform.forward * power_image.fillAmount * PowerCorrectionValue;
        fired = true;
        //crtForce = minForce;
        yield return new WaitForSeconds(.5f);
        fired = false;
        rotate = false;
        Vector3 powerScale = new Vector3(0.2f, 0.1f,0.2f);
        m_power.transform.localScale = powerScale;
    }



    void control()
    {
        if (Time.deltaTime < 0.1f)
        {
            if (!stop)
            {
                if (rotate == false)
                {
                    if (state == false)
                    {
                        Charact.transform.Rotate(0, -15, 0); ;
                        animator.SetTrigger("GameGO");
                        m_rotatePt.Play();
                    }
                }
            }
        }
    }
    void jump()
    {
        if (grounded)
        {
            //animator.SetBool("Jump", true);
            rb.velocity += new Vector3(0, 5, 0); //添加加速度
            rb.velocity += transform.forward * Time.deltaTime * 30; ; //添加加速度
            m_as.clip = m_ac[1];
            m_as.Play();
            grounded = false;
        }
    }
    public void DefaultAni()
    {
        animator.SetTrigger("Idel");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ice")
        {
            crtForce = maxForce;
            Debug.Log("in ice");
        }

        //if (other.tag == "enemy")
        //{
        //    if (!isHit)
        //    {
        //        isHit = true;
        //        Quaternion OriRotate = transform.rotation;
        //        transform.LookAt(other.gameObject.transform.root.transform);
        //        m_Ptobj.transform.LookAt(other.gameObject.transform.root.transform);
        //        m_pt.Play();
        //        m_as.clip = m_ac[3];
        //        m_as.Play();
        //        animator.SetTrigger("back");
        //        Vector3 direction = transform.position - other.transform.root.position;
        //        while (Mathf.Abs(direction.x) + Mathf.Abs(direction.z) < 3)
        //        {
        //            direction *= 2;
        //        }
        //        Debug.Log(direction);
        //        rb.velocity = new Vector3(direction.x, 0, direction.z) * 3;
        //        StartCoroutine(HitTime(OriRotate));

        //    }

        //}
        if (other.tag == "die")
        {
            if (!stop)
            {
                Debug.Log(other.name);
                StartCoroutine(Dead());
            }
            else if (!grounded)
            {
                StartCoroutine(Dead());
            }

        }
    }
    IEnumerator HitTime(Quaternion oriRotate)
    {
        yield return new WaitForSeconds(.5f);
        transform.rotation = oriRotate;
        rb.velocity = Vector3.zero;
        isHit = false;
        m_pt.Stop();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!grounded)
        {
            if (collision.gameObject.tag == "ground")
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 3);
                foreach (Collider item in colliders)
                {
                    if (item.gameObject.tag == "player" && item.gameObject.name != gameObject.name)
                    {
                        Debug.Log(item);
                        Vector3 direction = item.transform.root.position - transform.position;
                        item.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(direction.x, 0, direction.z) * 3;
                        m_jumppt.Play();
                        m_as.clip = m_ac[2];
                        m_as.Play();
                    }
                }
                Debug.Log("ground");
                rb.velocity = Vector3.zero;
                grounded = true;
                jumpcount -= 1;
                if (jumpcount == 0)
                {
                    stop = false;
                }
            }
        }
        //animator.SetBool("Jump", false);


    }


    IEnumerator Dead()
    {
        Dir.transform.localEulerAngles = Vector3.zero;//重生時，重置角色旋轉
        m_rotatePtobj.SetActive(false);//重生時，暫時關閉旋轉
        power_image.fillAmount = 0;//重生時，重置能量條
        //rb.velocity = Vector3.zero;//重置Rigidbody
        Debug.Log("die");
        isHit = true;
        state = false;
        m_as.clip = m_ac[4];
        m_as.Play();
        animator.SetTrigger("DOWN");
        stop = true;
        grounded = true;
        m_kandomanage.Score(m_playercontroll.name);
        yield return new WaitForSeconds(1);
        transform.position = Reposition.position;
        transform.rotation = Reposition.rotation;
        m_rotatePtobj.SetActive(false);
        yield return new WaitForSeconds(.1f);
        m_rotatePtobj.SetActive(true);
        rb.velocity = Vector3.zero;
        stop = false;
        isHit = false;
    }
    void time()
    {
        timer_i += Time.deltaTime;
        if (timer_i >= 5)
        {
            state = false;
            timer_i = 0;
        }
    }

}
