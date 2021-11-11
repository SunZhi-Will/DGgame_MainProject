using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//角色切換-掛在外部(共通模組)
public class CharacterChange : MonoBehaviour
{
    public bool AwakeOpen = false;//Awake喚醒模式
    public int test_number_p1 = 0, test_number_p2 = 1;//單場景測試模式用編號
    public float StartTimed = 0.2f;
    public GameObject p1_ch_obj { get { return _p1_ch_obj; } }
    public GameObject p2_ch_obj { get { return _p2_ch_obj; } }
    [SerializeField]
    private GameObject[] p1_obj_array, p2_obj_array;
    private GameObject _p1_ch_obj, _p2_ch_obj;

    private void Awake()
    {
        if (AwakeOpen == false) { return;}   
        InitCharacter();
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(StartTimed);
        if (AwakeOpen == false)
            InitCharacter();
    }
    private void InitCharacter()
    {
        //開啟角色
        GameMaster_koroshi _gameData_scr = GameMaster_koroshi.s_GameMaster;
        if (_gameData_scr != null)
        {
            for (int i = 0; i < p1_obj_array.Length; i++)
            {
                p1_obj_array[i].SetActive(false);
            }
            for (int i = 0; i < p2_obj_array.Length; i++)
            {
                p2_obj_array[i].SetActive(false);
            }
            _p1_ch_obj = p1_obj_array[_gameData_scr.p1_SelectCharacter];
            _p2_ch_obj = p2_obj_array[_gameData_scr.p2_SelectCharacter];
        }
        else
        {
            _p1_ch_obj = p1_obj_array[test_number_p1];
            _p2_ch_obj = p2_obj_array[test_number_p2];
        }

        _p1_ch_obj.SetActive(true);
        _p2_ch_obj.SetActive(true);
    }

    void Update()
    {
        
    }
}
