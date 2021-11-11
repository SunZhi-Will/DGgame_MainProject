using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEditing : MonoBehaviour
{
    public static GameObject g_PlaceObjects;
    [SerializeField]
    [Header("生成物件")]
    private GameObject m_Objects;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnMouseDown(){
        Debug.Log("W");
        if(g_PlaceObjects == null){
            Destroy(m_Objects);
        }else{
            Destroy(m_Objects);
            m_Objects = Instantiate(g_PlaceObjects, transform.position, g_PlaceObjects.transform.rotation);
            m_Objects.transform.parent = GameObject.Find("Site").transform;
        }
        
    }
    
}
