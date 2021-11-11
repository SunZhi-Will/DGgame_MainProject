using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//填入遊戲關卡資訊用
[CreateAssetMenu(fileName = "GameIntroduction", menuName = "Koroshi/遊戲介紹訊息")]
public class GameIntroduction_ScripbObject : ScriptableObject
{
    [Header("關卡名稱")]
    public string TitleName = "關卡名稱";
    [Header("對應場景名稱")]
    public string SceneName;
    public Sprite MainsSreen;//主要圖片
    [TextArea(0, 3)]
    [Header("遊戲簡單概述")]
    public string Content;
    [TextArea(0, 10)]
    [Header("遊戲規則詳細說明")]
    public string GameDetails;

    [System.Serializable]
    public class SetupData
    {
        [Header("是否啟用設定")]
        public bool enabled = true;
        [Header("預設值")]
        public int DefaultValue = 0;
        [Header("最小值")]
        public int miniValue = 0;
        [Header("最大值")]
        public int maxValue = 0;
        [Header("增加間隔")]
        public int IntervalValue = 2;
    }
    public SetupData RoundSetup = new SetupData();
    public SetupData TimedSetup = new SetupData();



}
