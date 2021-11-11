using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedLaunch : MonoBehaviour
{
    public GameObject g_CatapultGo;
    public float g_ServeTime;

    [SerializeField]
    [Header("重生點")]
    private GameObject[] m_AllRebirthPoint;
    private GameObject m_RebirthPoint;

    [SerializeField]
    [Header("跳水點")]
    private GameObject m_Height;
    
    private GameObject m_StandbyBall;

    [SerializeField]
    [Header("開始")]
    private bool m_Start;
     

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        m_Start = ObGenerator.m_Start;
        if(m_StandbyBall == null && m_Start){
            int _i = Random.Range(0,m_AllRebirthPoint.Length);
            m_RebirthPoint = m_AllRebirthPoint[_i];
            m_StandbyBall = Instantiate(g_CatapultGo, m_RebirthPoint.transform.position, m_RebirthPoint.transform.rotation);
            GameObject _go;
            if(_i == 0){
                _go = Instantiate(GameObject.Find("Main").GetComponent<ChangeRole>().g_P1, m_RebirthPoint.transform.position, m_RebirthPoint.transform.rotation);
            }else{
                _go = Instantiate(GameObject.Find("Main").GetComponent<ChangeRole>().g_P2, m_RebirthPoint.transform.position, m_RebirthPoint.transform.rotation);
            }

            if(_go.transform.GetChild(0).gameObject.name.IndexOf("Hips") > -1){
                _go.transform.GetChild(1).gameObject.transform.parent = m_StandbyBall.transform;
            }else{
                _go.transform.GetChild(0).gameObject.transform.parent = m_StandbyBall.transform;
            }


            m_StandbyBall.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = m_StandbyBall.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            m_StandbyBall.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().materials = m_StandbyBall.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().materials;
            Destroy(m_StandbyBall.transform.GetChild(2).gameObject);
            Destroy(_go);

            //m_StandbyBall.transform.parent = transform.parent.gameObject.transform.parent.gameObject.transform;
            m_StandbyBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX;
            
            StartCoroutine(DelayToInvokeDo(1f));
        }
    }
    private IEnumerator DelayToInvokeDo(float delaySeconds)
    {
        
        yield return new WaitForSeconds(delaySeconds);
        m_StandbyBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        m_StandbyBall.GetComponent<Catapult>().StartLaunch(m_Height.transform.position);
        
        
    }
    
}
