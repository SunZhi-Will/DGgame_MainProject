using UnityEngine;

namespace Frank
{
    public class LookAtCamera : MonoBehaviour
    {

        private Transform Cam_tra;

        private void Start()
        {
            Cam_tra = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            Quaternion rot = Quaternion.LookRotation(Cam_tra.position - transform.position);
            transform.rotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, 0);
        }
    }
}