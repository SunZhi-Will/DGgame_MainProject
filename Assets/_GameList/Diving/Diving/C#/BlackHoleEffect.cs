using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    [Header("黑洞吸收速度")]
    private float speed = 20;
    [SerializeField]
    [Header("黑洞是否開啟")]
    private bool m_TurnOn;

    [SerializeField]
    [Header("黑洞特效")]
    private GameObject m_BlackHole;

    [SerializeField]
    [Header("消失特效")]
    private GameObject m_AirBlackHole;
    void Start()
    {
        m_TurnOn = true;
    }
    private GameObject _go;
    // Update is called once per frame
    void Update()
    {
        
        /*if(_go != null){
            float _x = transform.position.x - _go.transform.position.x + transform.position.y - _go.transform.position.y;
            if(_x < 0.001 && _x > -0.001){
                Destroy(_go);
            }else if(_x < 0.1 && _x > -0.1){
                
            }
            
        }*/
    }
    private void OnTriggerStay(Collider other){
        if(m_TurnOn && other.gameObject.tag.IndexOf("Diver")>-1){
            
            float _x = transform.position.x - other.gameObject.transform.position.x;
            float _y = transform.position.y - other.gameObject.transform.position.y;
            float _z = Abs(_x) + Abs(_y);
            //Debug.Log(_x+ " " +_y);
            if( _z < 2f){
                Destroy(other.gameObject);
                m_TurnOn = false;
                
                m_BlackHole.SetActive(false);
                StartCoroutine(AppearRandomly(Random.Range(5, 30)));
                m_AirBlackHole.SetActive(true);
            }else {
                other.gameObject.GetComponent<Rigidbody>().velocity += new Vector3(_x / _z, _y / _z, 0);
                
            }
        }

    }
    private float Abs(float _num){
        return _num > 0 ? _num : -_num;
    }

    private IEnumerator AppearRandomly(float _second){
        yield return new WaitForSeconds(_second);
        m_TurnOn = true;
        m_BlackHole.SetActive(true);
        m_AirBlackHole.SetActive(false);
    }
}
