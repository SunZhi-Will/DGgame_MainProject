using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//UI跟3D角色或場景隨物件
public class UITargetFollow_Tool : MonoBehaviour
{
    private Camera cam;
    public RectTransform UI_rectTra;
    [SerializeField]
    private Transform Target_traj;
    public Vector3 OffectPos;


    private void Awake()
    {
        cam = Camera.main;
        if (cam == null) { Debug.LogError(gameObject.name + "\n" + "錯誤!目前場景不存在主攝影機"); }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Target_traj == null) { return; }
        Vector3 _pos = Target_traj.position;
        UI_rectTra.position = cam.WorldToScreenPoint(new Vector3(_pos.x + OffectPos.x, _pos.y + OffectPos.y, _pos.z + OffectPos.z));
    }

    public void SetTarget(Transform _tra)
    {
        Target_traj = _tra;
    }

}
