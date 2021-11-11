using UnityEngine;

namespace Frank
{
    public class TowardPlayer : MonoBehaviour
    {
        HPController player1, player2;

        float moveSpeed = 10;
        [HideInInspector] public float distance;

        //bool player1IsDied, player2IsDied, everyoneIsDied;

        private void Start()
        {
            if (player1 == null)
                player1 = GameObject.FindGameObjectWithTag("Player 1").GetComponent<HPController>();

            if (player2 == null)
                player2 = GameObject.FindGameObjectWithTag("Player 2").GetComponent<HPController>();
        }

        void Update()
        {
            if (ScoreBoardScript.g_Player1IsDied && !ScoreBoardScript.g_Player2IsDied)
                CameraEffect(player1);
            else if (ScoreBoardScript.g_Player2IsDied && !ScoreBoardScript.g_Player1IsDied)
                CameraEffect(player2);
        }

        void CameraEffect(HPController player)
        {
            if (player.currentHP == 0)
            {
                distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance > 3)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveSpeed);
                    moveSpeed = Mathf.Lerp(moveSpeed, 0, Time.deltaTime * .7f);
                }
            }
        }
    }
}
