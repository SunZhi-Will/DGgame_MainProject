//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;


//public class RoundCanvasControll : MonoBehaviour
//{
//    public Text Round;

//    public Text Time;
//    GameData_Kendo m_kandomanage;

//    // Start is called before the first frame update
//    void Start()
//    {
//        m_kandomanage = KandoManage.instance;
//    }
//    public void Addround()
//    {
//        if (m_kandomanage.round < 3)
//        {
//            m_kandomanage.round += 1;
//            Round.text = m_kandomanage.round.ToString();
//        }
//    }
//    public void Dtround()
//    {
//        if (m_kandomanage.round > 1)
//        {
//            m_kandomanage.round -= 1;
//            Round.text = m_kandomanage.round.ToString();
//        }
//    }
//    public void AddTime()
//    {
//        if (m_kandomanage.time == 60)
//        {
//            m_kandomanage.time += 30;
//            Time.text = m_kandomanage.time.ToString();
//        }
//    }
//    public void DtTime()
//    {
//        if (m_kandomanage.time == 90)
//        {
//            m_kandomanage.time -= 30;
//            Time.text = m_kandomanage.time.ToString();
//        }
//    }

//    public void changesence()
//    {
//        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
//    }
//}
