using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherControll : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_allCharact;
    [SerializeField]
    GameObject m_ptObj;
    [SerializeField]
    Transform m_pitcherTransform;
    [SerializeField]
    float m_speed;

    ParticleSystem m_pt;
    ParticleSystem m_pitcherPt;
    BaseBallManage m_manage;
    Animator m_animator;
    //PlayerControll m_controll;
    PlayerControl_ScriptObject m_controll;
    Rigidbody m_pitcherRb;
    [HideInInspector]
    public int g_pointIndex;
    [HideInInspector]
    public bool g_isChoose;
    [HideInInspector]
    public GameObject g_pitcher;

    bool hasChoose = false;
    int preCharact;
    int selectChNumber = 0;

    void Start()
    {
        
        if (GameMaster_koroshi.s_GameMaster != null)
            selectChNumber = GameMaster_koroshi.s_GameMaster.p2_SelectCharacter;
        //else
        //    selectChNumber = m_controll.characterIndex;
        Debug.Log(selectChNumber);
        //preCharact = m_controll.characterIndex;
        //g_pitcher = m_allCharact[m_controll.characterIndex];
        preCharact = selectChNumber;
        g_pitcher = m_allCharact[selectChNumber];
        m_pitcherRb = g_pitcher.GetComponent<Rigidbody>();
        m_manage = BaseBallManage.instance;
        g_pitcher.SetActive(true);
        m_animator = g_pitcher.GetComponent<Animator>();
        m_pt = m_ptObj.GetComponent<ParticleSystem>();
        g_pointIndex = 1;
        m_manage.SetPitcher(g_pointIndex);
    }

    //public void SetPlayer(PlayerControll playerControll)
    public void SetPlayer(PlayerControl_ScriptObject _playerControl_ScriptObject)
    {
        m_controll = _playerControl_ScriptObject;
    }

    public void ChangePlayre()
    {
        m_allCharact[preCharact].SetActive(false);
        g_pitcher.SetActive(true);
        m_animator = g_pitcher.GetComponent<Animator>();
        m_manage = BaseBallManage.instance;
        g_pointIndex = 1;
        m_manage.SetPitcher(g_pointIndex);
        //preCharact = m_controll.characterIndex;
        preCharact = selectChNumber;
    }

    public void ResetPlayer()
    {
        StopAllCoroutines();
        m_pitcherRb.velocity = Vector3.zero;
        m_pitcherRb.useGravity = false;
        g_pitcher.transform.position = m_pitcherTransform.position;
        g_pitcher.transform.rotation = m_pitcherTransform.rotation;
        g_pointIndex = 1;
        m_pitcherPt.Stop();
        m_manage.SetPitcher(g_pointIndex);
        hasChoose = false;
    }


    // Update is called once per frame
    void Update()
    { 
        
        //if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Left))
        if (PlayerControl_koroshi.KeyDown(m_controll.keyLeft))
        {
            if (!hasChoose)
            {
                g_pointIndex = 0;
                m_manage.SetPitcher(g_pointIndex);
                if (g_isChoose)
                {
                    g_isChoose = false;
                    m_manage.StartShot();
                    hasChoose = true;
                }
            }
        }
        //if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Down))
        if (PlayerControl_koroshi.KeyDown(m_controll.keyDown))
        {
                if (!hasChoose)
            {
                g_pointIndex = 1;
                m_manage.SetPitcher(g_pointIndex);
                if (g_isChoose)
                {
                    g_isChoose = false;
                    m_manage.StartShot();
                    hasChoose = true;
                }
            }
        }
        //if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2, PlayerKeyName.Right))
        if (PlayerControl_koroshi.KeyDown(m_controll.keyRight))
        {
            if (!hasChoose)
            {

                g_pointIndex = 2;
                m_manage.SetPitcher(g_pointIndex);
                if (g_isChoose)
                {
                    g_isChoose = false;
                    m_manage.StartShot();
                    hasChoose = true;
                }
            }
        }

    }
    public void Hit()
    {
        g_pitcher.transform.rotation = Quaternion.Euler(45f,0f,0f);
        m_pitcherRb.velocity = Vector3.forward * 30 + Vector3.up * 15;
    }

    public void OnFire(Vector3 target)
    {
        StartCoroutine(Fire(target));
    }

    IEnumerator Fire(Vector3 target)
    {
        m_pitcherPt = g_pitcher.GetComponentInChildren<ParticleSystem>();
        m_pitcherPt.Play();
        m_pt.Play();
        g_pitcher.transform.position = m_ptObj.transform.position;
        g_pitcher.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        while (Mathf.Abs(g_pitcher.transform.position.x - target.x) > .1f || Mathf.Abs(g_pitcher.transform.position.z - target.z) > .1f)
        {
            g_pitcher.transform.position = Vector3.MoveTowards(g_pitcher.transform.position, target, m_speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        bool _isPitcherWin = !m_manage.Judgement(target);
        if (_isPitcherWin)
        {
            target.z -= 10;
            while (Mathf.Abs(g_pitcher.transform.position.x - target.x) > .1f || Mathf.Abs(g_pitcher.transform.position.z - target.z) > .1f)
            {
                g_pitcher.transform.position = Vector3.MoveTowards(g_pitcher.transform.position, target, m_speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
           m_pitcherRb.useGravity = true;
        }
    }
}
