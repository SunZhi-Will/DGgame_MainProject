using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archery_ItemThrowing : MonoBehaviour
{
    public Vector3  target;
    public  bool isThrow = false;

    public const float g = 9.8f;

    public float speed = 10;
    private float verticalSpeed;
    private Vector3 moveDirection;

    private float angleSpeed;
    private float angle;
    
    private float time;

    void Update()
    {
        if (isThrow) { ThrowItems(); }
    }

    void ThrowItems()
    {
        if (transform.position.y < target.y)
        {
            //finish
            transform.position = target;
            isThrow = false;
            transform.rotation = Quaternion.identity;
            this.enabled = false;
            transform.GetChild(0).GetComponent<Archery_Item>().onGround.Invoke();
            return;
        }
        time += Time.deltaTime;
        float test = verticalSpeed - g * time;
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.up * test * Time.deltaTime, Space.World);
        float testAngle = -angle + angleSpeed * time;
        transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    public void SetValue()
    {
        float tmepDistance = Vector3.Distance(transform.position, target);
        float tempTime = tmepDistance / speed;
        float riseTime, downTime;
        riseTime = downTime = tempTime / 2;
        verticalSpeed = g * riseTime;
        transform.LookAt(target);

        float tempTan = verticalSpeed / speed;
        double hu = Mathf.Atan(tempTan);
        angle = (float)(180 / Mathf.PI * hu);
        transform.eulerAngles = new Vector3(-angle, transform.eulerAngles.y, transform.eulerAngles.z);
        angleSpeed = angle / riseTime;

        moveDirection = target - transform.position;
    }
}
