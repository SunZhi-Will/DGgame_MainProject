using UnityEngine;

namespace Frank
{
    public class RootMotionController : MonoBehaviour
    {
        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            animator.SendMessageUpwards("OnUpdateRootMotion", animator.deltaPosition);
        }
    }
}
