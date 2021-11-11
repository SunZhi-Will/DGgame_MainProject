using UnityEngine;
using AnimAction;

namespace Frank
{
    public class ActorController : MonoBehaviour
    {
        private ArrowScritp arrowScritp_scr;

        public enum InputType { Keyboard, Joystick }

        [SerializeField] InputType inputType;
        [SerializeField] GameObject player, triangle;
        [HideInInspector] public float movingSpeed;

        private IUserInput playerInput;
        private KeyboardInput keyboardInput;
        private JoystickInput joystickInput;
        private Animator animator;
        private Rigidbody rigid;
        private StateEffectController stateEffectController;
        private Vector3 movingVector;
        private bool lockMoving = false;
        private Vector3 deltPos;

        private void Awake()
        {
            keyboardInput = GetComponent<KeyboardInput>();
            joystickInput = GetComponent<JoystickInput>();
            SetInputType();

            arrowScritp_scr = GetComponentInChildren<ArrowScritp>();
        }

        // Start is called before the first frame update
        void Start()
        {
            animator = player.GetComponent<Animator>();
            rigid = GetComponent<Rigidbody>();
            stateEffectController = GetComponent<StateEffectController>();

            movingSpeed = stateEffectController.defaultMoveSpeed;

            rigid.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void FixedUpdate()
        {
            if (lockMoving == false)
            {
                rigid.velocity = new Vector3(movingVector.x, rigid.velocity.y, movingVector.z);
            }
            rigid.position += deltPos;
            deltPos = Vector3.zero;
        }

        // Update is called once per frame
        public float AnimAccelerate = 2;//加速動畫
        void Update()
        {
            if (playerInput.isAttack)
            {
                animator.SetTrigger("Attack");
            }
            //偵測是否為攻擊動畫中
            //Debug.Log(gameObject.name+"\n"+AnimScript.AnimStateJudgment(animator, "Attack"));
            if (AnimScript.AnimStateJudgment(animator, "Attack"))
            {
                animator.speed = AnimAccelerate;
                //if (arrowScritp_scr.enabled == true)
                //    arrowScritp_scr.enabled = false;
            }
            else
            {
                animator.speed = 1;
                //if (arrowScritp_scr.enabled == false)
                //    arrowScritp_scr.enabled = true;
            }
            //animator.speed = AnimScript.AnimStateJudgment(animator, "Attack") ? AnimAccelerate : 1;//修改動畫速度

            if (lockMoving == false)
            {
                animator.SetFloat("Forward", playerInput.directorMagnitude);
                if (playerInput.directorMagnitude > .1f)
                    player.transform.forward = Vector3.Slerp(player.transform.forward, playerInput.directorVector, .3f);

                movingVector = playerInput.directorMagnitude * player.transform.forward * movingSpeed;
            }

            if (ScoreBoardScript.g_Player1IsDied || ScoreBoardScript.g_Player2IsDied || ScoreBoardScript.g_EndOfRound)
            {
                lockMoving = true;
                joystickInput.enabled = false;
                keyboardInput.enabled = false;

                animator.SetFloat("Forward", 0);

                Debug.Log("回合結束");
            }

            animator.SetBool("Dizzy", stateEffectController.isDizzy);
        }

        private void SetInputType()
        {
            switch (inputType)
            {
                case InputType.Keyboard:
                    keyboardInput.enabled = true;
                    joystickInput.enabled = false;
                    break;
                case InputType.Joystick:
                    joystickInput.enabled = true;
                    keyboardInput.enabled = false;
                    break;
                default:
                    break;
            }

            IUserInput[] playerInputs = GetComponents<IUserInput>();
            foreach (var playerInput in playerInputs)
            {
                if (playerInput.enabled == true)
                    this.playerInput = playerInput;
            }
        }

        /// <summary>
        /// Finite State Machine
        /// </summary>
        private bool CheckState(string stateName, string layerName = "Base Layer")
        {
            return animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex(layerName)).IsName(stateName);
        }

        public void OnAttackEnter()
        {
            player.transform.LookAt(triangle.transform);
            playerInput.inputEnabled = false;
            lockMoving = true;
        }

        public void OnAttackExit()
        {
            playerInput.inputEnabled = true;
            lockMoving = false;
        }

        public void OnDizzyEnter()
        {
            playerInput.inputEnabled = false;
            lockMoving = true;
        }

        public void OnDizzyUpdate()
        {
            playerInput.inputEnabled = false;
            lockMoving = true;
        }

        public void OnDizzyExit()
        {
            playerInput.inputEnabled = true;
            lockMoving = false;
        }

        public void OnUpdateRootMotion(object _deltaPos)
        {
            if (CheckState("Attack"))
            {
                deltPos += (Vector3)_deltaPos;
            }
        }
    }
}