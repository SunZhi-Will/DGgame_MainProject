using UnityEngine;
using UnityEngine.UI;

namespace Frank
{
    public class HPController : MonoBehaviour
    {
        [SerializeField] GameObject Hp_Canvas;

        public Image[] maxHP;
        [HideInInspector] public int currentHP;

        private Animator animator;
        private Rigidbody rootRigidbody;
        private Collider rootCollider;
        private KeyboardInput keyboardInput;
        private JoystickInput joystickInput;
        private AttackController attackController;
        private StateEffectController stateEffectController;

        private Rigidbody[] rigidbodies;
        private Collider[] colliders;

        [HideInInspector] public bool isInCircle;

        float timer;

        // Start is called before the first frame update
        void Start()
        {
            isInCircle = true;

            currentHP = maxHP.Length;

            animator = GetComponentInChildren<Animator>();
            rootRigidbody = GetComponent<Rigidbody>();
            rootCollider = GetComponent<Collider>();
            keyboardInput = GetComponent<KeyboardInput>();
            joystickInput = GetComponent<JoystickInput>();
            stateEffectController = GetComponentInParent<StateEffectController>();

            rigidbodies = GetComponentsInChildren<Rigidbody>();
            colliders = GetComponentsInChildren<Collider>();

            SetRagdoll(false);
            keyboardInput.inputEnabled = true;
            joystickInput.inputEnabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < maxHP.Length; i++)
            {
                if (i < currentHP)
                    maxHP[i].enabled = true;
                else
                    maxHP[i].enabled = false;
            }

            if (!isInCircle && !ScoreBoardScript.g_Player1IsDied && !ScoreBoardScript.g_Player2IsDied)
            {
                if (Time.time > timer)
                {
                    GetHurt(gameObject);
                    timer = Time.time + 2;
                }
            }
        }

        private void GetHurt(GameObject player)
        {
            if (!stateEffectController.isInvincible)
                currentHP--;

            if (currentHP <= 0)
                currentHP = 0;

            if (currentHP == 0)
            {
                SetRagdoll(true);
                Hp_Canvas.SetActive(false);
                keyboardInput.inputEnabled = false;
                joystickInput.inputEnabled = false;
            }
        }

        private void SetRagdoll(bool enabled)
        {
            animator.enabled = !enabled;
            rootRigidbody.isKinematic = enabled;
            rootCollider.isTrigger = enabled;

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                if (rigidbodies[i] == rootRigidbody) continue;
                rigidbodies[i].isKinematic = !enabled;
            }

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] == rootCollider) continue;
                colliders[i].isTrigger = !enabled;
            }
        }
    }
}