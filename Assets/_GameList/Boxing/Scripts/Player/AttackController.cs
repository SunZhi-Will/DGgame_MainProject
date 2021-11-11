using System.Collections.Generic;
using UnityEngine;

namespace Frank
{
    public class AttackController : MonoBehaviour
    {
        public float attackRadius = 1;
        public float attackAngle = 100;
        [Space]
        [SerializeField] HPController hPController;
        [Space]
        [SerializeField] GameObject attackEffectPrefab;
        [SerializeField] Transform attackEffectTransform;
        [SerializeField] Canvas canvas;
        [SerializeField] Shader shader;
        [Space]
        [SerializeField] AudioSource attackAudioSource;

        private ShakeCamera shakeCamera;
        private GameObject obj;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        private void Start()
        {
            shakeCamera = Camera.main.GetComponent<ShakeCamera>();
        }

        private void Update()
        {
            DrawSectorSolid(transform.position, attackRadius, attackAngle);
        }

        private void Attack()
        {
            AttackRange("Player 1", LayerMask.GetMask("Player 2"));
            AttackRange("Player 2", LayerMask.GetMask("Player 1"));
        }

        private void AttackRange(string _name, LayerMask _layer2)
        {
            if (gameObject.layer == LayerMask.NameToLayer(_name))
            {
                Collider[] colliders1 = Physics.OverlapSphere(transform.position, attackRadius, _layer2);
                for (int i = 0; i < colliders1.Length; i++)
                {
                    //if (Vector3.Angle(transform.forward, colliders1[i].transform.position - transform.position) < attackAngle / 2)
                    if (Vector3.Angle(transform.forward, colliders1[i].transform.position - transform.position) < attackAngle / 1.2f)
                    {
                        if (hPController.currentHP > 0)
                        {
                            colliders1[i].SendMessageUpwards("GetHurt", gameObject);

                            attackAudioSource.Play();

                            GameObject clone = Instantiate(attackEffectPrefab, attackEffectTransform.position, Quaternion.identity);
                            Destroy(clone, 2);

                            shakeCamera.enabled = true;
                        }
                    }
                }
            }
        }

        private GameObject CreateMesh(List<Vector3> _vertices)
        {
            int[] triangles;
            Mesh mesh = new Mesh();

            int triangleAmount = _vertices.Count - 2;
            triangles = new int[3 * triangleAmount];

            for (int i = 0; i < triangleAmount; i++)
            {
                triangles[3 * i] = 0;
                triangles[3 * i + 1] = i + 1;
                triangles[3 * i + 2] = i + 2;
            }

            if (obj == null)
            {
                obj = new GameObject("AttackRangeMesh");
                obj.transform.position = Vector3.up * .1f;
                meshFilter = obj.AddComponent<MeshFilter>();
                meshRenderer = obj.AddComponent<MeshRenderer>();
            }

            mesh.vertices = _vertices.ToArray();
            mesh.triangles = triangles;

            meshFilter.mesh = mesh;
            meshRenderer.material.shader = shader;

            if (gameObject.layer == LayerMask.NameToLayer("Player 1"))
                meshRenderer.material.color = new Color(.03355288f, .216231f, .5471698f, 1);
            else
                meshRenderer.material.color = new Color(.5566038f, .08139017f, .08139017f, 1);

            return obj;
        }

        private void DrawSectorSolid(Vector3 _center, float _radius, float _angle)
        {
            int pointAmount = 500;
            float eachAngle = _angle / pointAmount;

            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(_center);

            for (int i = 0; i < pointAmount; i++)
            {
                Vector3 pos = Quaternion.Euler(0, -_angle / 2 + eachAngle * i, 0) * transform.forward * _radius + _center;
                vertices.Add(pos);
            }

            CreateMesh(vertices);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            DrawArc();
            DrawAngle();
        }

        private void DrawArc()
        {
            int segment = 6;
            for (int i = 0; i < segment; i++)
            {
                Vector3 p1 = Quaternion.Euler(0, i * attackAngle / 2 / segment, 0) * transform.forward * attackRadius + Vector3.up * .01f + transform.position;
                Vector3 p2 = Quaternion.Euler(0, (i + 1) * attackAngle / 2 / segment, 0) * transform.forward * attackRadius + Vector3.up * .01f + transform.position;
                Gizmos.DrawLine(p1, p2);

                Vector3 p3 = p1 + Vector3.up * .1f;
                Vector3 p4 = p2 + Vector3.up * .1f;
                Gizmos.DrawLine(p3, p4);

                p1 = Quaternion.Euler(0, i * -attackAngle / 2 / segment, 0) * transform.forward * attackRadius + Vector3.up * .01f + transform.position;
                p2 = Quaternion.Euler(0, (i + 1) * -attackAngle / 2 / segment, 0) * transform.forward * attackRadius + Vector3.up * .01f + transform.position;
                Gizmos.DrawLine(p1, p2);

                p3 = p1 + Vector3.up * .1f;
                p4 = p2 + Vector3.up * .1f;
                Gizmos.DrawLine(p3, p4);
            }
        }

        private void DrawAngle()
        {
            Vector3 p1 = Quaternion.Euler(0, attackAngle / 2, 0) * transform.forward * attackRadius + Vector3.up * .01f + transform.position;
            Vector3 p2 = p1 + Vector3.up * .1f;
            Gizmos.DrawLine(transform.position + Vector3.up * .01f, p1);
            Gizmos.DrawLine(transform.position + Vector3.up * (.1f + .01f), p2);

            p1 = Quaternion.Euler(0, -attackAngle / 2, 0) * transform.forward * attackRadius + Vector3.up * .01f + transform.position;
            p2 = p1 + Vector3.up * .1f;
            Gizmos.DrawLine(transform.position + Vector3.up * .01f, p1);
            Gizmos.DrawLine(transform.position + Vector3.up * (.1f + .01f), p2);
        }
    }
}