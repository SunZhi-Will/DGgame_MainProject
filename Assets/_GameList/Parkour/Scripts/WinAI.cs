using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class WinAI : MonoBehaviour
{
    public AudioClip clap;
    public GameObject effect;
    AudioSource audiosource;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1"|| other.tag == "Player2")
        {
            Instantiate(effect, this.transform.position, Quaternion.Euler(new Vector3(0, -90, 0)));
            audiosource.PlayOneShot(clap);
        }
    }
}