using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KandoManage : MonoBehaviour
{

    public static KandoManage instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public int round = 1;

    public int time = 60;

}
