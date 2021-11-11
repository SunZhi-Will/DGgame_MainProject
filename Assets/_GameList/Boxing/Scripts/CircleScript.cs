using UnityEngine;

namespace Frank
{
    public class CircleScript : MonoBehaviour
    {
        public Vector3 speed = new Vector3(0, 0, 100);

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(speed * Time.deltaTime);
        }
    }
}
