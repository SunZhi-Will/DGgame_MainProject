using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateObject : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    [Header("替代標籤")]
    private string[] m_ObTag;

    [SerializeField]
    [Header("更新物件")]
    private GameObject[] m_Obstacle;

    private GameObject[] m_PropsPint;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpObject()
    {
        int _i = 0;
        foreach (var item in m_ObTag)
        {
            m_PropsPint = GameObject.FindGameObjectsWithTag(item);
            ObjectHandling(m_PropsPint, _i * 4);
            _i++;
        }
    }
    private void ObjectHandling(GameObject[] _PropsPint, int __i){
        int _ram;
        foreach (var item in _PropsPint)
        {
            _ram = Random.Range(__i, __i + 4);
            if(_ram == -1){
                Destroy(item);
            }else{
                GameObject _go = Instantiate(m_Obstacle[_ram], item.transform.position, item.transform.rotation);
                _go.transform.parent = item.transform.parent;
                Destroy(item);
            }
        } 
    }
}
