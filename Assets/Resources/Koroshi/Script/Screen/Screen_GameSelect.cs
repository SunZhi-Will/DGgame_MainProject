using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Koroshi
//遊戲選擇
public class Screen_GameSelect : MonoBehaviour
{
    public MainScene_UI mainScene_UI;

    [System.Serializable]
    public struct GameMessage 
    {
        public Text title_text;//關卡標題
        public Image screen_image;//遊戲縮圖畫面
        public Text content_text;//關卡說明文字
    }
    public GameMessage gameMessage;
   

    //移動攝影機
    public GameObject 
        Title_CMvcam1, //標題時攝影機
        GameSelect_CMvcam1;//關卡選擇時攝影機

    public Button LeftSelect_btn, RightSelect_btn;//左右選擇

    [System.Serializable]
    public struct GameIntroduction 
    {
        public GameIntroduction_ScripbObject scrObj;
    }
    //關卡介紹資訊
    public GameIntroduction[] gameIntroduction;
    public int nowSelect;//目前選擇

    public Screen_SelectCharacter screen_SelectCharacter_scr;//角色選擇

    //--學生--
    //public GameManage m_gamemanage;
    private MainCameraContorll m_maincameracontroll;//攝影機工具
    private Change m_change;//轉換場景工具
    //--------
    public Screen_PkSetupMenu screen_PkSetupMenu;//Pk場景(可能會需要在同一場景)
    public string LoadSceneName = "PkMenu";

    void OnEnable()
    {
        if (GameMaster_koroshi.s_GameMaster == null) { Debug.LogError("GameMaster_koroshi.s_GameMaster 沒有設定!"); }

        OpenCamera(true);
        //選擇關卡時，不用出現2P玩家按鍵提示
        mainScene_UI.OpenKeyTip(PlayerNumber.p2, GameKeyTipState.Confirm, false);
        mainScene_UI.OpenKeyTip(PlayerNumber.p2, GameKeyTipState.Cancel, false);
    }

    void Start()
    {
        LeftSelect_btn.onClick.AddListener(() => { ChangeSelectGame(-1); });
        RightSelect_btn.onClick.AddListener(() => { ChangeSelectGame(+1); });
        m_change = FindObjectOfType<Change>();
        m_maincameracontroll= FindObjectOfType<MainCameraContorll>();

        //初始化上一次選擇
        nowSelect = GameMaster_koroshi.s_GameMaster.data.nowSelectGameNumber;
        ChangeSelectGame(0);
    }

    void Update()
    {
        if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Left))
        {
            ChangeSelectGame(-1);
        }
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Right))
        {
            ChangeSelectGame(+1);
        }
        //確認
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Confirm))
        {    
            GameMaster_koroshi.s_GameMaster.data.nowSelectGame = gameIntroduction[nowSelect].scrObj;//確認目前選擇遊戲
            GameMaster_koroshi.s_GameMaster.data.nowSelectGameNumber = nowSelect;//確認目前選擇遊戲編號
            screen_PkSetupMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);

            Debug.Log("目前選擇遊戲編號:" + "\n" + nowSelect);
            Debug.Log("確認選擇遊戲" + "\n" + gameIntroduction[nowSelect].scrObj.name);
            AudioManager.s_AudioManager.PlayAudio_Click();

            //原本用轉換場景的方法
            //m_gamemanage.GameIndex = nowSelect;//確認選擇
            //m_change.ChangeScene(LoadSceneName);//轉換至設定場景
        }
        //取消
        else if (PlayerControl_koroshi.KeyDown(PlayerNumber.p1, PlayerKeyName.Cancel) || PlayerControl_koroshi.KeyDown(PlayerKeyName.Mouse1) || PlayerControl_koroshi.Escape_keyDown)
        {
            screen_SelectCharacter_scr.gameObject.SetActive(true);
            gameObject.SetActive(false);
            ResetData();
            Debug.Log("返回角色選擇");
        }
    }

    //切換選擇遊戲
    void ChangeSelectGame(int _updataSelect)
    {
        int _predictionUpdate = nowSelect + _updataSelect;//更新預測，檢測是否超出陣列
        if (_updataSelect > 0) 
        {
            if (_predictionUpdate >= gameIntroduction.Length) { _predictionUpdate = 0; }//超出最大則歸零(循環)
        }
        else if (_updataSelect < 0) 
        {
            if (_predictionUpdate < 0) { _predictionUpdate = gameIntroduction.Length - 1; }//底於最小則回到最大(循環)
        }
        nowSelect = _predictionUpdate;//更新選擇
        //更新攝影機
        m_maincameracontroll.ChangeGameCamera(nowSelect);
        //更新縮圖畫面，與顯示文字內容
        GameIntroduction_ScripbObject _scrObj = gameIntroduction[nowSelect].scrObj;
        gameMessage.title_text.text = _scrObj.TitleName;
        gameMessage.screen_image.sprite = _scrObj.MainsSreen;
        gameMessage.content_text.text = _scrObj.Content;
        AudioManager.s_AudioManager.PlayAudio_Move();
    }

    //開啟攝影機
    void OpenCamera(bool _enabled)
    {
        if (Title_CMvcam1 == null) { return; }
        if (GameSelect_CMvcam1 == null) { return; }
        Title_CMvcam1.SetActive(!_enabled);
        GameSelect_CMvcam1.SetActive(_enabled);
    }

    //取消 遊戲選擇 返回時，重置設定
    void ResetData()
    {
        //選擇關卡時，不用出現2P玩家按鍵提示
        mainScene_UI.OpenKeyTip(PlayerNumber.p2, GameKeyTipState.Confirm, true);
        mainScene_UI.OpenKeyTip(PlayerNumber.p2, GameKeyTipState.Cancel, true);
        OpenCamera(false);
    }

    void OnDisable()
    {
       
    }
}
