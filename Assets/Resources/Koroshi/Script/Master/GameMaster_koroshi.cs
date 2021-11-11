
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Koroshi
//全遊戲管理
//音效管理(本來要切的，但看來目前用不到)-partial
//Windown -> Rendering -> Occlusion Culling(在某些遊戲場景可以看到有趣的東西www)
public partial class GameMaster_koroshi : MonoBehaviour
{
    public static GameMaster_koroshi s_GameMaster { get { return gameMaster_koroshi; } }
    private static GameMaster_koroshi gameMaster_koroshi;

    //[SerializeField]
    //private AudioSource audioSource;
    private int maxModelCount = 3;//模型最大角色數-切換異色用

    //選擇角色
    //1P角色//待改善
    public int p1_SelectCharacter
    {
        get
        {
            if (data.p1_SelectCharacter < maxModelCount)
                return data.p1_SelectCharacter;
            else
                return data.p1_SelectCharacter - maxModelCount;
        }
    }
    //2P角色 //待改善
    public int p2_SelectCharacter
    {
        get
        {
            if (data.p2_SelectCharacter < maxModelCount)
                return data.p2_SelectCharacter;
            else
                return data.p2_SelectCharacter - maxModelCount;
        }
    }
    //角色頭像
    public Sprite p1_Head { get { return Character_Head_array[data.p1_SelectCharacter]; } }
    public Sprite p2_Head { get { return Character_Head_array[data.p2_SelectCharacter]; } }
    //目前遊戲設定資料
    [System.Serializable]
    public struct Data
    {
        public int nowSelectGameNumber;//回到主畫面時，顯示對應攝影機畫面用
        public GameIntroduction_ScripbObject nowSelectGame;//目前選擇遊戲
        public int Round;//回合數
        public float Timed;//時間
      
        public int p1_SelectCharacter;
        public int p2_SelectCharacter;

        public bool FullOrWindowns;//視窗化與全螢幕狀態
        public float AudioValue;//音量大小
    }
    public Data data;

    [Header("1P 角色清單")] [SerializeField]
    private GameObject[] Character_1P_array;
    [Header("2P 角色清單")][SerializeField]
    private GameObject[] Character_2P_array;
    [Header("角色頭像清單")][SerializeField]
    private Sprite[] Character_Head_array;

    public string TitleSceneName = "Main";

    public int TestRunData;//跑酷用測試資料

    private void Awake()
    {
        if (gameMaster_koroshi == null)
        {
            gameMaster_koroshi = this;

            InitSetupValue();
            DontDestroyOnLoad(gameObject);

            //預設角色選擇
            data.p1_SelectCharacter = 0;
            data.p2_SelectCharacter = 1;
        }
        else
        {
            Destroy(gameObject);//防止重複
            return;
        }

        if (Character_1P_array == null) { Debug.LogError("Character_array 尚未設定角色列表"); }
        if (Character_1P_array.Length <= 0) { Debug.LogError("Character_array 角色列表為0"); }
        if (Character_2P_array == null) { Debug.LogError("Character_array 尚未設定角色列表"); }
        if (Character_2P_array.Length <= 0) { Debug.LogError("Character_array 角色列表為0"); }
    }

    //初始化數值
    void InitSetupValue()
    {
        //初始化數值
        Screen.fullScreen = data.FullOrWindowns = true;//全螢幕
        data.AudioValue = 5;
    }

    void Start()
    {
    }
    private void FixedUpdate()
    {
        data.FullOrWindowns = Screen.fullScreen;//有切換全螢幕與視窗化
    }

    void Update()
    {
    }

    //設定選擇角色
    public void SetSelectCharacter(PlayerNumber _playerNumber,int _select)
    {
        switch (_playerNumber)
        {
            case PlayerNumber.p1:
                data.p1_SelectCharacter = _select;
                break;
            case PlayerNumber.p2:
                data.p2_SelectCharacter = _select;
                break;
        }

    }
    //選擇目前角色並生成
    public GameObject NowSelectCharacter_Spawn(PlayerNumber _playerNumber)
    {
        GameObject _obj = null;
        GameObject _spawnObj = null;
        Animator _anim = null;
        int _number = -1;
        switch (_playerNumber)
        {
            case PlayerNumber.p1:
                _number = data.p1_SelectCharacter;
                //異色板切換
                //if (_number > maxModelCount)
                //{
                //    _number -= maxModelCount;
                //    Debug.Log("1P更換異色");
                //}
                _spawnObj = Character_1P_array[_number];
                break;
            case PlayerNumber.p2:
                _number = data.p2_SelectCharacter;
                //異色板切換
                //if (_number > maxModelCount)
                //{
                //    _number -= maxModelCount;
                //    Debug.Log("2P更換異色");
                //}
                _spawnObj = Character_2P_array[_number];
                break;
        }
        
        _obj = Instantiate(_spawnObj);
        _obj.SetActive(true);
        return _obj;
    }
    //模型異色模式切換檢測
    public bool PlayerModelColorChange(PlayerNumber _playerNumber)
    {
        bool _colorChange = false;
        if (_playerNumber == PlayerNumber.p1)
        {
            if (data.p1_SelectCharacter > maxModelCount)
                _colorChange = true;
        }
        else if (_playerNumber == PlayerNumber.p2)
        {
            if (data.p2_SelectCharacter > maxModelCount)
                _colorChange = true;
        }
        return _colorChange;
    }
    
    //離開遊戲
    private void OnApplicationQuit()
    {
        Screen.fullScreen = true;//啊哈~還想切阿~
    }
}
