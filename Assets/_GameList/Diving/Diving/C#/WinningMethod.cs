using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinningMethod : MonoBehaviour
{
    [SerializeField]
    [Header("Win角色替換")]
    private GameObject[] m_Win_Play;
    [SerializeField]
    [Header("Lose角色替換")]
    private GameObject[] m_Lose_Play;

    [SerializeField]
    [Header("WinText")]
    private Text m_Win_Text;
    [SerializeField]
    [Header("LoseText")]
    private Text m_Lose_Text;

    public void Winner(bool _P1){
        GameObject.Find("Main").GetComponent<ChangeRole>().Change(m_Win_Play, _P1);
        if(!_P1){
            //GameObject.Find("Main").GetComponent<ChangeRole>().SetChange(m_Lose_Play, m_Win_Play);
            m_Win_Text.text = "P2";
            m_Win_Text.color = Color.red;
            m_Lose_Text.text = "P1";
            m_Lose_Text.color = Color.cyan;
        }
        StartCoroutine(DelayDisappears());
    }
    private IEnumerator DelayDisappears(){
        yield return new WaitForSeconds(0.5f);
        foreach (var item in m_Win_Play){
            item.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("InhaleAndExhale", true);
        }
    }
}
