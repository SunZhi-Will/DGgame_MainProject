using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullBoxAI : MonoBehaviour
{
    [SerializeField] Animator animator;


    Collider PullBoxCollider;
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
        this.transform.position = Player1.transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player2")
        {
            //if (Player2.GetComponent<Player2AI>().InitialSpeed == 1 && Input.GetKeyDown(KeyCode.I))
            if (Player2.GetComponent<Player2AI>().InitialSpeed == 1 && PlayerControl_koroshi.Key(PlayerNumber.p1, PlayerKeyName.Confirm))
            {
                place = this.transform.position;
                place.y -= 1.4f;
                Instantiate(effect, place, Quaternion.Euler(new Vector3(0, 0, 0)));
                audiosource.PlayOneShot(pull);
                Debug.Log("1拉2");
                Player2.GetComponent<Player2AI>().InitialSpeed = 4;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player2")
        {
            //PullBoxCollider.isTrigger = false;
        }
    }
}