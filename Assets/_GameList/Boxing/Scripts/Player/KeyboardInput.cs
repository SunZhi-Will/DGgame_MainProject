using UnityEngine;

namespace Frank
{
    public class KeyboardInput : IUserInput
    {
        public PlayerControl_ScriptObject playerControl_scrObj;

        public string keyUp = "w";
        public string keyDown = "s";
        public string keyRight = "d";
        public string keyLeft = "a";
        public string keyAttack = "space";

        private StateEffectController stateEffectController;

        private void Start()
        {
            stateEffectController = GetComponent<StateEffectController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Timer.g_Timer == 0)
            {
                //targetDirectorUp = (Input.GetKey(keyUp) ? 1 : 0) - (Input.GetKey(keyDown) ? 1 : 0);
                //targetDirectorRight = (Input.GetKey(keyRight) ? 1 : 0) - (Input.GetKey(keyLeft) ? 1 : 0);
                targetDirectorUp = (PlayerControl_koroshi.Key(playerControl_scrObj.keyUp) ? 1 : 0) - (PlayerControl_koroshi.Key(playerControl_scrObj.keyDown) ? 1 : 0);
                targetDirectorRight = (PlayerControl_koroshi.Key(playerControl_scrObj.keyRight) ? 1 : 0) - (PlayerControl_koroshi.Key(playerControl_scrObj.keyLeft) ? 1 : 0);
               
                if (stateEffectController.isConfusion)
                {
                    //targetDirectorUp = (Input.GetKey(keyDown) ? 1 : 0) - (Input.GetKey(keyUp) ? 1 : 0);
                    //targetDirectorRight = (Input.GetKey(keyLeft) ? 1 : 0) - (Input.GetKey(keyRight) ? 1 : 0);
                    targetDirectorUp = (PlayerControl_koroshi.Key(playerControl_scrObj.keyDown) ? 1 : 0) - (PlayerControl_koroshi.Key(playerControl_scrObj.keyUp) ? 1 : 0);
                    targetDirectorRight = (PlayerControl_koroshi.Key(playerControl_scrObj.keyLeft) ? 1 : 0) - (PlayerControl_koroshi.Key(playerControl_scrObj.keyRight) ? 1 : 0);
                }

                if (!inputEnabled)
                {
                    targetDirectorUp = 0;
                    targetDirectorRight = 0;
                }

                directorUp = Mathf.SmoothDamp(directorUp, targetDirectorUp, ref velocityDirectorUp, .1f);
                directorRight = Mathf.SmoothDamp(directorRight, targetDirectorRight, ref velocityDirectorRight, .1f);

                Vector2 tempDirectorAxis = SquareToCircle(new Vector2(directorRight, directorUp));

                directorMagnitude = Mathf.Sqrt(Mathf.Pow(tempDirectorAxis.y, 2) + Mathf.Pow(tempDirectorAxis.x, 2));
                directorVector = tempDirectorAxis.x * transform.right + tempDirectorAxis.y * transform.forward;

                //bool newAttack = Input.GetKey(keyAttack);
                bool newAttack = PlayerControl_koroshi.Key(playerControl_scrObj.keyConfirm);

                if (newAttack != lastAttack && newAttack == true)
                    isAttack = true;
                else
                    isAttack = false;
                lastAttack = newAttack;
            }
        }
    }
}