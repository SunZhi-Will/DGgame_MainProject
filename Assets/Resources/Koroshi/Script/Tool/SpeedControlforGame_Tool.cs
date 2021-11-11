using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//遊戲速度控制
//Koroshi
public class SpeedControlforGame_Tool : MonoBehaviour
{
    public float speed = 1;
    void Start()
    {
        DontDestroyOnLoad(gameObject);   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            speed = 1;
        }
        Time.timeScale = speed;
    }
}
