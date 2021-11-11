using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class CameraAI : MonoBehaviour
{
    public Transform target;
    public float x=180;
    public float y=25;
    public float xSpeed = 150;
    public float ySpeed = 150;
    public float distance = 20;
    public float disSpeed = 200;
    public float minDistence = 1;
    public float maxDistence = 15;
    public float maxrotation = 180;
    public float minrotation = -180;
    private Quaternion rotationEuler;
    private Vector3 cameraPosition;

    void Update()
    {
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

        if (x > 360)
        {
            x -= 360;
        }
        else if (x < 0)
        {
            x += 360;
        }

        distance -= Input.GetAxis("Mouse ScrollWheel") * disSpeed * Time.deltaTime;

        distance = Mathf.Clamp(distance, minDistence, maxDistence);

        rotationEuler = Quaternion.Euler(y, x, 0);

        cameraPosition = rotationEuler * new Vector3(0, 0, -distance) + target.position;

            transform.rotation = rotationEuler;
            transform.position = cameraPosition;
        
    }
    
}