using UnityEngine;
using UnityEngine.UI;

namespace Frank
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] Text timerTxt;

        public static int g_Timer = 3;

        public static bool g_IsFight = false;

        void Update()
        {
            if (GameManager.g_IsStart)
            {
                g_Timer = 3;
                InvokeRepeating("CountTime", 1, 1);
                GameManager.g_IsStart = false;
            }
        }

        private void CountTime()
        {
            g_Timer--;
            timerTxt.text = g_Timer.ToString();

            if (g_Timer <= 0)
                g_Timer = 0;

            if (g_Timer == 0)
            {
                timerTxt.enabled = false;
                CancelInvoke();
                g_IsFight = true;
            }
        }
    }
}