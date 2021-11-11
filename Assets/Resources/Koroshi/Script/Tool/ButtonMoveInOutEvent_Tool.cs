using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
//Koroshi
//按鈕移出入工具
public class ButtonMoveInOutEvent_Tool : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    private Action _click_Action, _moveIn_Action, _moveOut_Action;
    private void Start()
    {
        Button _btn = GetComponent<Button>();
        if (_btn == null) { Debug.LogError(gameObject.name+"\n"+"錯誤!沒有按鈕!");  }
        _btn.onClick.AddListener(() => { if (_click_Action != null) { _click_Action(); } });
    }

    //設定單獨一筆事件
    public void SetClickEvent(Action _newAction)
    {
        _click_Action = _newAction;
    }
    //設定移入事件
    public void SetMoveInEvent(Action _newAction)
    {
        _moveIn_Action = _newAction;
    }
    //設定移出事件
    public void SetMoveOutEvent(Action _newAction)
    {
        _moveOut_Action = _newAction;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_moveIn_Action != null) { _moveIn_Action(); }
        Debug.Log("按鈕移入");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_moveOut_Action != null) { _moveOut_Action(); }
        Debug.Log("按鈕移出");
    }
}
