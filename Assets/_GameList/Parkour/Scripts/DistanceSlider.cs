using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceSlider : MonoBehaviour
{
    //public GameObject Boss;
    //public Slider slider;

    public Transform Boss_tra;//只有用transform的話直接宣告Transform能稍微省資源消耗
    public Image bar_image;//進度表
    public RectTransform triangle_rectTra;//箭頭物件
    private float barWidth;

    void Start()
    {
        barWidth = bar_image.rectTransform.sizeDelta.x;//擷取進度條寬度
    }

    void Update()
    {
        //slider.value = (Boss.transform.position.z + 5) / 400;

        bar_image.fillAmount = (Boss_tra.transform.position.z + 5) / 400;
        Vector3 _pos = triangle_rectTra.anchoredPosition;
        triangle_rectTra.anchoredPosition = new Vector2(barWidth * bar_image.fillAmount ,_pos.y);
    }
}
