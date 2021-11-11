using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//淡出入工具
public class FadeInOut_Tool : MonoBehaviour
{
    public FadeInOut_Tool Syne_fadeInOut;//同步物件淡出入(網球真的很搞Orz)
    private GameCommon gameCommon_scr;//淡出後再開啟
    public float startWaitTimed = 0.5f;
    public float Speed = 0.5f;
    private CanvasGroup CanvasGroup_scr;
    public enum State
    {
        none,
        fadeIn,
        fadeOut,
    }
    private State stateMode;//目前狀態

    public State startSetupMode = State.none;//設定開始時執行模式狀態

    private void Awake()
    {
        gameCommon_scr = FindObjectOfType<GameCommon>();
        if (gameCommon_scr == null) { Debug.LogError(gameObject.name+"\n"+"場景不存在 GameCommon 組件"); }
        CanvasGroup_scr = GetComponent<CanvasGroup>();
        if (CanvasGroup_scr == null) { Debug.LogError("目前物件不存在 CanvasGroup"); }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(startWaitTimed);
        FadeInOutState(startSetupMode);
    }

    //淡出入狀態切換
    public void FadeInOutState(State _newState)
    {
        //switch (_newState) 
        //{
        //    //淡入時，重置為淡出
        //    case State.fadeIn:
        //        CanvasGroup_scr.alpha = 0;
        //        break;
        //    //反之亦然
        //    case State.fadeOut:
        //        CanvasGroup_scr.alpha = 1;
        //        break;
        //}
        stateMode = _newState;
        gameObject.SetActive(true);
    }
    void Update()
    {
        switch (stateMode)
        {
            case State.fadeIn:
                CanvasGroup_scr.alpha = Mathf.Lerp(CanvasGroup_scr.alpha, 1, Speed);
                if (CanvasGroup_scr.alpha == 1)
                {
                    startSetupMode = stateMode = State.none;
                }
                break;
            case State.fadeOut:
                CanvasGroup_scr.alpha = Mathf.Lerp(CanvasGroup_scr.alpha, 0, Speed);
                if (CanvasGroup_scr.alpha == 0)
                {
                    startSetupMode = stateMode = State.none;
                }
                //待改善
                if (CanvasGroup_scr.alpha <= 0.001f)
                {
                    //淡出後開啟物件
                    if (gameCommon_scr.enabled == false)
                        gameCommon_scr.enabled = true;
                }
                break;
        }
        if (Syne_fadeInOut == false) { return; }
        Syne_fadeInOut.FadeInOutState(stateMode);

    }
}
