using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Frank
{
    public class PropsSpawn : MonoBehaviour
    {
        public GameObject[] propsPrefabs;

        List<GameObject> propsClones = new List<GameObject>();

        public int maxCount = 1;

        public float radius = 3;

        float timer;

        void Update()
        {
            if (propsClones.Count < maxCount)
            {
                if (Time.time > timer)
                {
                    Vector3 v = transform.forward * Random.Range(0, radius);
                    v = Quaternion.Euler(0, Random.Range(0, 360), 0) * v;
                    Vector3 spawnPoint = v + transform.position;

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(spawnPoint, out hit, radius, NavMesh.AllAreas))
                    {
                        GameObject clone = Instantiate(propsPrefabs[Random.Range(0, propsPrefabs.Length)], spawnPoint, Quaternion.Euler(0, Random.Range(0, 360), 0));
                        propsClones.Add(clone);
                        timer = Time.time + 5;
                    }
                }
            }
            else
            {
                for (int i = 0; i < propsClones.Count; i++)
                {
                    if (propsClones[i] == null)
                    {
                        propsClones.RemoveAt(i);
                        timer = Time.time + 3;
                        break;
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
