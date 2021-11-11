using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Frank
{
    public class ScoreBoardScript : MonoBehaviour
    {
        private string Timed_tailString = "s";//顯示秒數後墜
        [SerializeField] Text timerTxt, endOfRoundTxt, koTxt;
        [SerializeField] Text player1ScoreTxt, player2ScoreTxt;
        [SerializeField] Animator endOfRoundTxtAnimator, koTxtAnimator;
        [SerializeField] AudioSource koAudioSource;

        [SerializeField]//方便修改測試用
        float timer, round;
        bool canPlayKoAudio;
        public static bool g_AddPlayer1Score, g_AddPlayer2Score, g_Player1IsDied, g_Player2IsDied, g_EndOfRound;

        [SerializeField] GameObject m_scoreCanvas;

        GameObject player1, player2;
        HPController player1HP, player2HP;
        HPController[] hPControllers;
        TowardPlayer towardsPlayer;

        private void Start()
        {
            player1 = GameObject.FindGameObjectWithTag("Player 1");
            player2 = GameObject.FindGameObjectWithTag("Player 2");
            player1HP = player1.GetComponent<HPController>();
            player2HP = player2.GetComponent<HPController>();

            hPControllers = FindObjectsOfType<HPController>();
            towardsPlayer = Camera.main.GetComponent<TowardPlayer>();

            g_Player1IsDied = false;
            g_Player2IsDied = false;
            g_AddPlayer1Score = false;
            g_AddPlayer2Score = false;
            g_EndOfRound = false;

            canPlayKoAudio = true;

            endOfRoundTxt.enabled = false;
        }

        private void Update()
        {
            timerTxt.text = timer + Timed_tailString;

            //player1ScoreTxt.text = "0" + GameManager.g_Player1Score;
            //player2ScoreTxt.text = "0" + GameManager.g_Player2Score;

            player1ScoreTxt.text = GameManager.g_Player1Score.ToString();
            player2ScoreTxt.text = GameManager.g_Player2Score.ToString();

            if (Timer.g_IsFight)
            {
                timer = GameManager.g_Timer;
                InvokeRepeating("CountTime", Timer.g_Timer, 1);
                timerTxt.enabled = true;
                Timer.g_IsFight = false;
            }

            for (int i = 0; i < hPControllers.Length; i++)
            {
                if (hPControllers[i].currentHP == 0 && timer != 0)
                {
                    CancelInvoke();

                    if (hPControllers[i].gameObject.layer == LayerMask.NameToLayer("Player 1"))
                    {
                        if (!g_Player2IsDied)
                        {
                            g_Player1IsDied = true;
                            g_AddPlayer2Score = true;
                        }
                    }
                    else if (hPControllers[i].gameObject.layer == LayerMask.NameToLayer("Player 2"))
                    {
                        if (!g_Player1IsDied)
                        {
                            g_Player2IsDied = true;
                            g_AddPlayer1Score = true;
                        }
                    }

                    if (towardsPlayer.distance <= 3)
                    {
                        StartCoroutine(IE_TextEffect(koTxt, koTxtAnimator));

                        if ((g_Player1IsDied || g_Player2IsDied) && canPlayKoAudio)
                        {
                            StartCoroutine(IE_Audio(koAudioSource));
                            canPlayKoAudio = false;
                        }
                    }
                }
            }
        }

        private void CountTime()
        {
            if (timer > 0)
            {
                timer--;

                timerTxt.text = timer + Timed_tailString;
                //if (timer < 10)
                //    timerTxt.text = "0" + timer + Timed_tailString;
            }
            else
            {
                CancelInvoke();
                timer = 0;
                timerTxt.text = timer + Timed_tailString;
                //timerTxt.text = "0" + timer + Timed_tailString;

                StartCoroutine(IE_TextEffect(endOfRoundTxt, endOfRoundTxtAnimator));

                if (player1HP.currentHP > player2HP.currentHP)
                    g_AddPlayer1Score = true;
                else if (player1HP.currentHP < player2HP.currentHP)
                    g_AddPlayer2Score = true;
            }
        }

        IEnumerator IE_TextEffect(Text _text, Animator _animator)
        {
            g_EndOfRound = true;
            yield return new WaitForSecondsRealtime(1);
            _text.enabled = true;
            _animator.SetTrigger("IsTrigger");
            yield return new WaitForSecondsRealtime(3);
            if (!m_scoreCanvas.activeSelf)
            {
                switch (GameManager.g_CurrentRound)
                {
                    case 0:
                        GameManager.g_CurrentRound = 1;
                        break;
                    case 1:
                        GameManager.g_CurrentRound = 2;
                        break;
                    case 2:
                        GameManager.g_CurrentRound = 3;
                        break;
                    case 3:
                        GameManager.g_CurrentRound = 4;
                        break;
                    case 4:
                        GameManager.g_CurrentRound = 5;
                        break;
                }

                if (g_AddPlayer1Score)
                {
                    switch (GameManager.g_Player1Score)
                    {
                        case 0:
                            GameManager.g_Player1Score = 1;
                            break;
                        case 1:
                            GameManager.g_Player1Score = 2;
                            break;
                        case 2:
                            GameManager.g_Player1Score = 3;
                            break;
                        case 3:
                            GameManager.g_Player1Score = 4;
                            break;
                        case 4:
                            GameManager.g_Player1Score = 5;
                            break;
                    }
                }
                else if (g_AddPlayer2Score)
                {
                    switch (GameManager.g_Player2Score)
                    {
                        case 0:
                            GameManager.g_Player2Score = 1;
                            break;
                        case 1:
                            GameManager.g_Player2Score = 2;
                            break;
                        case 2:
                            GameManager.g_Player2Score = 3;
                            break;
                        case 3:
                            GameManager.g_Player2Score = 4;
                            break;
                        case 4:
                            GameManager.g_Player2Score = 5;
                            break;
                    }
                }
                m_scoreCanvas.SetActive(true);
            }
        }

        IEnumerator IE_Audio(AudioSource _audioSource)
        {
            yield return new WaitForSecondsRealtime(1);
            _audioSource.Play();
        }
    }
}
