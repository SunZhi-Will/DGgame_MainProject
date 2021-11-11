using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archery_Bomb : MonoBehaviour
{
    BoxCollider collider;
    Vector3 size;
    [SerializeField] Archery_Item item;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        size = collider.bounds.size;

        Invoke("Bomb",3);
    }
    Collider[] Get()
    {
       
        Collider[] colliders = Physics.OverlapBox(transform.position, size * 0.5f, Quaternion.identity);
        return colliders;
    }
    void Bomb()
    {
        item.onHit.Invoke();

        if(Get().Length <= 0)
        {
            item.Damage(10);
            return;
        }
        bool temp = false;
        foreach (Collider a in Get())
        {
            if (a.transform == item.player.transform)
            {
                temp |= true;
            }
        }

        if(!temp) { item.Damage(10); }
    }
}
