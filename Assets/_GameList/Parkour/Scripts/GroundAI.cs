using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class GroundAI : MonoBehaviour
{
    public Material NowMT;
    public int count = 1;
    public AudioClip fall;
    AudioSource audiosource;
    private Rigidbody _rigidbody;//可以暫存起來

    void Start()
    {
        NowMT = GetComponent<Renderer>().material;
        audiosource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
        {
            count--;
            Debug.Log("踩到朽木...");
            audiosource.PlayOneShot(fall);
            NowMT.color = Color.black;
            //Invoke("NoneRigid", 2f);//盡量別用廣播，不然Shift + F12 找不到
            StartCoroutine(NoneRigid(1.5f));
            if(count < 0)
            {
                //Invoke("NoneRigid", 0f);
                StartCoroutine(NoneRigid(0.5f));//延遲些,給人反應
            }
        }
    }
    //void NoneRigid()
    //{
    //    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    //}
    IEnumerator NoneRigid(float _timed)
    {
        yield return new WaitForSeconds(_timed);
        _rigidbody.constraints = RigidbodyConstraints.None;
    }
}