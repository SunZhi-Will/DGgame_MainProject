using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Archery_Arrow : MonoBehaviour
{
    public bool shoot = false;
    Vector3 previousPos;

    [SerializeField]float speed,damage;
    public Archery_Player player;
    [SerializeField]GameObject[] hit;

    public UnityEvent onHit;

    private void Start()
    {
        Invoke("DestoryThis", 2);
    }
    private void Update()
    {
        if (!shoot) { return; }
        previousPos = transform.position;
        transform.Translate(new Vector3(0, 0, 1)*speed*Time.deltaTime);
        Hit();
    }

    void Hit()
    {
        float length = (transform.position - previousPos).magnitude;
        Debug.DrawLine(transform.position, previousPos);
        Vector3 direction = transform.position - previousPos;
        RaycastHit raycastHit;
        if (Physics.Raycast(previousPos, direction, out raycastHit, length))
        {
            Transform hits = raycastHit.collider.transform;
            Archery_Health hitHealth = hits.GetComponent<Archery_Health>();
            if (hitHealth!= null)
            {
                this.transform.parent = hits;

                shoot = false;
                if (hitHealth.m_type == Archery_Health.type.Boss)
                {
                    hitHealth.boss.RanomThrowPlace(player);
                    hitHealth.HealthChange(-damage );
                    hit[0].transform.position = raycastHit.point;
                    hit[0].SetActive(true);
                }
                else if (hitHealth.m_type == Archery_Health.type.Player)
                {
                    hitHealth.player.ByHit();
                    hitHealth.HealthChange(-damage);
                    hit[1].transform.position = raycastHit.point;
                    hit[1].SetActive(true);
                    Invoke("UnVisable", .5f);
                }
                onHit.Invoke();
            }
        }
    }

    void DestoryThis()
    {
        if(transform.parent == null)
        {
            Destroy(this.gameObject);
        }
    }

    void UnVisable()
    {
        this.gameObject.SetActive(false);
    }
}
