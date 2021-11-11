using UnityEngine;

namespace Frank
{
    public abstract class IUserInput : MonoBehaviour
    {
        public float directorUp;
        public float directorRight;
        public float directorMagnitude;
        public Vector3 directorVector;

        [HideInInspector] public bool isAttack;
        protected bool lastAttack;

        [HideInInspector] public bool inputEnabled = true;

        protected float targetDirectorUp;
        protected float targetDirectorRight;
        protected float velocityDirectorUp;
        protected float velocityDirectorRight;

        protected Vector2 SquareToCircle(Vector2 input)
        {
            Vector2 output = Vector2.zero;

            output.x = input.x * Mathf.Sqrt(1 - (Mathf.Pow(input.y, 2) / 2));
            output.y = input.y * Mathf.Sqrt(1 - (Mathf.Pow(input.x, 2) / 2));

            return output;
        }
    }
}