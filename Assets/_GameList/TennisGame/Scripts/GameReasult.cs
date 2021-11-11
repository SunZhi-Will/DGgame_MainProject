using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReasult : MonoBehaviour
{
    public GameData_Tennis gameData_Tennis;
    public GameObject[] p_b,Wall;
    public GameObject lose;
    public Animator[] pbA;
    // Start is called before the first frame update


    //private Transform player1_tra, player2_tra;
    //public Transform player1EndPos_tra, player2EndPos_tra;//終點位置物件(外部放入用)
    //private Vector3 player1EndPos, player2EndPos;//終點位置
    //public float MoveEndSpeed = 10;
    //private bool entetWin;
    //private bool[] moveOver = new bool[2];

    //IEnumerator Start()//Unity繼承MonoBehaviour除了內建的void Start還有IEnumerator
    //{
    //    yield return new WaitForSeconds(5);
    //    player1_tra = gameData_Tennis.characterChange_scr.p1_ch_obj.transform;//暫存起來
    //    player2_tra = gameData_Tennis.characterChange_scr.p2_ch_obj.transform;
    //    player1EndPos = player1EndPos_tra.position;//position比localPosition更耗效能，暫存能減少消耗
    //    player2EndPos = player2EndPos_tra.position;
    //}

    void Update()
    {
        //if (!entetWin) { return; }
        
        //if (Vector3.Distance(player1_tra.position, player1EndPos) > 0.2f && moveOver[0])
        //    player1_tra.position = Vector3.MoveTowards(player1_tra.position, player1EndPos, MoveEndSpeed);
        //else
        //    moveOver[0] = true;
        //if (Vector3.Distance(player2_tra.position, player2EndPos) > 0.2f && moveOver[1])
        //    player2_tra.position = Vector3.MoveTowards(player1_tra.position, player2EndPos, MoveEndSpeed);
        //else
        //    moveOver[1] = true;
    }
    public void Win()
    {
        //entetWin = true;
        foreach (var g in p_b) {
            g.GetComponent<Animator>().applyRootMotion = false;
        }
        foreach (var w in Wall)
        {
            w.SetActive(false);
        }
        foreach (var a in pbA)
        {
            a.applyRootMotion = false;//還是沒加RRRRR
            a.SetTrigger("Win");
        }
        gameData_Tennis.Common_scr.ShowSettlement(true);
    }
    public void Lose(int n)
    {
        p_b[n].SetActive(false);
    }
    public void Result_Text()
    {
        gameData_Tennis.Common_scr.ShowSettlement(false);
        //lose.SetActive(true);
    }
}
