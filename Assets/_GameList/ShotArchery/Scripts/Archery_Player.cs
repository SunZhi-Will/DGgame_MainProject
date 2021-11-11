using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Archery_Player : MonoBehaviour
{
    #region Var
    [SerializeField] KeyCode rightKey;
    [SerializeField] KeyCode leftKey;
    [SerializeField] KeyCode shootKey;

    [Range(0,1)]public float moveSpeed;
    [SerializeField] AudioSource step;
    public Animator animator;

    [SerializeField] BoxCollider PlayerSet;
    public Archery_Health health;
    Vector3 previousPos;
    float min, max = 0;

    [SerializeField] bool isShoot = false;
    [SerializeField] Transform arrowSpawn;
    [SerializeField] RotationConstraint _bowRotation;
    [SerializeField] Transform[] arrow;
     public int strongArrow = 0;
    float arrowSpawnTime;
    public float arrowDeltaTime = 2.5f;
    float spawnReduceTime;

    [HideInInspector]public bool isSlide,isStaright = false;
    [SerializeField] ParticleSystem _straight;
    public GameObject[] _health;
    public ParticleSystem arrowStr;
    #endregion
    private void Start()
    {
        Bounds bounds = PlayerSet.bounds;
        min = bounds.min.x; max = bounds.max.x;
        arrowSpawnTime = arrowDeltaTime;
    }
    private void Update()
    {
        if (isStaright) { return; }
        ShootDeltaTimeReduce();
        PlayerMovement();
    }
    void PlayerMovement()
    {
        previousPos = transform.position;
        if (!isShoot)
        {
            if (isSlide)
            {
                if (Input.GetKey(rightKey))
                {
                    if (!step.isPlaying) { step.Play(); }
                    transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime * 10, Space.World);
                    animator.SetBool("Walk", true);
                }
                else if (Input.GetKey(leftKey))
                {
                    if (!step.isPlaying) { step.Play(); }
                    transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime * 10, Space.World);
                    animator.SetBool("Walk", true);
                }
                else if (Input.GetKeyUp(leftKey) || Input.GetKeyUp(rightKey))
                {
                    step.Stop();
                    animator.SetBool("Walk", false);
                }
            }
            else
            {
                if (Input.GetKey(rightKey))
                {
                    if (!step.isPlaying) { step.Play(); }
                    transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime * 10, Space.World);
                    animator.SetBool("Walk", true);
                    if ((transform.GetChild(0).eulerAngles.y) % 360 != 90)
                    {
                        transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 90 - transform.rotation.y, 0));
                    }
                }
                else if (Input.GetKey(leftKey))
                {
                    if (!step.isPlaying) { step.Play(); }
                    transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime * 10, Space.World);
                    animator.SetBool("Walk", true);
                    if ((transform.GetChild(0).eulerAngles.y) % 360 != 270)
                    {
                        transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 270 - transform.rotation.y, 0));
                    }
                }
                else if (Input.GetKeyUp(leftKey) || Input.GetKeyUp(rightKey))
                {
                    step.Stop();
                    animator.SetBool("Walk", false);
                    if ((transform.GetChild(0).eulerAngles.y) % 360 != 0)
                    {
                        transform.GetChild(0).eulerAngles = transform.eulerAngles;
                    }
                }
            }
        }

        MoveLimint();
        HitByMove();

        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
    }
    void MoveLimint()
    {
        float x = transform.position.x;
        x = Mathf.Clamp(x,min,max);
        transform.position = new Vector3(x,transform.position.y,transform.position.z);
    }

    public Transform ShotObject()
    {
        Transform newArrow = Instantiate(arrow[1].gameObject, arrowSpawn.position, transform.rotation).transform;
        newArrow.GetComponent<Archery_Arrow>().shoot = true;
        newArrow.GetComponent<Archery_Arrow>().player = this;
        return newArrow;
    }
    void Shoot()
    {
        step.Stop();
        if (arrowSpawnTime <= arrowDeltaTime) { return; }
        if (isSlide) { return; }
        isShoot = true;
        animator.SetTrigger("Shoot");

        StartCoroutine(SpawnArrow());
        arrowSpawnTime = 0;
    }
    IEnumerator SpawnArrow()
    {
        _bowRotation.weight = 0;
        do
        {
            yield return null;
        } while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.3f);

        GameObject newArrow = null;
        if (strongArrow > 0)
        {
            newArrow = Instantiate(arrow[1].gameObject, arrowSpawn.position, transform.rotation);
            strongArrow -= 1;
            if (strongArrow <= 0) { arrowStr.Stop(); }
        }
        else
        {
            newArrow = Instantiate(arrow[0].gameObject, arrowSpawn.position, transform.rotation);
        }

        newArrow.GetComponent<Archery_Arrow>().shoot = true;
        newArrow.GetComponent<Archery_Arrow>().player = this;

        _bowRotation.weight = 1;
        isShoot = false;
        yield break;
    }
    void ShootDeltaTimeReduce()
    {
        arrowSpawnTime += Time.deltaTime;
        spawnReduceTime += Time.deltaTime;

        if(spawnReduceTime >= 2.5f)
        {
            float x = arrowDeltaTime - 0.05f;
            arrowDeltaTime = Mathf.Clamp(x, 0.2f, 1);
            spawnReduceTime = 0;
        }
    }
    void HitByMove()
    {
        float length = (transform.position - previousPos).magnitude;
        Debug.DrawLine(transform.position - new Vector3(0, .1f, 0), previousPos - new Vector3(0, .1f, 0), Color.red) ;
        Vector3 direction = (transform.position - previousPos).normalized;
        RaycastHit raycastHit;
        if (Physics.Raycast(previousPos-new Vector3(0,.3f,0), direction, out raycastHit, length))
        {
            Archery_Item _item= raycastHit.collider.transform.GetComponent<Archery_Item>();
            if(_item!= null)
            {
                _item.onHit.Invoke();
            }
        }
    }
    public void ByHit()
    {
        isStaright = true;
        Invoke("StraightEnd",1);
        _straight.Play();
        animator.SetBool("Straight", true);
    }
    void StraightEnd()
    {
        isStaright = false;
        animator.SetBool("Straight",false);
        _straight.Stop();
    }
}