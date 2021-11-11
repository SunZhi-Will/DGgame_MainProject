using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StainController : MonoBehaviour
{
    ObjectPool OP;
    GameObject[] player;
    // Start is called before the first frame update
    void Start()
    {
        OP = ObjectPool.instance;
         //Debug.Log("Stain");
        player = GameObject.FindGameObjectsWithTag("Player");

    }
    private void OnEnable()
    {
        StartCoroutine(StainsClean());

    }

    // Update is called once per frame
    void Update()
    {
       
    }
   

    IEnumerator StainsClean()
    {
        //Debug.Log("N2");
        yield return new WaitForSeconds(3);
        OP.PutStainBack(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        StopCoroutine(StainsClean());


    }
}
