using UnityEngine;

namespace Frank
{
    public class JoystickInput : IUserInput
    {
        public string xAxis = "LeftStickX";
        public string yAxis = "LeftStickY";
        public string button2 = "Circle";

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
                targetDirectorUp = Input.GetAxis(yAxis);
                targetDirectorRight = Input.GetAxis(xAxis);

                if (stateEffectController.isConfusion)
                {
                    targetDirectorUp = -1 * Input.GetAxis(yAxis);
                    targetDirectorRight = -1 * Input.GetAxis(xAxis);
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

                bool newAttack = Input.GetButton(button2);
                if (newAttack != lastAttack && newAttack == true)
                    isAttack = true;
                else
                    isAttack = false;
                lastAttack = newAttack;
            }
        }
    }
}