using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//劍道
public class GameData_Kendo : GameData_system
{
    public UITargetFollow_Tool p1_PowerUI, p2_PowerUI;//玩家1.2能量條
    private CharacterChange characterChange_scr;
    private KandoGameManage kandoGameManage_scr;
    [Header("死亡牆物件")]
    public Transform Die_tra;

    private bool pause;
    public List<playerAI> playerAI_list_scr;

    public int round;
    public int time;//為啥時間是int RRRRRRRR

    protected override void f_Awake()
    {
        if (GameMaster_koroshi.s_GameMaster != null) 
        {
            round = GameMaster_koroshi.s_GameMaster.data.Round;
            time = (int)GameMaster_koroshi.s_GameMaster.data.Timed;
        }
        characterChange_scr = GetComponent<CharacterChange>();

    }
    protected override void f_Start()
    {
        kandoGameManage_scr = FindObjectOfType<KandoGameManage>();

        playerAI[] array_scr = FindObjectsOfType<playerAI>();
        //playerAI_list_scr = new List<playerAI>();
        //for (int i = 0; i < array_scr.Length; i++)
        //{
        //    playerAI _scr = array_scr[i];
        //if (_scr.enabled)
        //    playerAI_list_scr.Add(_scr);
        //}
    }
    protected override void f_Start_StartTimedEvent()
    {
        playerAI_list_scr = new List<playerAI>();
        playerAI _playerAI = characterChange_scr.p1_ch_obj.GetComponent<playerAI>();
        if (_playerAI == null) { Debug.LogError(gameObject.name + "\n" + characterChange_scr.p1_ch_obj + " 不存在 playerAI"); }
        playerAI_list_scr.Add(_playerAI);
        if (_playerAI == null) { Debug.LogError(gameObject.name + "\n" + characterChange_scr.p2_ch_obj + " 不存在 playerAI"); }
        playerAI_list_scr.Add(characterChange_scr.p2_ch_obj.GetComponent<playerAI>());
        //設定角色
        kandoGameManage_scr.SetPlayerObject(characterChange_scr.p1_ch_obj, characterChange_scr.p2_ch_obj);
        kandoGameManage_scr.enabled = true;
        p1_PowerUI.SetTarget(characterChange_scr.p1_ch_obj.transform);
        p2_PowerUI.SetTarget(characterChange_scr.p2_ch_obj.transform);
    }

    void Update()
    {
        TestFunction();

        if (Time.timeScale == 0)
        {
            if (pause == true) { return; }
            for (int i = 0; i < playerAI_list_scr.Count; i++)
            {
                playerAI_list_scr[i].enabled = false;
            }
            PauseAudio(true);
            pause = true;
        }
        else
        {
            if (pause == false) { return; }
            for (int i = 0; i < playerAI_list_scr.Count; i++)
            {
                playerAI_list_scr[i].enabled = true;
            }
            PauseAudio(false);
            pause = false;
        }

    }


    //測試功能
    void TestFunction()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            playerAI_list_scr[1].transform.position = Die_tra.position;
            kandoGameManage_scr.time_g = 1;
            Debug.Log("測試功能-1P Win畫面");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            playerAI_list_scr[0].transform.position = Die_tra.position;
            kandoGameManage_scr.time_g = 1;
            Debug.Log("測試功能-2P Win畫面");
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            kandoGameManage_scr.time_g = 2;
            Debug.Log("測試功能-時間到");
        }
#endif
    }

}
