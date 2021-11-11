using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archery_Score : MonoBehaviour
{
    public GameData_ShotArchery GameData_ShotArchery_scr;
    [SerializeField] Archery_Health player1, player2, boss;
    [SerializeField] Archery_Player[] player;

    [SerializeField] UnityEngine.UI.Text player1Score, player2Score;
    [SerializeField] UnityEngine.UI.Image player1Score_bar, player2Score_bar;
    [SerializeField] UnityEngine.UI.Image bossHealthBar;

    public void score()
    {
        //player1Score.text = player1.health.ToString("0");
        //player2Score.text = player2.health.ToString("0");
        player1Score_bar.fillAmount = player1.health / player1.maxHealth;//顯示血量
        player2Score_bar.fillAmount = player2.health / player2.maxHealth;//顯示血量
        bossHealthBar.fillAmount = boss.health / boss.maxHealth;

        Debug.Log("Player1 : " + player1.health + "/" + player1.maxHealth + "    Player2 : " + player2.health + "/" + player2.maxHealth + "    Boss : " + boss.health + "/" + boss.maxHealth);

        if (player1.health <= 0)
        {
            GameData_ShotArchery_scr.ShowSettlement(0, 1);
            Debug.LogError("Player2 Win");
            //Time.timeScale = 0.1f;
        }
        else if (player2.health <= 0)
        {
            GameData_ShotArchery_scr.ShowSettlement(1, 0);
            Debug.LogError("Player1 Win");
            //Time.timeScale = 0.1f;
        }

        if (boss.health <= 0)
        {
            if (boss.transform.GetChild(boss.transform.childCount - 1).transform.GetComponent<Archery_Arrow>().player == player[0])
            {
                GameData_ShotArchery_scr.ShowSettlement(1, 0);
                Debug.LogError("Player1 Win");
                //Time.timeScale = 0.1f;
            }
            else
            {
                //GameData_ShotArchery_scr.ShowSettlement(0, 1);
                GameData_ShotArchery_scr.ShowSettlement(0, 1);
                Debug.LogError("Player2 Win");
                //Time.timeScale = 0.1f;
            }
        }
    }
}
