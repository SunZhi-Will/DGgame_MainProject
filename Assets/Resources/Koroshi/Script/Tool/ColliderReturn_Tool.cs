using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//接觸Collider物件
//-外部掛
public class ColliderReturn_Tool : MonoBehaviour
{
    [SerializeField]
    private string TargetTag = string.Empty;//觸發目標
    public string nowTargetTag;//顯示目前接觸對象(回傳給其他人做其他事情用~)

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TargetTag)
        {
            nowTargetTag = other.tag;
        }
    }
}
