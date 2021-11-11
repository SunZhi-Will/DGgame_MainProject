using UnityEngine;

namespace Frank
{
    public class PropsScritp : MonoBehaviour
    {
        [SerializeField] GameObject pickupEffectPrefab;

        public float eulerAngle = 1;

        private Rigidbody rigid;

        private void Start()
        {
            rigid = GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.FreezePosition;

            Destroy(gameObject, 10);
        }

        // Update is called once per frame
        void Update()
        {
            transform.eulerAngles += new Vector3(0, eulerAngle, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player 1" || other.gameObject.tag == "Player 2")
            {
                GameObject pickupEffect = Instantiate(pickupEffectPrefab, other.transform.position, other.transform.rotation);
                Destroy(pickupEffect, 1);
                Destroy(gameObject);
            }
        }
    }
}
