using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectGenerator : MonoBehaviour
{
    [SerializeField]
    [Header("寬度點位")]
    private int g_RangePint = 5;
    [SerializeField]
    [Header("高度")]
    private int g_GenerateHeight = 21;
    [SerializeField]
    [Header("點之間距離")]
    private int g_ObjectInterval = 3;
    [SerializeField]
    [Header("障礙物")]
    private GameObject[] g_Obstacle;
    [SerializeField]
    [Header("道具")]
    private GameObject[] g_SpecialProps;
    [SerializeField]
    [Header("人群")]
    private GameObject g_Play;
    [SerializeField]
    [Header("勝利物件")]
    private GameObject m_Win;


    
    private int m_Range;
    private int m_GenerateHeight;
    
    private GameObject[,] m_VenueLocation;
    private Vector3[,] m_VenueLocationPos;
    private void Start()
    {
        m_Range = g_RangePint / 2 * g_ObjectInterval;
        m_GenerateHeight = g_GenerateHeight;
        m_VenueLocation = new GameObject[m_GenerateHeight, g_RangePint];
        m_VenueLocationPos = new Vector3[m_GenerateHeight, g_RangePint];
        
        GameObject _go = Instantiate(m_Win, new Vector3(-10.5f, 0, 0), m_Win.transform.rotation);
        _go.GetComponent<MeshRenderer>().material.color = Color.red;
        _go.transform.parent = gameObject.transform;
        _go = Instantiate(m_Win, new Vector3(10.5f, 0, 0), m_Win.transform.rotation);
        _go.GetComponent<MeshRenderer>().material.color = Color.blue;
        _go.transform.parent = gameObject.transform;

        for(int i = 0; i < m_GenerateHeight; i++){
            for(int j = 0; j < g_RangePint - (i % 2); j++){
                if(Random.Range(0, 2) == 1){
                    GOeneration(j, i, g_Obstacle[0]);
                }
            }
        }
        Special();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    private void GOeneration(int _Pint, int _Height, GameObject _gob){
        GameObject _go;

        if(m_VenueLocation[_Height, _Pint] != null){
            RepeatProcessing(_gob);
        }else{
            if(_Height % 2 == 0){
                m_VenueLocationPos[_Height, _Pint] = new Vector3(m_Range - _Pint * g_ObjectInterval, (_Height+1) * g_ObjectInterval, transform.position.z);
            }else{
                m_VenueLocationPos[_Height, _Pint] = new Vector3(m_Range - _Pint * g_ObjectInterval - g_ObjectInterval / 2.0f, (_Height+1) * g_ObjectInterval, transform.position.z);
            }
            _go = Instantiate(_gob, m_VenueLocationPos[_Height, _Pint] , _gob.transform.rotation);
            _go.transform.parent = gameObject.transform;
            m_VenueLocation[_Height, _Pint] = _go;
        }

        
        
        
    }
    private void Special(){
        RepeatProcessing(g_SpecialProps[0]);
    }
    private string RepeatProcessing(GameObject _gob){
        for(int i = Random.Range(0, m_GenerateHeight); i < m_GenerateHeight; i++){
            for(int j = Random.Range(0, g_RangePint - (i % 2)); j < g_RangePint - (i % 2); j++){
                if(m_VenueLocation[i, j] == null){
                    GOeneration(j, i, _gob);
                    return "Good";
                }
            }
        }
        return "Err";

    }
}
