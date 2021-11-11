using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowdControl : MonoBehaviour
{
    [SerializeField]
    [Header("是否為P2")]
    public bool g_Play2;
    
    [SerializeField]
    [Header("UI氧氣條")]
    private Image StrengthBar;

    [SerializeField]
    [Header("氧氣自然回復")]
    private float OxygenAudo = 10f;
    
    [SerializeField]
    [Header("氧氣量")]
    private float Oxygen = 100;
    
    [SerializeField]
    [Header("氧氣上限")]
    private float OxygenMax = 200;

    

    [SerializeField]
    [Header("垂直輸入量")]
    private float input_V;
    
    [SerializeField]
    [Header("水平輸入量")]
    private float input_H;

    [SerializeField]
    [Header("人群移動速度")]
    private float m_Speed = 10;

    [SerializeField]
    [Header("球體改變速度")]
    private float DManSpeed = 1;

    [SerializeField]
    [Header("人群移動上限")]
    private float m_Height = 25;

    [SerializeField]
    [Header("吐氣特效")]
    private GameObject m_ExhalationEffects;

    [SerializeField]
    [Header("吸氣特效")]
    private GameObject m_InhalationEffects;
    

    [SerializeField]
    [Header("人員")]
    private GameObject[] m_DivingMan;

    private bool m_Start;

    void FixedUpdate ()
    {
        //if(g_Play2){
        //    input_V = Input.GetAxis ("Vertical_P2");
        //    input_H = -1 * Input.GetAxis ("Horizontal_P2");
        //}else{
        //    input_V = Input.GetAxis ("Vertical");
        //    input_H = Input.GetAxis ("Horizontal");
        //}

        if (!g_Play2)
        {
            input_V = PlayerControl_koroshi.GetAxis(PlayerNumber.p1, PlayerKeyName.Axis_Vertical);
            input_H = -1 * PlayerControl_koroshi.GetAxis(PlayerNumber.p1, PlayerKeyName.Axis_Horizontal);
        }
        else
        {
            input_V = PlayerControl_koroshi.GetAxis(PlayerNumber.p2, PlayerKeyName.Axis_Vertical);
            input_H = PlayerControl_koroshi.GetAxis(PlayerNumber.p2, PlayerKeyName.Axis_Horizontal);
        }
    }

    void Start()
    {
        transform.position = new Vector3(transform.position.x, 12, transform.position.z);
        Oxygen = OxygenMax / 2;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAction();
        if(ObGenerator.m_Start){
            m_Start = true;
        }else if(m_Start){
            Start();
            m_Start = false;
        }
    }
    void PlayerAction()
    {
        if(input_V == 0){
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
        if(input_H == 0 || Oxygen < 0){
            Oxygen +=  OxygenAudo * Time.deltaTime;
        }else{
            Oxygen -= input_H;
        }

        if(Oxygen > OxygenMax){
            Oxygen = OxygenMax;
        }

        StrengthBar.fillAmount = Oxygen / OxygenMax;
        if(((transform.position.y < m_Height && input_V > 0) || (transform.position.y > 0 && input_V < 0))){
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            GetComponent<Rigidbody>().velocity = new Vector3(0, input_V * m_Speed, 0);
        }else if(transform.position.y > m_Height){
            transform.position = new Vector3(transform.position.x , m_Height, transform.position.z);
        }else if(transform.position.y < 0){
            transform.position = new Vector3(transform.position.x , 0, transform.position.z);
        }
            

        if(OxygenMax - Oxygen < OxygenMax / 100 * 10 || Oxygen < OxygenMax / 100 * 10){
            StrengthBar.color = Color.red;
        }else if(OxygenMax - Oxygen < OxygenMax / 100 * 30 || Oxygen < OxygenMax / 100 * 30){
            StrengthBar.color = Color.yellow;
        }else{
            StrengthBar.color = Color.white;
        }

        if(Oxygen > 0 && input_H > 0){
            m_ExhalationEffects.SetActive(true);
            m_InhalationEffects.SetActive(false);
            InhaleAndExhale(m_DivingMan, true, input_H * 15f);
        }else if(Oxygen < OxygenMax && input_H < 0){
            m_InhalationEffects.SetActive(true);
            m_ExhalationEffects.SetActive(false);
            InhaleAndExhale(m_DivingMan, true, input_H * 15f);
        }else{
            m_ExhalationEffects.SetActive(false);
            m_InhalationEffects.SetActive(false);
            InhaleAndExhale(m_DivingMan, false, 0f);
        }
        

        
    }
    private void InhaleAndExhale(GameObject[] _go, bool _gob, float _gof){
        float _num = -90;
        if(!g_Play2){
            _num = 90;
        }
        foreach (var item in _go)
        {
            item.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("InhaleAndExhale", _gob);
            item.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(_gof, _num, 0);
        }
    }
    
    private void OnTriggerStay(Collider other){
        try{
            if(((StrengthBar.fillAmount > 0f && input_H > 0) || (StrengthBar.fillAmount < 1f && input_H < 0)) && (other.gameObject.tag.IndexOf("Diver") > -1 || other.gameObject.name.IndexOf("FloatingObject") > -1)){
                if(g_Play2){
                    other.gameObject.GetComponent<Rigidbody>().velocity += new Vector3( -input_H * DManSpeed, 0, 0);
                    
                }else{
                    other.gameObject.GetComponent<Rigidbody>().velocity += new Vector3( input_H * DManSpeed, 0, 0);
                }
                
            }
        }catch(MissingComponentException e){
            Debug.Log(e);
        }
        //Debug.Log(other.gameObject.tag.IndexOf("ImmutablePhysics"));
    }
}
