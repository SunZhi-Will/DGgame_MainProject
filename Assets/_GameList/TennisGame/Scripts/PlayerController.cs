using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject player;
    public GameObject[] point;
    Vector3 _point,_point1,_point2;
    public Animator anim;
    public bool IsSwing_1, IsSwing_2;
    ObjectController OC;
    ObjectCreate O_Craete;
    ObjectPool OP;
    BoatController BC;
    // Start is called before the first frame update
    Rigidbody r;
    float count = 2;
    bool notchange = true;

    void Start()
    {
        O_Craete = ObjectCreate.instance;
        OC = ObjectController.instance;
        OP = ObjectPool.instance;
        BC = BoatController.instance;
        IsSwing_1 = false;
        IsSwing_2 = false;
        r = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        switch (player.name)
        {
            case "Player 1":
                Swing1();
                if (O_Craete.isChange && notchange)
                {
                    if (O_Craete.player1IsLeft)
                        {
                            count = 0.0f;
                        //O_Craete.player1IsLeft = !O_Craete.player1IsLeft;
                        _point = gameObject.transform.position + (gameObject.transform.position - point[1].transform.position) / 2 + Vector3.up * 14.0f;
                            _point1 = gameObject.transform.position;
                            _point2 = point[1].transform.position;
                        notchange = false;
                    }
                    else
                    {
                            count = 0.0f;
                        //O_Craete.player1IsLeft = !O_Craete.player1IsLeft;
                        _point = gameObject.transform.position + (point[0].transform.position - gameObject.transform.position) / 2 + Vector3.up * 4.0f;
                            _point1 = gameObject.transform.position;
                            _point2 = point[0].transform.position;
                        notchange = false;
                    }
                    
                    
                }
                break;
            case "Player 2":
                Swing2();
                if (O_Craete.isChange && notchange)
                {
                    if (!O_Craete.player1IsLeft)
                    {
                        count = 0.0f;
                        //Debug.Log(gameObject.name + " + Change");
                        _point = gameObject.transform.position + (gameObject.transform.position - point[1].transform.position) / 2 + Vector3.up * 14.0f;
                        _point1 = gameObject.transform.position;
                        _point2 = point[1].transform.position;
                        notchange = false;
                    }
                    else
                    {
                        count = 0.0f;
                        _point = gameObject.transform.position + (point[0].transform.position - gameObject.transform.position) / 2 + Vector3.up * 4.0f;
                        _point1 = gameObject.transform.position;
                        _point2 = point[0].transform.position;
                        notchange = false;
                    }
                }
                break;
        }
        r.Sleep();
        
        if (count < 1.0f)
        {
            count += 1.0f * Time.deltaTime;
            Vector3 m1 = Vector3.Lerp(_point1, _point, count);
            Vector3 m2 = Vector3.Lerp(_point, _point2, count);
            gameObject.transform.position = Vector3.Lerp(m1, m2, count);
        }
        else if (count > 1.0f && count<1.5f ) {
            O_Craete.isChange = false;
            notchange = true;
            count = 3;
        }
        

    }

    public void Swing1()
    {

        //if (Input.GetKeyDown(KeyCode.V))
        if(PlayerControl_koroshi.KeyDown(PlayerNumber.p1,PlayerKeyName.Cancel))
        {
            //IsSwing_1 = true;
            //anim.SetLayerWeight(1, 1);
            if (O_Craete.player1IsLeft)
            {
                anim.SetTrigger("ToR");
            }
            else
            {
                anim.SetTrigger("ToL");
            }

        }
        //else if (Input.GetKeyDown(KeyCode.C))
        if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1,PlayerKeyName.Confirm))
        {
                //IsSwing_1 = true;
                //anim.SetLayerWeight(1, 1);
                anim.SetTrigger("PassBOSS");
        }

    }
    public void Swing2()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2,PlayerKeyName.Cancel))
        {
            //IsSwing_2 = true;
            //anim.SetLayerWeight(1, 1);
            if (O_Craete.player1IsLeft)
            {
                anim.SetTrigger("ToL");
            }
            else
            {
                anim.SetTrigger("ToR");
            }
        }
        //else if (Input.GetKeyDown(KeyCode.O))
        if (PlayerControl_koroshi.KeyDown(PlayerNumber.p2,PlayerKeyName.Confirm))
        {
            //IsSwing_2 = true;
            //anim.SetLayerWeight(1, 1);
            anim.SetTrigger("PassBOSS");
        }


    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collider");
        if (other.gameObject.tag == "Stain")
        {

            switch (gameObject.name)
            {
                case "Player 1":
                    //Debug.Log("Stain" + gameObject.name + "//" + other.gameObject.tag);
                    OP.speed1 = 9f;
                    BC.time1 = 0;
                    break;
                case "Player 2":
                    OP.speed2 = 9f;
                    BC.time2 = 0;
                    break;
            }

        }


    }

}
