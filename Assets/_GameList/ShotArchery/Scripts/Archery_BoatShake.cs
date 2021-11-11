using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archery_BoatShake : MonoBehaviour
{
    [SerializeField] float deltaTime, offestTime, shakeValue, offestY;

    Vector3 oPos;
    float times;
    private void Start()
    {
        oPos = transform.position;
    }

    private void Update()
    {
        times += Time.deltaTime;

        transform.position = oPos + new Vector3(0, Mathf.Sin((times + offestTime) * deltaTime) * shakeValue + offestY, 0);
    }
}
