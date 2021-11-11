using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archery_Slide : MonoBehaviour
{
    [SerializeField] Archery_Item item;
    public void Slide()
    {
        StartCoroutine(Sliding());
    }
    IEnumerator Sliding()
    {
        item.player.isSlide = true;
        float i = 0;

        while (i <= 3f)
        {
            i += Time.deltaTime;


            item.player.transform.GetChild(0).Rotate(new Vector3(0, Time.deltaTime * 1250, 0));

            yield return null;
        }
        item.player.isSlide = false;
        item.player.transform.GetChild(0).eulerAngles = item.player.transform.eulerAngles;
        yield return null;
    }
}