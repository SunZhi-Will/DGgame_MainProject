using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    
    public Rigidbody g_BallisticSeed;

    public AudioClip g_FallingWater;

    private Rigidbody m_BallisticSeed;
    private float m_LockCoordinates;


    private bool m_LockZ = false;
    private bool m_LockDelay = false;


    [SerializeField]
    [Header("落水特效")]
    private GameObject m_FallingIntoTheWater;



    private void Start()
    {
        m_BallisticSeed = g_BallisticSeed;
        //StartLaunch();
        
    }

    
    private void Update()
    {
        if(transform.position.z < m_LockCoordinates || m_LockZ){
            m_BallisticSeed.constraints = RigidbodyConstraints.FreezePositionZ;
            m_LockZ = true;

            transform.position = new Vector3(transform.position.x,transform.position.y, m_LockCoordinates);
        }
        if(!m_LockDelay && transform.position.y < 0){
            m_LockDelay = true;
            gameObject.GetComponent<AudioSource>().clip = g_FallingWater;
            gameObject.GetComponent<AudioSource>().Play();
            Instantiate(m_FallingIntoTheWater, transform.position, new Quaternion());
            Destroy(gameObject, 0.01f);
        }
        
    }
    public void StartLaunch(Vector3 _Height){
        gameObject.GetComponent<AudioSource>().Play();
        m_LockCoordinates = _Height.z;
        m_BallisticSeed.velocity = _Height - transform.position + new Vector3((transform.position.x - _Height.x) / 2, 5f, 0);
        m_BallisticSeed.velocity = m_BallisticSeed.velocity + new Vector3(m_BallisticSeed.velocity.x, 0, 0);
        //m_BallisticSeed.velocity = new Vector3(Random.Range(-1f, 1f), _Height,-20f);


    }
    

    

}
