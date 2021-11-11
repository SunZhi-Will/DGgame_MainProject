using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelControll : MonoBehaviour
{
    [SerializeField]
    float m_waveHeight, m_waveSpeed;
    Vector3 nowPoint;
    float m_heightLimit, m_lowLimit;
    bool isUp = true;
    private void Start()
    {
        m_heightLimit = transform.position.y + m_waveHeight;
        m_lowLimit = transform.position.y - m_waveHeight;
        nowPoint = transform.position;
    }
    private void Update()
    {
        if (isUp)
        {
            nowPoint.y += m_waveSpeed;
            if (nowPoint.y >= m_heightLimit)
            {
                nowPoint.y = m_heightLimit;
                isUp = false;
            }
            transform.position = nowPoint;
        }
        else
        {
            nowPoint.y -= m_waveSpeed;
            if (nowPoint.y <= m_lowLimit)
            {
                nowPoint.y = m_lowLimit;
                isUp = true;
            }
            transform.position = nowPoint;
        }
    }

}
