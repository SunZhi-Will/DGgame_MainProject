using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringEffect : MonoBehaviour
{
    [SerializeField]
    [Header("彈跳高度")]
    private float m_BounceHeight = 20f;
    [SerializeField]
    [Header("特效")]
    private GameObject m_Effects;
    [SerializeField]
    [Header("是否一次性")]
    private bool m_Disappear;

    private void OnTriggerEnter(Collider other){
        try{
            if(other.gameObject.tag.IndexOf("Diver") > -1){
                
                other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3( other.gameObject.GetComponent<Rigidbody>().velocity.x, m_BounceHeight, other.gameObject.GetComponent<Rigidbody>().velocity.z);
                StartCoroutine(SpecialEffects());

            }
        }catch(MissingComponentException e){
            Debug.Log(e);
            
        }
    }
    public IEnumerator SpecialEffects(){
        m_Effects.SetActive(true);
        if(m_Disappear){
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        yield return new WaitForSeconds(1f);
        m_Effects.SetActive(false);
        if(m_Disappear){
            Destroy(gameObject);
        }
    }
}
