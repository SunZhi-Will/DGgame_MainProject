using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullBox2 : MonoBehaviour
{
    [SerializeField] Animator animator;


    Collider PullBoxCollider2;
    public GameObject Player1;
    public GameObject Player2;

    public GameObject effect;
    Vector3 place;
    public AudioClip pull;
    AudioSource audiosource;

    void Start()
    {    
        audiosource = GetComponent<AudioSource>();
    }
    void Update()
    {
        this.transform.position = Player2.transform.position ;
    }

    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player1")
        {

            //if (Player1.GetComponent<PlayerAI>().InitialSpeed == 1 && Input.GetKeyDown(KeyCode.E))
            if (Player1.GetComponent<PlayerAI>().InitialSpeed == 1 && PlayerControl_koroshi.Key(PlayerNumber.p2, PlayerKeyName.Confirm))
            {
                place = this.transform.position;
                place.y -= 1.4f;
                Instantiate(effect, place, Quaternion.Euler(new Vector3(0, 0, 0)));
                audiosource.PlayOneShot(pull);
                Debug.Log("2拉1");
                Player1.GetComponent<PlayerAI>().InitialSpeed = 4;
            }
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1")
        {
            //PullBoxCollider2.isTrigger = false;
        }
    }
}
