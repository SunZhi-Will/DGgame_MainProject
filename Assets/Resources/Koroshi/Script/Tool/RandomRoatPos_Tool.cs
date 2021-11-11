using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Koroshi
//隨機旋轉與位置for UI
public class RandomRoatPos_Tool : MonoBehaviour
{
    public bool RandomPos = true, RandomRoat = true;
    RectTransform rectTra;
    [System.Serializable]
    public struct Range
    {
        public float mini, max;
    }
    [System.Serializable]
    public struct RoatData
    {
        public Range x,y,z,w;
    }
    public RoatData roatData;
    [System.Serializable]
    public struct PosData
    {
        public Range x, y;
    }
    public PosData posData;
    void Awake()
    {
        rectTra = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (RandomPos)
        {
            rectTra.anchoredPosition = new Vector2(
                Random.Range(posData.x.mini, posData.x.max),
                Random.Range(posData.y.mini, posData.y.max));
        }
        if (RandomRoat)
        {
            rectTra.eulerAngles = new Vector3(
                Random.Range(roatData.x.mini, roatData.x.max),
                Random.Range(roatData.y.mini, roatData.y.max),
                Random.Range(roatData.z.mini, roatData.z.max));
            //rectTra.eulerAngles = new Quaternion(
            //    Random.Range(roatData.x.mini, roatData.x.max),
            //    Random.Range(roatData.y.mini, roatData.y.max),
            //    Random.Range(roatData.z.mini, roatData.z.max),
            //    Random.Range(roatData.w.mini, roatData.w.max));
        }
    }
}
