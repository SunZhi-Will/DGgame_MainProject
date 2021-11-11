using System.Collections;
using UnityEngine;

public class BatterControll : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_allCharact;
    [SerializeField]
    ParticleSystem[] m_pt;

    BaseBallManage m_manage;
    Animator m_animator;
    //PlayerControll m_controll;
    PlayerControl_ScriptObject m_controll;
    Rigidbody m_rb;
    [HideInInspector]
    public int g_pointIndex, g_batterTarget;
    [HideInInspector]
    public bool g_isChoose;

    int preCharact;
    bool isMirror;
    int selectChNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (GameMaster_koroshi.s_GameMaster != null)
            selectChNumber = GameMaster_koroshi.s_GameMaster.p1_SelectCharacter;
        //else
        //    selectChNumber = m_controll.characterIndex;
        Debug.Log(selectChNumber);
        //m_allCharact[m_controll.characterIndex].SetActive(true);
        //m_animator = m_allCharact[m_controll.characterIndex].GetComponent<Animator>();
        m_allCharact[selectChNumber].SetActive(true);
        m_animator = m_allCharact[selectChNumber].GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
        preCharact = selectChNumber;
        m_manage = BaseBallManage.instance;
        g_pointIndex = 1;
        g_batterTarget = g_pointIndex + 1;
        isMirror = false;
        m_manage.SetBatter(g_pointIndex);
    }

    //public void SetPlayer(PlayerControll playerControll)
    public void SetPlayer(PlayerControl_ScriptObject playerControll)
    {
        m_controll = playerControll;
    }
    public void ChangePlayre()
    {
        m_allCharact[preCharact].SetActive(false);
        //m_allCharact[m_controll.characterIndex].SetActive(true);
        //m_animator = m_allCharact[m_controll.characterIndex].GetComponent<Animator>();
        m_allCharact[selectChNumber].SetActive(true);
        m_animator = m_allCharact[selectChNumber].GetComponent<Animator>();
        m_manage = BaseBallManage.instance;
        g_pointIndex = 1;
        isMirror = false;
        m_animator.SetBool("Mirror", isMirror);
        m_manage.SetBatter(g_pointIndex);
        //preCharact = m_controll.characterIndex;
        preCharact = selectChNumber;
    }

    public void ResetPlayer()
    {
        StopAllCoroutines();
        m_rb.velocity = Vector3.zero;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        isMirror = false;
        m_animator.SetBool("Mirror", isMirror);
        g_pointIndex = 1;
        g_batterTarget = g_pointIndex + 1;
        m_manage.SetBatter(g_pointIndex);
    }
    public void Hit()
    {
        m_animator.SetTrigger("Batting");
        for (int i = 0; i < m_pt.Length; i++)
        {
            m_pt[i].Play();
        }
    }


    public void OnDrop()
    {
        StartCoroutine(Drop());
    }
    IEnumerator Drop()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        isMirror = false;
        m_animator.SetBool("Mirror", isMirror);
        transform.Rotate(0, -90, 0);
        m_animator.SetTrigger("Drop");
        yield return new WaitForSeconds(.3f);
        m_rb.velocity = -(Vector3.forward * 13);
    }

    // Update is called once per frame
    void Update()
    {
        if (g_isChoose)
        {
            Control();
            //if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Left))
            //{
            //    if (g_pointIndex > 0)
            //    {
            //        g_pointIndex -= 1;
            //        if (isMirror)
            //            g_batterTarget = g_pointIndex - 1;
            //        else
            //            g_batterTarget = g_pointIndex + 1;
            //        m_manage.SetBatter(g_pointIndex);
            //    }
            //    if (g_pointIndex == 0)
            //    {
            //        g_batterTarget = g_pointIndex + 1;
            //        transform.rotation = new Quaternion(0, 0, 0, 0);
            //        isMirror = false;
            //        m_animator.SetBool("Mirror", isMirror);
            //    }
            //}
            //if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Right))
            //{
            //    if (g_pointIndex < 2)
            //    {
            //        g_pointIndex += 1;
            //        g_batterTarget = g_pointIndex + 1;
            //        m_manage.SetBatter(g_pointIndex);
            //    }
            //    if (g_pointIndex == 2)
            //    {
            //        g_batterTarget = g_pointIndex - 1;
            //        transform.rotation = new Quaternion(0, 180, 0, 0);
            //        isMirror = true;
            //        m_animator.SetBool("Mirror", isMirror);
            //    }
            //}

            //if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Down))
            //{
            //    if (g_pointIndex == 1)
            //    {
            //        transform.Rotate(0, 180, 0);
            //        if (isMirror)
            //        {
            //            g_batterTarget = g_pointIndex + 1;
            //            isMirror = false;
            //        }
            //        else
            //        {
            //            g_batterTarget = g_pointIndex - 1;
            //            isMirror = true;
            //        }
            //        m_animator.SetBool("Mirror", isMirror);
            //    }
            //}



            //if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Confirm))
            //{
            //    m_animator.SetTrigger("Batting");
            //}
        }
    }

    //控制器
    void Control()
    {
        //if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Left))
        if (PlayerControl_koroshi.KeyDown(m_controll.keyLeft))
        {
            g_pointIndex = 0;
            g_batterTarget = g_pointIndex + 1;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            m_animator.SetBool("Mirror", isMirror = false);
            m_manage.SetBatter(0);
        }
        //else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Down))
        if (PlayerControl_koroshi.KeyDown(m_controll.keyDown))
        {
            g_pointIndex = 1;
            if (isMirror)
            {
                g_batterTarget = g_pointIndex - 1;
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else

            {
                g_batterTarget = g_pointIndex + 1;
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            m_animator.SetBool("Mirror", isMirror);
            m_manage.SetBatter(1);
        }
        //else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Right))
        if (PlayerControl_koroshi.KeyDown(m_controll.keyRight))
        {
            g_pointIndex = 2;
            g_batterTarget = g_pointIndex - 1;
            transform.rotation = new Quaternion(0, 180, 0, 0);
            m_animator.SetBool("Mirror", isMirror = true);
            m_manage.SetBatter(2);
        }


        //左右旋轉
        System.Action _LefiRightAction = () =>
        {
            transform.Rotate(0, 180, 0);
            m_animator.SetBool("Mirror", isMirror);
        };
        //if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Confirm))
        if (PlayerControl_koroshi.KeyDown(m_controll.keyConfirm))
        {
            if (g_pointIndex != 1) { return; }//後面確定沒東西要跑才用
            if (!isMirror)
            {
                g_batterTarget = g_pointIndex - 1;
                isMirror = true;
                _LefiRightAction();
            }
        }
        //else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Cancel))
        if (PlayerControl_koroshi.KeyDown(m_controll.keyCancel))
        {
            if (g_pointIndex != 1) { return; }//後面確定沒東西要跑才用
            if (isMirror)
            {
                g_batterTarget = g_pointIndex + 1;
                isMirror = false;
                _LefiRightAction();
            }
        }
    }


}
