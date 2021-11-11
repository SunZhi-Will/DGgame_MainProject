using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archery_Boss : MonoBehaviour
{
    #region Var
    [Header("Move")]
    [SerializeField]
    Vector3 moveDirection;
    [SerializeField] [Range(1.5f, 3.5f)] float moveSpeed;

    bool isFliping = false;
    [SerializeField] Animator animator;

    [SerializeField] BoxCollider Set;
    float min, max = 0;

    float randomNextTime;
    float deltaTime;

    [Header("Spawn")]
    [SerializeField]
    Transform spawner;
    [SerializeField] BoxCollider[] playerSet;
    [SerializeField] Archery_Player[] player;
    float[] playerAxisZ = new float[2];
    [SerializeField] GameObject[] items;
    #endregion

    private void Start()
    {
        Bounds bounds = Set.bounds;
        min = bounds.min.x; max = bounds.max.x;
        Set.enabled = false;

        for (int i = 0; i < playerSet.Length; i++)
        {
            playerAxisZ[i] = playerSet[i].bounds.center.z;
        }

    }
    private void Update()
    {
        if (isFliping) { return; }
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime,Space.World);
        MoveLimint();
        ChangeSpeed();
    }
    void MoveLimint()
    {
        float x = transform.position.x;
        if (x <= min || x >= max)
        {
            moveDirection *= -1;
            StartCoroutine(SwitchDirection());
        }
    }

    void ChangeSpeed()
    {
        deltaTime += Time.deltaTime;

        if (deltaTime >= randomNextTime)
        {
            randomNextTime = Random.Range(1.5f, 2.5f);
            deltaTime = 0;

            moveSpeed = Random.Range(1.5f, 3.5f);
            if (Random.Range(0, 1.0f) <= .1f)
            {
                moveDirection *= -1;
                StartCoroutine(SwitchDirection());
            }
        }
    }

    IEnumerator SwitchDirection()
    {
        isFliping = true;
        float angle = 0;
        animator.SetTrigger("Jump");
        do
        {
            angle += Time.deltaTime * 100;
            transform.GetChild(0).RotateAround(transform.position, new Vector3(0, 1, 0), Time.deltaTime * 100);
            yield return null;
        } while (angle <= 180);
        transform.GetChild(0).RotateAround(transform.position, new Vector3(0, 1, 0), 180 - angle);

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime*10);
        isFliping = false;

        yield break;
    }

    public void RanomThrowPlace(Archery_Player _player)
    {
        animator.SetTrigger("Eat");
        float x, z = 0;

        x = Random.Range(min+2.5f, max-2.5f);

        float i = Random.Range(0, 1.0f);

        if (_player == player[0])
        {
            if (i <= 0.7f) { i = 0; } else { i = 1; }
        }
        else
        {
            if (i <= 0.7f) { i = 1; } else { i = 0; }
        }

        StartCoroutine(FaceToPlayer((int)i));
        z = playerAxisZ[(int)i];

        int j = Random.Range(0, items.Length);

        GameObject newItem = Instantiate(items[j], spawner.position, Quaternion.identity,playerSet[(int)i].transform);

        newItem.GetComponent<Archery_ItemThrowing>().target = new Vector3(x, playerSet[(int)i].bounds.max.y, z);

        newItem.GetComponent<Archery_ItemThrowing>().SetValue();
        newItem.GetComponent<Archery_ItemThrowing>().isThrow = true;

        newItem.transform.GetChild(0).GetComponent<Archery_Item>().player = player[(int)i];
    }

    IEnumerator FaceToPlayer(int player)
    {
         Vector3 preRot = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
         float angle = 0;
         if(player == 0)
         {
             do
             {
                 angle += 1000 * Time.deltaTime;
                 transform.RotateAround(transform.position, new Vector3(0, 1*moveDirection.x, 0), 1000 * Time.deltaTime);
                 yield return null;
             } while (angle <= 90);
             transform.RotateAround(transform.position, new Vector3(0, 1*moveDirection.x, 0), 90 -angle);
         }
         else
         {
             do
             {
                 angle += 1000 * Time.deltaTime;
                 transform.RotateAround(transform.position, new Vector3(0, -1*moveDirection.x, 0), 1000 * Time.deltaTime);
                 yield return null;
             } while (angle <= 90);
             transform.RotateAround(transform.position, new Vector3(0,-1*moveDirection.x, 0), 90 -angle);
         }

        yield return new WaitForSeconds(0.5f);
        angle = 0;
        if (player == 0)
        {
            do
            {
                angle += 850 * Time.deltaTime;
                transform.RotateAround(transform.position, new Vector3(0, -1 * moveDirection.x, 0), 850 * Time.deltaTime);
                yield return null;
            } while (angle <= 90);
            transform.rotation = Quaternion.identity;
        }
        else
        {
            do
            {
                angle += 850 * Time.deltaTime;
                transform.RotateAround(transform.position, new Vector3(0, 1 * moveDirection.x, 0), 850 * Time.deltaTime);
                yield return null;
            } while (angle <= 90);
            transform.rotation = Quaternion.identity;
        }
    }
}
