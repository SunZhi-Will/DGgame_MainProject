using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("目前控制器")]
    public PlayerControl_ScriptObject playerControl_scrObj;//優點可以通用腳本，缺點是外面要放
    public static BoatController instance;

    GameObject[] Boat,Boat2;//用搜索的會搜索到別人的船，順序會錯亂
    public GameObject win;
    //public GameObject All_player;
    ObjectPool OP;
    ObjectCreate OC;
    public float  time1, time2;
    // Start is called before the first frame update

    public GameCommon gameCommon_scr;
    [SerializeField]
    private Transform nowBoat;//目前角色的船

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        OP = ObjectPool.instance;
        OC = ObjectCreate.instance;

        //Boat = GameObject.FindGameObjectsWithTag("Boat");
        //Boat2 = GameObject.FindGameObjectsWithTag("Boat2");
        //直接把組件下方的所有物件挖出來比對~總會找到的www
        Transform[] _obj_array = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < _obj_array.Length; i++)
        {
            Transform _obj = _obj_array[i];
            if (_obj.tag == "Boat" || _obj.tag == "Boat2")
            {
                nowBoat = _obj;
                break;
            }
        }
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if (OP == null) { Debug.LogWarning(gameObject.name + "\n" + "OP為空"); return; }
        if (OC == null) { Debug.LogWarning(gameObject.name + "\n" + "OC為空"); return; }
        if (!OC.isChange)
        {
            if (OC.game_time != 0)
            {
                /*
                if (gameObject.name == "P1_1" || gameObject.name == "P1_2" || gameObject.name == "P1_3")
                {
                    Move1();
                }
                else if (gameObject.name == "P2_1" || gameObject.name == "P2_2" || gameObject.name == "P2_3")
                {
                    Move2();
                }*/
                if (gameObject.name.Contains("P"))//檢測文字內容是否有對應字段，這樣就不用用等於了
                {
                    Move();
                }
            }
            else
            {
                //Boat[0].transform.rotation = Quaternion.Euler(0, 0, 0);
                //Boat[1].transform.rotation = Quaternion.Euler(0, 0, 0);
                nowBoat.rotation = Quaternion.Euler(0, 0, 0);
            }
            
        }

        time1 += .02f;
        time2 += .02f;
        time1 = Mathf.Clamp(time1, 0f, 3f);
        time2 = Mathf.Clamp(time1, 0f, 3f);
        if (time1 == 3f)
        {
            OP.speed1 = 17f;
            time1 = 0;
        }
        if (time2 == 3f)
        {
            OP.speed2 = 17f;
            time2 = 0;
        }
    }

    ////1P.2P通用移動
    //private enum MoveState
    //{
    //    walk,//走路
    //    Run,//跑步
    //}
    //private MoveState moveState;

    //Vector3 _v_movePos = Vector3.zero;
    //Quaternion _roat = Quaternion.identity;
    private float _angle = 0;//旋轉角度

    void Move()
    {
        float _moveXpos = 0;
        float _moveZpos = 0;
        float _distance = 0;//方向
        float _value = 1.8f;//移動值

        //通用事件
        System.Action _action = () =>
        {
            if (_moveZpos > 0)
                _angle = 45;
            else if (_moveZpos < 0)
                _angle = 135;
            else
                _angle = 90;
        };

        if (PlayerControl_koroshi.Key(playerControl_scrObj.keyUp))
        {
            _moveZpos = Vector3.forward.z * OP.speed1 * Time.deltaTime;
            TwiceKeyDown(playerControl_scrObj.keyUp);
            _angle = 0;
        }
        else if (PlayerControl_koroshi.Key(playerControl_scrObj.keyDown))
        {
            _moveZpos = Vector3.back.z * OP.speed1 * Time.deltaTime;
            _angle = 180;
            TwiceKeyDown(playerControl_scrObj.keyDown);
        }
        if (PlayerControl_koroshi.Key(playerControl_scrObj.keyLeft))
        {
            _moveXpos = Vector3.left.x * OP.speed1 * Time.deltaTime;
            _distance = -1;
            _action();
            _angle = _angle * _distance;
            TwiceKeyDown(playerControl_scrObj.keyLeft);
        }
        else if (PlayerControl_koroshi.Key(playerControl_scrObj.keyRight))
        {
            _moveXpos = Vector3.right.x * OP.speed1 * Time.deltaTime;
            _distance = 1;
            _action();
            _angle = _angle * _distance;
            TwiceKeyDown(playerControl_scrObj.keyRight);
        }


        //旋轉
        nowBoat.rotation = Quaternion.Lerp(nowBoat.rotation, Quaternion.Euler(0, _angle, 0), 0.3f);
        //TwiceKeyDown_Timed_Update();
        //switch (moveState)
        //{
        //    case MoveState.walk:
        //        transform.Translate(_moveXpos, 0, _moveZpos);
        //        break;
        //    case MoveState.Run:
        //        transform.Translate(_moveXpos * 2, 0, _moveZpos);
        //        break;
        //}

        transform.Translate(_moveXpos * _value, 0, _moveZpos * _value);
        //Vector3 _newPos = new Vector3(_moveXpos * _value, 0, _moveZpos * _value);
        //Vector3 _pos = transform.localPosition;
        //Vector3 _endPos = new Vector3(_pos.x + _newPos.x, _pos.x + _newPos.y, _pos.z + _newPos.z);
        //transform.localPosition = Vector3.Lerp(transform.localPosition, _endPos, 1);
    }
    ////連續兩次按下相同方向鍵
    ////private 
    //private string nowKey;
    //private int KeyDownCount = 0;
    //private float nowWaitKeyDownTwiceTimed;//超過指定時間重置按下時間
    //public float maxWaitKeyDownTwiceTimed = 1;
    void TwiceKeyDown(string _newkey)
    {
    //    if (PlayerControl_koroshi.KeyDown(_newkey))
    //    {
    //        if (nowKey != _newkey)//更換鍵時
    //        {
    //            nowKey = _newkey;
    //            KeyDownCount = 0;
    //        }
    //        KeyDownCount++;
    //        if (KeyDownCount > 1)//連按N次後
    //        {
    //            moveState = MoveState.Run;
    //        }
    //    }
    //}
    ////計算重置跑步時間
    //void TwiceKeyDown_Timed_Update()
    //{
    //    if (KeyDownCount > 1) { return; }
    //    if (nowWaitKeyDownTwiceTimed < maxWaitKeyDownTwiceTimed)
    //    {
    //        nowWaitKeyDownTwiceTimed += Time.deltaTime;
    //    }
    //    else
    //    {
    //        KeyDownCount = 0;
    //        nowWaitKeyDownTwiceTimed = 0;
    //        moveState = MoveState.walk;
    //    }
    }

    //void KeepKeyUP()
    //{
    //    bool _up = PlayerControl_koroshi.KeyUp(playerControl_scrObj.keyUp);
    //    bool _down = PlayerControl_koroshi.KeyUp(playerControl_scrObj.keyDown);
    //    bool _left = PlayerControl_koroshi.KeyUp(playerControl_scrObj.keyLeft);
    //    bool _right = PlayerControl_koroshi.KeyUp(playerControl_scrObj.keyRight);

    //    Debug.Log(_up + "/" + _down + "/" + _left + "/" + _right);

    //    //if (PlayerControl_koroshi.KeyUp(playerControl_scrObj.keyUp) && PlayerControl_koroshi.KeyUp(playerControl_scrObj.keyDown) &&
    //    //    PlayerControl_koroshi.KeyUp(playerControl_scrObj.keyLeft) && PlayerControl_koroshi.KeyUp(playerControl_scrObj.keyRight))
    //    if (_up && _down && _left && _right)
    //    {
    //        nowWaitKeyDownTwiceTimed = 0;
    //        KeyDownCount = 0;
    //        moveState = MoveState.walk;
    //    }
    //    //if (nowKey == _newkey && KeyDownCount > 1)
    //}

    /*
    public void Move1()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Boat[0].transform.rotation = Quaternion.Euler(0,0,0);
            transform.Translate(Vector3.forward *OP.speed1 * Time.deltaTime);
            //anim.SetFloat("moveSpeed_T", Mathf.Lerp(1f, 0f, 0.1f));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Boat[0].transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector3.back * OP.speed1 * Time.deltaTime);
            //anim.SetFloat("moveSpeed_T", Mathf.Lerp(-1f, 0f, 0.1f));
        }
        else
        {
            //anim.SetFloat("moveSpeed_T", Mathf.Lerp(0f, 0f, 0.1f));
        }

        if (Input.GetKey(KeyCode.A))
        {
            Boat[0].transform.rotation = Quaternion.Euler(0, -90, 0);
            transform.Translate(Vector3.left * OP.speed1 * Time.deltaTime);
            //anim.SetFloat("moveSpeed_T", Mathf.Lerp(-2f, -1f, 0.1f));

        }
        else if (Input.GetKey(KeyCode.D))
        {
            Boat[0].transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.Translate(Vector3.right * OP.speed1 * Time.deltaTime);
            //anim.SetFloat("moveSpeed_T", Mathf.Lerp(2f, 1f, 0.1f));

        }
    }
    public void Move2()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Boat2[0].transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector3.forward * OP.speed2 * Time.deltaTime);
            //anim.SetFloat("moveSpeed_T", Mathf.Lerp(1f, 0f, 0.1f));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Boat2[0].transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector3.back * OP.speed2 * Time.deltaTime);
            //anim.SetFloat("moveSpeed_T", Mathf.Lerp(-1f, 0f, 0.1f));
        }
        else
        {
            //anim.SetFloat("moveSpeed_T", Mathf.Lerp(0f, 0f, 0.1f));
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Boat2[0].transform.rotation = Quaternion.Euler(0, -90, 0);
            transform.Translate(Vector3.left * OP.speed2 * Time.deltaTime);
            // anim.SetFloat("moveSpeed_T", Mathf.Lerp(-2f, -1f, 0.1f));

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Boat2[0].transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.Translate(Vector3.right * OP.speed2 * Time.deltaTime);
            // anim.SetFloat("moveSpeed_T", Mathf.Lerp(2f, 1f, 0.1f));
        }
    }
    */

    public void Result_Text()
    {
       //win.SetActive(true);
        gameObject.SetActive(false);
    }
}
