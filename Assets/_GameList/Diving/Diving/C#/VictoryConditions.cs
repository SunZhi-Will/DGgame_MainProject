using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryConditions : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool m_Win_Play; 
    public int test = 0;
    void Start()
    {
        m_Win_Play = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other){
        if(other.gameObject.tag.IndexOf("Diver") > -1 && !m_Win_Play){
            m_Win_Play = true;
            GameObject.Find("Object Generator").GetComponent<ObGenerator>().ViConditions(this.gameObject.name);
        }
    }
}
