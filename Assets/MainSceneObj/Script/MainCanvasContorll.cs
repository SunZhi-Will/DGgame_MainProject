using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasContorll : MonoBehaviour
{
    [SerializeField]
    GameObject m_meun, m_optioncanvas, m_gamelist, m_CMvcam1, m_CMvcam2;
    void Start()
    {
        
    }

    #region Button
    public void StartButton()
    {
        m_meun.SetActive(false);
        m_gamelist.SetActive(true);
        m_CMvcam1.SetActive(false);
        m_CMvcam2.SetActive(true);
    }

    public void OptionsButton()
    {
        m_optioncanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    #endregion

}
