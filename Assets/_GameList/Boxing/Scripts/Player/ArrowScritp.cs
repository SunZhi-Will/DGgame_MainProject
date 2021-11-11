using UnityEngine;

namespace Frank
{
    public class ArrowScritp : MonoBehaviour
    {
        public Transform left, right;

        //const float moveSpeed = 50;
        const float moveSpeed = 150;

        Transform target = null;

        void Start()
        {
            target = left;
        }
        void Update()
        {
            if (transform.parent.localRotation == left.localRotation)
                target = right;
            else if (transform.parent.localRotation == right.localRotation)
                target = left;

            transform.parent.localRotation = Quaternion.RotateTowards(transform.parent.localRotation, target.localRotation, moveSpeed * Time.deltaTime);
        }
    }
}