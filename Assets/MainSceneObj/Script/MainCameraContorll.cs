using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCameraContorll : MonoBehaviour
{
    [SerializeField]
    GameObject m_CMvcam1, m_CMvcam2;
    [SerializeField]
    Transform[] m_allcmtargets;
    CinemachineBrain m_cinemachinebrain;
    CinemachineVirtualCamera[] m_allcinemachinecamera;
    CinemachineTrackedDolly m_dollytracked;
    void Start()
    {
        m_cinemachinebrain = GetComponent<CinemachineBrain>();
        m_allcinemachinecamera = new CinemachineVirtualCamera[2];
        m_allcinemachinecamera[0] = m_CMvcam1.GetComponent<CinemachineVirtualCamera>();
        m_allcinemachinecamera[1] = m_CMvcam2.GetComponent<CinemachineVirtualCamera>();
        m_allcinemachinecamera[1].m_LookAt = m_allcmtargets[0];
        m_dollytracked = m_allcinemachinecamera[1].GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    public void ChangeGameCamera(int gameIndex)
    {
        m_allcinemachinecamera[1].m_LookAt = m_allcmtargets[gameIndex];
        m_dollytracked.m_PathPosition = gameIndex;
    }
}
