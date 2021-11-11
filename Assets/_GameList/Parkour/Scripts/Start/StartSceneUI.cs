using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneUI : MonoBehaviour
{
    public Button Bt1;
    public Button Bt2;
    public int rndnum;

    void Start()
    {
        PlayerPrefs.SetInt("PlayerNum", 1);
        PlayerPrefs.SetInt("WinOrLose", 0);
        rndnum = Random.Range(0, 7);
    }
    public void StartPlay()
    {
        if(Bt1.interactable == false)
        {
            PlayerPrefs.SetInt("PlayerNum", 1);
        }
        else if (Bt2.interactable == false)
        {
            PlayerPrefs.SetInt("PlayerNum", 2);
        }
        switch (rndnum)
        {
            case 0: SceneManager.LoadScene("跑酷");break;
            case 1: SceneManager.LoadScene("跑酷1"); break;
            case 2: SceneManager.LoadScene("跑酷2"); break;
            case 3: SceneManager.LoadScene("跑酷3"); break;
            case 4: SceneManager.LoadScene("跑酷4"); break;
            case 5: SceneManager.LoadScene("跑酷5"); break;
            case 6: SceneManager.LoadScene("跑酷6"); break;
            default: SceneManager.LoadScene("跑酷"); break;
        }
        
    }
}
