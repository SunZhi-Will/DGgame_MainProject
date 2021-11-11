using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObGenerator : MonoBehaviour
{
    public GameData_Diving gameData_scr;
    [SerializeField]
    [Header("場地")]
    private GameObject[] m_Site;

    [SerializeField]
    [Header("場地生成點")]
    private Transform m_SitePos;

    [SerializeField]
    [Header("障礙物")]
    private GameObject[] m_Obstacle;

    [SerializeField]
    [Header("道具")]
    private GameObject[] m_Props;
    [SerializeField]
    [Header("勝利物件")]
    private GameObject m_Win;

    [SerializeField]
    [Header("區域交換")]
    private bool m_VictoryZoneExchange;

    [SerializeField]
    [Header("勝利範圍擴增速度")]
    private float m_AmplificationSpeed;

    [SerializeField]
    [Header("局勝UI")]
    private GameObject m_WinUI;

    [SerializeField]
    [Header("局勝UIText")]
    private Text m_WinUI_Text;

    [SerializeField]
    [Header("一局時間")]
    private int m_CountdownSeconds = 120;

    private int m_sCountdownSeconds;

    [SerializeField]
    [Header("倒數UI")]
    private Text m_CountdownSeconds_Text;


    
    private GameObject[] m_ObstaclePint;
    private GameObject[] m_PropsPint;
    private GameObject m_Win_P1;
    private GameObject m_Win_P2;

    private static GameObject ms_WinUI;
    private static UnityEngine.UI.Text ms_WinUI_Text;

    private GameObject m_CurrentVenue;
    

    [SerializeField]
    [Header("開始")]
    public static bool m_Start;

    [SerializeField]
    [Header("開始倒數")]
    private Text m_StartText;

    [SerializeField]
    [Header("勝利局數")]
    private int m_TotalGames = 1;
    [SerializeField]
    [Header("P1獲勝次數")]
    private int m_P1Wins;
    [SerializeField]
    [Header("P2獲勝次數")]
    private int m_P2Wins;

    [SerializeField]
    [Header("P1次數Text")]
    private Text[] m_P1Wins_Text;
    [SerializeField]
    [Header("P2次數Text")]
    private Text[] m_P2Wins_Text;

    [SerializeField]
    [Header("獲勝呼叫")]
    private ChangeRole m_CMain;


    private void Start()
    {
        m_Start = false;
        m_P1Wins = m_P2Wins = 0;
        //SiteInitialization();
        
    }
    public void SetTotalGames(int _TotalGames){
        m_TotalGames = _TotalGames;
        //if(m_TotalGames + _TotalGames < 8 && m_TotalGames + _TotalGames > 0){
        //    m_TotalGames += _TotalGames;
        //    GameObject.Find("TotalGames").GetComponent<Text>().text = m_TotalGames +"";
        //}

    }

    public void SiteInitialization()
    {
        m_sCountdownSeconds = m_CountdownSeconds;
        ms_WinUI = m_WinUI;
        ms_WinUI.SetActive(false);
        ms_WinUI_Text = m_WinUI_Text;
        
        int _ram = Random.Range(0, m_Site.Length);
        m_CurrentVenue= Instantiate(m_Site[_ram], m_SitePos.position, transform.rotation);
        
        m_CurrentVenue.transform.parent = gameObject.transform.parent.gameObject.transform;
        
        m_ObstaclePint = GameObject.FindGameObjectsWithTag("ObstaclePint");
        foreach (var item in m_ObstaclePint)
        {
            _ram = Random.Range(-1, m_Obstacle.Length);
            if(_ram == -1){
                Destroy(item);
            }else{
                GameObject _go = Instantiate(m_Obstacle[_ram], item.transform.position, item.transform.rotation);
                _go.transform.parent = item.transform.parent;
                Destroy(item);
            }
        }

        m_PropsPint = GameObject.FindGameObjectsWithTag("PropsPoint");
        foreach (var item in m_PropsPint)
        {
            _ram = Random.Range(0, m_Props.Length);
            if(_ram == -1){
                Destroy(item);
            }else{
                GameObject _go = Instantiate(m_Props[_ram], item.transform.position, m_Props[_ram].transform.rotation);
                _go.transform.parent = item.transform.parent;
                Destroy(item);
            }
        }
        gameObject.GetComponent<UpdateObject>().UpObject();

        //if()
        //StartCoroutine(StartCountdown(3f));
    }
    public IEnumerator StartCountdown(float delaySeconds)
    {
        m_StartText.text = delaySeconds + "";
        yield return new WaitForSeconds(0.99f);
        if(delaySeconds - 1f > 0){
            StartCoroutine(StartCountdown(delaySeconds - 1f));
        }else{
            StartThisGame();
            m_StartText.transform.parent.gameObject.SetActive(false);
            //m_StartText.gameObject.SetActive(false);
            //GameObject.Find("bg").SetActive(false);
        }
    }
    public void StartThisGame()
    {
        m_Start = true;
        m_VictoryZoneExchange = false;
        m_Win_P1 = Instantiate(m_Win, gameObject.transform.position + new Vector3(-11f, 0, 0), m_Win.transform.rotation);
        m_Win_P1.name = "P1 Win";
        m_Win_P1.GetComponent<MeshRenderer>().material.color = Color.blue;
        m_Win_P1.transform.parent = gameObject.transform;
        m_Win_P1.GetComponent<VictoryConditions>().test = 1;
        m_Win_P2 = Instantiate(m_Win, gameObject.transform.position + new Vector3(11f, 0, 0), m_Win.transform.rotation);
        m_Win_P2.name = "P2 Win";
        m_Win_P2.GetComponent<MeshRenderer>().material.color = Color.red;
        m_Win_P2.transform.parent = gameObject.transform;
        Destroy(m_CurrentVenue, m_CountdownSeconds);
        
        StartCoroutine(ForcedEndOfCountdown());
        

    }

    // Update is called once per frame
    void Update()
    {
        if(m_Start){
           VictoryZoneExchange();
        }
    }
    private void VictoryZoneExchange(){
        if(m_VictoryZoneExchange){
            if(m_Win_P2.transform.localScale.x < 23f / 2f){
                m_Win_P2.transform.localScale += new Vector3(m_AmplificationSpeed * Time.deltaTime, 0, 0);
                m_Win_P2.transform.position += new Vector3(m_AmplificationSpeed / 2 * Time.deltaTime, 0, 0);
                m_Win_P1.transform.localScale += new Vector3(m_AmplificationSpeed * Time.deltaTime, 0, 0);
                m_Win_P1.transform.position -= new Vector3(m_AmplificationSpeed / 2 * Time.deltaTime, 0, 0);
            }
        }else{
            if(m_Win_P1.transform.localScale.x < 23f / 2f){
                m_Win_P1.transform.localScale += new Vector3(m_AmplificationSpeed * Time.deltaTime, 0, 0);
                m_Win_P1.transform.position += new Vector3(m_AmplificationSpeed / 2 * Time.deltaTime, 0, 0);
                m_Win_P2.transform.localScale += new Vector3(m_AmplificationSpeed * Time.deltaTime, 0, 0);
                m_Win_P2.transform.position -= new Vector3(m_AmplificationSpeed / 2 * Time.deltaTime, 0, 0);
            }
        }
    }


    public void ViConditions(string _str){
        if(_str.IndexOf("P1") > -1){
            m_P1Wins++;
            foreach (var item in m_P1Wins_Text)
            {
                item.text = m_P1Wins + "";
            }
            m_P1Wins_Text[1].color = new Color32(255, 255, 0, 255);
            m_P2Wins_Text[1].color = new Color32(255, 255, 255, 255);
            
        }else{
            m_P2Wins++;
            foreach (var item in m_P2Wins_Text)
            {
                item.text = m_P2Wins + "";
            }
            m_P1Wins_Text[1].color = new Color32(255, 255, 255, 255);
            m_P2Wins_Text[1].color = new Color32(255, 255, 0, 255);
        }
        ms_WinUI.SetActive(true);
        ms_WinUI_Text.text = "";
        //ms_WinUI_Text.text = "第"+ (m_P1Wins + m_P2Wins + 1) + "回合";


        

        Destroy(m_Win_P1);
        Destroy(m_Win_P2);
        Destroy(m_CurrentVenue);
        m_CountdownSeconds = m_sCountdownSeconds;
        m_Start = false;
        if( (m_P1Wins + m_P2Wins < m_TotalGames) && (m_P1Wins <= m_TotalGames / 2) && (m_P2Wins <= m_TotalGames / 2)){
            StartCoroutine(RestartGame());
        }
        else{
            ms_WinUI.SetActive(false);
            //m_CountdownSeconds_Text.text = "";
            m_CountdownSeconds_Text.transform.parent.gameObject.SetActive(false);

            //m_CMain.Win(m_P1Wins, m_P2Wins);
            gameData_scr.Common_scr.ShowSettlement(m_P1Wins, m_P2Wins);
        }
    }
    private IEnumerator RestartGame(){
        yield return new WaitForSeconds(3f);
        m_StartText.transform.parent.gameObject.SetActive(true);
        //m_StartText.gameObject.SetActive(true);
        //GameObject.Find("bg").SetActive(true);
        m_CountdownSeconds_Text.color = new Color32(255, 255, 255, 255);
        //m_CountdownSeconds_Text.gameObject.GetComponent<Outline>().effectColor = new Color32(36, 36, 36, 114);
        //m_CountdownSeconds_Text.gameObject.GetComponent<Shadow>().effectColor = new Color32(36, 36, 36, 128);
        m_CountdownSeconds_Text.text = "2:00";
        SiteInitialization();
        StartCoroutine(StartCountdown(3f));//下一局倒數另外開
    }


    private IEnumerator ForcedEndOfCountdown(){
        
        m_CountdownSeconds--;
        string _s = "";
        /*if(m_CountdownSeconds % 30 == 0){
            m_VictoryZoneExchange = !m_VictoryZoneExchange;
            Vector3 _pos = m_Win_P1.transform.position;
            m_Win_P1.transform.position = m_Win_P2.transform.position;
            m_Win_P2.transform.position = _pos;
        }*/


        if(m_CountdownSeconds % 60 < 10){
            _s = "0";
        }
        if(m_CountdownSeconds < 10){
            m_CountdownSeconds_Text.color = new Color32(255, 0, 0, 128);
            //m_CountdownSeconds_Text.gameObject.GetComponent<Shadow>().effectColor = new Color32(255, 0, 0, 128);
        }
        m_CountdownSeconds_Text.text = (m_CountdownSeconds / 60) + ":" + _s + (m_CountdownSeconds % 60);
        yield return new WaitForSeconds(1f);
        
        if(m_CountdownSeconds > 0 && m_Start){
            StartCoroutine(ForcedEndOfCountdown());
        }
    }
}
