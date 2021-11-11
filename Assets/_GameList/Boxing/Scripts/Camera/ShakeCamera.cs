using UnityEngine;

namespace Frank
{
    public class ShakeCamera : MonoBehaviour
    {
        Camera cam;

        [Range(1,5)]
        public float level = 3;
        public float timer = .5f;

        float currentLevel;
        float currentFrame;
        float currentTimer;
        bool isEnabled;

        void OnEnable()
        {
            isEnabled = true;
            currentLevel = level;
            currentTimer = timer;
        }

        private void Start()
        {
            cam = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isEnabled)
            {
                if (currentTimer > 0)
                {
                    currentTimer -= Time.deltaTime;
                    cam.rect = new Rect((currentLevel * Random.value) * .001f, (currentLevel * Random.value) * .001f, 1, 1);
                }
                else
                    enabled = false;
            }
        }

        void OnDisable()
        {
            isEnabled = false;
        }
    }
}