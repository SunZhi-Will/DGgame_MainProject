using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceAI : MonoBehaviour
{
    public GameObject effect;
    Vector3 place;
    public AudioClip push;
    AudioSource audiosource;

    void Start()
    {
        place = this.transform.position;
        place.y += 1.5f;
        audiosource = GetComponent<AudioSource>();
    }
    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Pull1"|| other.tag == "Pull2")
        {
            if ((other.tag == "Pull1"&& Input.GetKeyDown(KeyCode.I))||(other.tag == "Pull2" && Input.GetKeyDown(KeyCode.E)))
            {
                Instantiate(effect, place, Quaternion.Euler(new Vector3(0, -90, 0)));
                audiosource.PlayOneShot(push);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().velocity += Vector3.forward*2;
            }
        }

    }
}
