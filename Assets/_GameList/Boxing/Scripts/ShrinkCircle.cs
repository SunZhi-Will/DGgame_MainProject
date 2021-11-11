using System.Collections;
using UnityEngine;

namespace Frank
{
    public class ShrinkCircle : MonoBehaviour
    {
        public GameObject circlePrefab;
        GameObject circle;

        public float side = 1;
        float radius, x, z;

        public float keepZoneTime = 3;
        public float[] zoneTimes;
        float remainTime;

        int stage;
        bool canContinue = true;

        PropsSpawn propsSpawn;
        SphereCollider sphereCollider;

        // Start is called before the first frame update
        void Start()
        {
            propsSpawn = FindObjectOfType<PropsSpawn>();

            zoneTimes[stage] += Time.time;
            remainTime = zoneTimes[stage];

            stage = 0;

            circle = Instantiate(circlePrefab, Vector3.zero, circlePrefab.transform.rotation);
            circle.name = circlePrefab.name;
            circle.transform.localScale = new Vector3(side, side, transform.localScale.z);

            sphereCollider = GameObject.Find("ShrinkCircle").GetComponent<SphereCollider>();

            radius = side / 2;
            x = Random.Range(-radius, radius);
            z = Random.Range(-radius, radius);
        }

        // Update is called once per frame
        void Update()
        {
            if (Timer.g_Timer == 0)
            {
                if (canContinue)
                {
                    remainTime = zoneTimes[stage] - Time.time;

                    if (remainTime < 0)
                        remainTime = 0;

                    if (remainTime == 0)
                    {
                        canContinue = false;
                        stage++;

                        SetCircle();
                    }
                }
            }
        }

        private void SetCircle()
        {
            circle.transform.position = Vector3.Slerp(circle.transform.position, new Vector3(x, 0, z), Time.deltaTime * 10);
            circle.transform.localScale = Vector3.Slerp(circle.transform.localScale, new Vector3(radius, radius, transform.localScale.z), Time.deltaTime * 10);

            propsSpawn.transform.position = Vector3.Slerp(propsSpawn.transform.position, new Vector3(x, 0, z), Time.deltaTime * 10);
            propsSpawn.radius = circle.transform.localScale.x;

            sphereCollider.center = circle.transform.position;
            sphereCollider.radius = propsSpawn.radius;

            radius /= 2;
            x = Random.Range(x - radius, x + radius);
            z = Random.Range(z - radius, z + radius);

            if (stage < zoneTimes.Length)
                StartCoroutine(NextZoneTime());
        }

        IEnumerator NextZoneTime()
        {
            yield return new WaitForSeconds(keepZoneTime);
            StartNextCircle();
        }

        private void StartNextCircle()
        {
            canContinue = true;

            zoneTimes[stage] += Time.time;
            remainTime = zoneTimes[stage];
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player 1" || other.gameObject.tag == "Player 2")
                other.GetComponent<HPController>().isInCircle = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player 1" || other.gameObject.tag == "Player 2")
                other.GetComponent<HPController>().isInCircle = false;
        }
    }
}
