using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public static ObjectController instance;

    public GameObject[] pot,Bat;
    public GameObject stains;
    public const float g = 50f;
    float speed = 30;
    private float verticalSpeed;
    private Vector3 moveDirection;
    private float angleSpeed;
    private float angle;
    private float time;
    public Vector3 target;
    Rigidbody ball_R;
    bool  ToTeammate1=false, ToTeammate2 = false;
    float tmepDistance, tempTime, riseTime, tempTan,downTime, test, testAngle;
    double hu;
    int x;
    
    ObjectCreate OCreate;
    ObjectPool OP;

    private void Awake()
    {
        OP = ObjectPool.instance;
        OCreate = ObjectCreate.instance;
        ball_R = gameObject.GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        if (gameObject.activeSelf) {
            StartPass();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Bat = GameObject.FindGameObjectsWithTag("Bat");
        StartPass();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (ToTeammate1)
        {
            if (Vector3.Distance(gameObject.transform.position, target) ==0)
            {
                ToTeammate1 = false;
                ball_R.useGravity = true;
                return;
            }
            time += Time.deltaTime;
            test = verticalSpeed - g * time;
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);
            transform.Translate(Vector3.up * test * Time.deltaTime, Space.World);
            testAngle = -angle + angleSpeed * time;
            transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else if (ToTeammate2)
        {
            if (Vector3.Distance(gameObject.transform.position, target)<=0)
            {
                ToTeammate2 = false;
                ball_R.useGravity = true;
                return;
            }
            time += Time.deltaTime;
            test = verticalSpeed - g * time;
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);
            transform.Translate(Vector3.up * test * Time.deltaTime, Space.World);
            testAngle = -angle + angleSpeed * time;
            transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else {
            if (Vector3.Distance(gameObject.transform.position, target) ==0)
            {
                return;
            }
            time += Time.deltaTime;
            test = verticalSpeed - g * time;
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);
            transform.Translate(Vector3.up * test * Time.deltaTime, Space.World);
            testAngle = -angle + angleSpeed * time;
            transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        checkDistance();

        if (gameObject.transform.position.y <= 0  ) {
            //Debug.Log("Yes");
            //Instantiate(stains,new Vector3(gameObject.transform.position.x,0.07f, gameObject.transform.position.z), stains.transform.rotation);
            OP.GetStain(new Vector3(gameObject.transform.position.x, -2.5f, gameObject.transform.position.z), stains.transform.rotation);
            //Destroy(gameObject, 0);
            OCreate.loc[x].Stop();
            OP.PutBack(gameObject);
            

        }
        

    }
    int sw=3;
    public void Pass() {
        if (!(sw == 0 || sw == 2))
        {
            OCreate.loc[x].transform.position =new Vector3(target.x,1, target.z);
            OCreate.loc[x].Play();
        }
        else {
            sw = 5;
        }
        tmepDistance = Vector3.Distance(transform.position, target);
        tempTime = tmepDistance / speed;
        riseTime = downTime = tempTime / 2;
        verticalSpeed = g * riseTime;
        transform.LookAt(target);

        tempTan = verticalSpeed / speed;
        hu = Math.Atan(tempTan);
        angle = (float)(180 / Math.PI * hu);
        transform.eulerAngles = new Vector3(-angle, transform.eulerAngles.y, transform.eulerAngles.z);
        angleSpeed = angle / riseTime;

        moveDirection = target - transform.position;
        time = 0;
    }

    private float Range = 10;
    public void checkDistance() {
        //Debug.Log(Vector3.Distance(gameObject.transform.position, Bat[0].transform.position));
        //if (Vector3.Distance(gameObject.transform.position, Bat[0].transform.position) <= 6)
        if (Vector3.Distance(gameObject.transform.position, Bat[0].transform.position) <= Range)
            {
            
            if (OCreate.IsSwing_1)
            {
                //Debug.Log("IN_1");
                OCreate.swing.Play();
                OCreate.swing_1.Play();
                OCreate.IsSwing_1 = false;
                ball_R.useGravity = false;
                ball_R.velocity = Vector3.zero;
                sw = OCreate.swing1;
                OCreate.loc[x].Stop();
                if (sw == 1 || sw == 3)
                {
                    OCreate.loc[x].Stop();
                    x = 2;
                }
                target = pot[OCreate.swing1].transform.position;
                Pass();
                ToTeammate1 = true;
            }
        }
        //else if (Vector3.Distance(gameObject.transform.position, Bat[1].transform.position) <= 6)
        else if (Vector3.Distance(gameObject.transform.position, Bat[1].transform.position) <= Range)
        {
            if (OCreate.IsSwing_2)
            {
                //Debug.Log("IN_2");
                OCreate.swing.Play();
                OCreate.swing_2.Play();
                OCreate.IsSwing_2 = false;
                ball_R.useGravity = false;
                ball_R.velocity = Vector3.zero;
                sw = OCreate.swing2;
                OCreate.loc[x].Stop();
                if (sw == 1 || sw == 3)
                {
                    OCreate.loc[x].Stop();
                    x = 3;
                }
                target = pot[OCreate.swing2].transform.position;
                Pass();
                ToTeammate2 = true;
            }
        }
        else {
            
        }
        
    }

    public void StartPass() {
        //Debug.Log("Load_StartPass");
        ball_R.useGravity = false;
        ball_R.velocity = Vector3.zero;
        target = OCreate.target;
        x = OCreate.x;
        Pass();
    }

    
    
}
