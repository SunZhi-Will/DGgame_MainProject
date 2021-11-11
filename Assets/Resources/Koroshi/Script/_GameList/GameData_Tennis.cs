using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//網球-遊戲資料
//-同一個場景的話，盡量用static
//-結算別放Boss身上
//-角色控制器不要綁名子抓是誰，根據腳本放置在在誰身上，判斷其底下的子物件來抓，或者Debug顯示警告，否則很容易一個小改動，就導致抓不到名稱，控制器錯亂
//-用搜索還是需要另外判斷持有者是誰，因為不能保證100%搜索到的順序是固定的
//-UI的部分盡量別用算的，除非要做針對不同視窗的適應，固定視窗的話調整CanvasScaler上解析度即可，因為這樣如果亂移動父子物件就會錯亂，除非把父子物件變更後也算進去
//-空物件 ObjectPool 行號155 在回收的時候似乎混入了原始物件導致出現空包彈，詳細的自己去調查囉~
//-子彈生成建議可以用 C# Queue(上網查囉)，他有的「先進先出規則，而且不用設定移除編號，可以直接取出
//PS:花比較多時間的部分基本上都在綁死名稱難調整、UI父子物件綁死，生成物件與遊戲系統管理器在不同地方要來回兩腳本找來找去上Orz，
//另外ObjectCreate腳本中Ball用不到的其實可以砍掉...不然整合的時候以為那個是主生成物件測試半天才發現不是...
public class GameData_Tennis : GameData_system
{
    public int Timed = 300; 
    public ObjectCreate objectCreate_scr;
    public CharacterChange characterChange_scr;//角色切換系統
    public UnityEngine.UI.Image bar_image;//飽食度

    //遊戲系統
    public ObjectCreate GameSystem_scr;
    private bool pause = true;

    protected override void f_Awake()
    {
        if (GameMaster_koroshi.s_GameMaster != null)
        {
            GameMaster_koroshi.Data _data = GameMaster_koroshi.s_GameMaster.data;
            objectCreate_scr.game_time = (int)_data.Timed;
        }
        else
        {
            objectCreate_scr.game_time = Timed;
            characterChange_scr = GetComponent<CharacterChange>();
        }
    }

    protected override void f_Start()
    {
    }
    protected override void f_Start_StartTimedEvent()
    {
        GameSystem_scr.playGame();//自動開始遊戲
    }
    
    void Update()
    {
        TestFunction();

        if (Time.timeScale == 0)
        {
            if (pause == true) { return; }
            PauseAudio(true);
            pause = true;
        }
        else
        {
            if (pause == false) { return; }
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
            GameSystem_scr.game_time = 1;
            Debug.Log("測試功能-失敗畫面");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            GameSystem_scr.game_time = 1;
            bar_image.fillAmount = 1;
            Debug.Log("測試功能-勝利畫面");
        }
#endif
    }
}
