using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenEditBox : MonoBehaviour
{
    [SerializeField]
    [Header("寬度點位")]
    private int m_RangePint = 5;
    [SerializeField]
    [Header("高度點位")]
    private int m_GenerateHeight = 21;
    [SerializeField]
    [Header("點之間距離")]
    private float m_ObjectInterval = 3;

    [SerializeField]
    [Header("可放置物件")]
    private GameObject m_PlaceableObjects;

    private float m_Range;
    private GameObject[,] m_VenueLocation;
    private Vector3[,] m_VenueLocationPos;
    void Start()
    {
        m_Range = m_RangePint / 2 * m_ObjectInterval;
        m_VenueLocation = new GameObject[m_GenerateHeight, m_RangePint];
        m_VenueLocationPos = new Vector3[m_GenerateHeight, m_RangePint];
        for(int i = 0; i < m_GenerateHeight; i++){
            for(int j = 0; j < m_RangePint; j++){
                
                GOeneration(j, i, m_PlaceableObjects);
                
            }
        }
    }
    private void GOeneration(int _Pint, int _Height, GameObject _gob){
        GameObject _go;

        
        
        m_VenueLocationPos[_Height, _Pint] = new Vector3(m_Range - _Pint * m_ObjectInterval, (_Height+1) * m_ObjectInterval, transform.position.z);
        
        _go = Instantiate(_gob, m_VenueLocationPos[_Height, _Pint] , _gob.transform.rotation);
        _go.transform.parent = gameObject.transform;
        m_VenueLocation[_Height, _Pint] = _go;
        

        
        
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
