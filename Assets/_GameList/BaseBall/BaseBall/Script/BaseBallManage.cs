using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BaseBallManage : MonoBehaviour
{
    #region Sington
    public static BaseBallManage instance;
    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    public GameData_BaseBallGame gameData_scr;

    [SerializeField]
    Transform[] m_allBatterPoint;
    [SerializeField]
    GameObject[] m_allCamera, m_batterPtObj;
    [SerializeField]
    GameObject m_batter, m_pitcher, m_chooseCanvas, m_images, m_boss;
    [SerializeField]
    //PlayerControll[] m_allPlayer;
    PlayerControl_ScriptObject[] m_allPlayer;
    [SerializeField]
    Text m_timeText;
    [SerializeField]
    Transform m_pitcherCameraPoint, m_mainCameraPoint;

    Transform traceTarget;
    ParticleSystem[] m_batterPt;
    Animation m_cameraAni, m_bossAni;
    Animator m_bossAnimator;
    Vector3 pitcherTarget;
    BatterControll m_batterControll;
    PitcherControll m_pitcherControll;

    bool m_startFollow, m_isHit;
    public int Round { get { return m_round; } }
    public int p1Score { get { return P1Score; } }
    public int p2Score { get { return P2Score; } }
    int m_pitcherIndex, m_batterIndex, P1Score, P2Score, m_round, m_maxScore;
    private enum Character
    {
        BatterIndex,
        Pitcher,
    }

    void Start()
    {
        m_batterPt = new ParticleSystem[m_batterPtObj.Length];
        //m_pitcherIndex = 0;
        //m_batterIndex = 1;
        m_pitcherIndex = (int)Character.Pitcher;//比較直覺
        m_batterIndex = (int)Character.BatterIndex;
        m_round = 1;
        m_maxScore = 3;
        m_startFollow = false;
        m_batterControll = m_batter.GetComponent<BatterControll>();
        m_batterControll.SetPlayer(m_allPlayer[m_batterIndex]);
        m_pitcherControll = m_pitcher.GetComponent<PitcherControll>();
        m_pitcherControll.SetPlayer(m_allPlayer[m_pitcherIndex]);
        m_cameraAni = m_allCamera[0].GetComponent<Animation>();
        m_bossAni = m_boss.GetComponent<Animation>();
        m_bossAnimator = m_boss.GetComponentInChildren<Animator>();
        for (int i = 0; i < m_batterPtObj.Length; i++)
        {
            m_batterPt[i] = m_batterPtObj[i].GetComponent<ParticleSystem>();
        }
        m_batter.SetActive(true);
        m_pitcher.SetActive(true);
        StartCoroutine(StartGame());
    }
    public void SetBatter(int pointIndex)
    {
        m_batter.transform.position = m_allBatterPoint[pointIndex].position;
    }

    public void SetPitcher(int pointIndex)
    {
        pitcherTarget = m_allBatterPoint[pointIndex].position;
        pitcherTarget = new Vector3(pitcherTarget.x, 0, pitcherTarget.z);
        m_pitcher.transform.LookAt(pitcherTarget);
    }

    IEnumerator StartGame()
    {
        m_allCamera[0].transform.position = m_mainCameraPoint.position;
        m_allCamera[0].transform.rotation = m_mainCameraPoint.rotation;
        m_bossAni.Play();
        m_timeText.text = "Round " + m_round.ToString();
        m_batterControll.g_isChoose = true;
        yield return new WaitForSeconds(3);
        m_cameraAni.Play();
        yield return new WaitForSeconds(1);
        m_timeText.text = "Fight";
        yield return new WaitForSeconds(.8f);
        m_pitcherControll.g_isChoose = true;
        traceTarget = m_pitcherControll.g_pitcher.transform;
        m_images.SetActive(true);
        m_timeText.text = "";
    }


    public bool Judgement(Vector3 pitcherTarget)
    {
        bool isPitcherWin;
        //m_batterControll.StopAllCoroutines();
        //m_pitcherControll.StopAllCoroutines();
        if (m_batterControll.g_pointIndex == m_pitcherControll.g_pointIndex)
        {
            switch (m_pitcherIndex)
            {
                case 0:
                    P1Score += 1;
                    m_timeText.text = "P1得分";
                    break;
                case 1:
                    P2Score += 1;
                    m_timeText.text = "P2得分";
                    break;
            }
            traceTarget = m_batter.transform;
            isPitcherWin = true;
            gameData_scr.PlayShow(true);//投手獲勝
            Debug.Log("P2得分");
        }
        else if (m_batterControll.g_batterTarget == m_pitcherControll.g_pointIndex)
        {
            switch (m_batterIndex)
            {
                case 0:
                    P1Score += 1;
                    m_timeText.text = "P1得分";
                    break;
                case 1:
                    P2Score += 1;
                    m_timeText.text = "P2得分";
                    break;
            }
            m_pitcherControll.StopAllCoroutines();

            m_batterControll.Hit();
            m_pitcherControll.Hit();
            m_isHit = true;
            StartCoroutine(HitCamera(m_allCamera[1]));
            for (int i = 0; i < m_batterPtObj.Length; i++)
            {
                m_batterPtObj[i].transform.position = pitcherTarget;
                m_batterPt[i].Play();
            }
            isPitcherWin = false;
            StartCoroutine(SlowMotion());
            Debug.Log("P1得分");
            gameData_scr.PlayShow(false);//打手獲勝
        }
        else
        {

            m_timeText.text = "平手";
            isPitcherWin = false;
        }

        BossEat(pitcherTarget);
        if (P1Score == m_maxScore)
        {
            StartCoroutine(Slow());
            gameData_scr.Common_scr.ShowSettlement(P1Score, P2Score);
            Debug.Log("P1獲勝");
        }
        else if (P2Score == m_maxScore)
        {
            Debug.Log("P2獲勝");
            gameData_scr.Common_scr.ShowSettlement(P1Score, P2Score);
        }
        else
        {
            StartCoroutine(ResetGame(2));
        }
        return isPitcherWin;
    }
    IEnumerator Slow()
    {
        Debug.Log("減速");
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1;
    }
    IEnumerator SlowMotion()
    {
        Time.timeScale = .3f;
        m_startFollow = false;
        while (Time.timeScale < 1)
        {
            Time.timeScale += .1f;
            yield return new WaitForSeconds(.1f);
        }
        Time.timeScale = 1;
    }
    public void StartShot()
    {
        StartCoroutine(StartCount());
    }
    IEnumerator StartCount()
    {
        m_images.SetActive(false);
        m_timeText.text = "3";
        yield return new WaitForSeconds(1);
        m_timeText.text = "2";
        yield return new WaitForSeconds(1);
        m_timeText.text = "1";
        yield return new WaitForSeconds(1);
        m_timeText.text = "";
        m_batterControll.g_isChoose = false;
        m_pitcherControll.OnFire(pitcherTarget);
        m_allCamera[0].SetActive(false);
        m_allCamera[2].SetActive(true);
        yield return new WaitForSeconds(1);
        m_allCamera[2].SetActive(false);
        m_allCamera[1].SetActive(true);
        m_startFollow = true;
        StartCoroutine(CameraFollow(m_allCamera[1]));
    }

    public void ChangePlayer()
    {
        switch (m_pitcherIndex)
        {
            case 0:
                m_pitcherIndex = 1;
                m_batterIndex = 0;
                m_batterControll.SetPlayer(m_allPlayer[m_batterIndex]);
                m_pitcherControll.SetPlayer(m_allPlayer[m_pitcherIndex]);
                break;
            case 1:
                m_pitcherIndex = 0;
                m_batterIndex = 1;
                m_batterControll.SetPlayer(m_allPlayer[m_batterIndex]);
                m_pitcherControll.SetPlayer(m_allPlayer[m_pitcherIndex]);
                break;
        }
    }
    IEnumerator HitCamera(GameObject camera)
    {
        while (m_isHit)
        {
            Debug.Log(camera);
            Vector3 HitCameraPoint = m_batter.transform.position;
            HitCameraPoint.x += 4;
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, HitCameraPoint, 8.5f * Time.deltaTime);
            camera.transform.LookAt(traceTarget);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CameraFollow(GameObject camera)
    {
        while (m_startFollow)
        {
            Vector3 pitcherCamera = traceTarget.position;
            pitcherCamera.y += 4;
            pitcherCamera.x += 4;
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, pitcherCamera, 8.5f * Time.deltaTime);
            camera.transform.LookAt(traceTarget);
            yield return new WaitForEndOfFrame();
        }
    }

    public void BossEat(Vector3 target)
    {
        target.z -= 10;
        target.y = m_boss.transform.position.y;
        m_boss.transform.position = target;
        m_bossAnimator.SetTrigger("Jump");
    }
    IEnumerator ResetGame(int time)
    {
        m_round += 1;
        //ChangePlayer();
        yield return new WaitForSeconds(time);
        m_isHit = false;
        m_startFollow = false;
        m_batterControll.ResetPlayer();
        m_pitcherControll.ResetPlayer();
        m_timeText.text = "";
        m_allCamera[1].transform.position = m_pitcherCameraPoint.position;
        m_allCamera[0].SetActive(true);
        m_allCamera[1].SetActive(false);
        StartCoroutine(StartGame());
    }

}
