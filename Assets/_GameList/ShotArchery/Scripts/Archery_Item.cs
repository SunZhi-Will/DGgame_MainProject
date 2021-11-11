using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Archery_Item : MonoBehaviour
{
    public Archery_Player player;

    public UnityEvent onHit;
    public UnityEvent onGround;

    public void Damage(float damage)
    {
        player.health.HealthChange(damage);
    }

    public void  Arrow()
    {
        player.strongArrow += 1;
        player.arrowStr.Play();
    }
    public void OnGround()
    {
        if (Get().Length <= 0) { return; }
        foreach (Collider a in Get())
        {
            if (a.transform == player.transform)
            {
                onHit.Invoke();
                transform.parent.GetComponent<Archery_ItemThrowing>().enabled = false;
            }
        }
    }
    Collider[] Get()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 size = collider.bounds.size;
        Collider[] colliders = Physics.OverlapBox(transform.position, size * 0.5f, Quaternion.identity);
        return colliders;
    }    

    public void HealthAdd()
    {
        player._health[0].SetActive(false);
        player._health[0].SetActive(true);
        player._health[1].SetActive(false);
        player._health[1].SetActive(true);
    }
}
