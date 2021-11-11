using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Frank
{
    #region Declare
    public class GameManager : MonoBehaviour
    {
        public GameData_Boxing gameData_scr;

        [Serializable]
        public class PkCanvas
        {
            public Text roundTxt, timeTxt;
        }

        [Serializable]
        public class ScoreCanvas
        {
            public Text roundTxt, player1ScoreTxt, player2ScoreTxt;
        }

        [Serializable]
        public class EndCanvas
        {
            //public GameObject restartButton, returnButton;
            public GameObject player1, player2;
            [HideInInspector] public Animator[] player1Animators, player2Animators;
            public GameObject[] player1Prefabs, player2Prefabs;
        }

        [SerializeField] GameObject m_ScreenCanvas, m_ScoreBoardCanvas, m_PkCanvas, m_ScoreCanvas, m_EndCanvas;
        [SerializeField] GameObject mainCamera, cam;
        [SerializeField] AudioSource buttonAudioSource;
        [SerializeField] GameObject[] m_GameObjects;

        public PkCanvas pkCanvas;
        public ScoreCanvas scoreCanvas;
        public EndCanvas endCanvas;

        public static float g_Timer = 60;
        public static float g_CurrentRound, g_MaxRound = 3;
        public static int g_Player1Score, g_Player2Score;
        public static bool g_IsStart = false;
        
        public bool Game_IsEnd { get { return isEnd; } }
        bool player1IsWinner, player2IsWinner, isEnd;
        #endregion

        private void Start()
        {
            Debug.Log("g_Player1Score:" + g_Player1Score + "\n" +
                "g_Player2Score:" + g_Player2Score + "\n" +
                "g_CurrentRound:" + g_CurrentRound + "\n" +
                "isEnd:" + isEnd + "\n" +
                "g_IsStart:" + g_IsStart);

            RoundToString(g_MaxRound);
            TimeToString();

            if (g_CurrentRound != 0)
            {
                m_PkCanvas.SetActive(false);
                m_ScreenCanvas.SetActive(true);
                m_ScoreBoardCanvas.SetActive(true);
                g_IsStart = true;

                foreach (GameObject obj in m_GameObjects)
                    obj.SetActive(false);
            }
        }

        private void Update()
        {
            //switch (g_CurrentRound)
            //{
            //    case 1:
            //        scoreCanvas.roundTxt.text = "第一回合";
            //        break;
            //    case 2:
            //        scoreCanvas.roundTxt.text = "第二回合";
            //        break;
            //    case 3:
            //        scoreCanvas.roundTxt.text = "第三回合";
            //        break;
            //    case 4:
            //        scoreCanvas.roundTxt.text = "第四回合";
            //        break;
            //    case 5:
            //        scoreCanvas.roundTxt.text = "第五回合";
            //        break;
            //    case 6:
            //        scoreCanvas.roundTxt.text = "第五回合";
            //        break;
            //}

            //用轉換的比較快~而且也比較萬用XD((就是計算公式很懶Orz
            scoreCanvas.roundTxt.text = "第" + ChangeChar((int)g_CurrentRound - 1) + "回合";

            scoreCanvas.player1ScoreTxt.text = "0" + g_Player1Score.ToString();
            scoreCanvas.player2ScoreTxt.text = "0" + g_Player2Score.ToString();

            if (m_ScoreCanvas.activeSelf == true)
                StartCoroutine(IE_EnterScene(gameData_scr.NowSceneName));//遊戲結束或繼續

            if (g_CurrentRound == g_MaxRound)
            {
                //mainCamera.SetActive(false);
                cam.SetActive(true);
                m_ScreenCanvas.SetActive(false);
                m_ScoreBoardCanvas.SetActive(false);
                m_EndCanvas.SetActive(true);

                //只顯示獲勝玩家
                if (g_Player1Score > g_Player2Score)
                {
                    endCanvas.player1.SetActive(true);
                }
                else if (g_Player1Score < g_Player2Score)
                {
                    endCanvas.player2.SetActive(true);
                }


                if (!isEnd)
                {
                    SetEndCanvas();
                    isEnd = true;
                }
            }
        }

        //數字轉國字
        string ChangeChar(int _number)//除非有人玩到100回合才會爆炸
        {
            if (_number < 0) { return "?"; }
            if (_number > 99) { Debug.LogError("超過100回合!"); return "99"; }
            string[] _char_array = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
            string _displayChar = string.Empty;//實際顯示文字
            int _tenNumber = -1;//十位數
            //超過10
            if (_number > 0)
            {
                _tenNumber = _number / 10;//計算出十位數
            }
            if (_tenNumber > 0)
            {
                _number = _number - (10 * _tenNumber);//計算出個位數
                _displayChar = _char_array[_tenNumber - 1] + _char_array[_number];//顯示訊息
                Debug.Log("_tenNumber:" + _tenNumber + "\n" + "_number:" + _number);
            }
            else
            {
                _displayChar = _char_array[_number];//顯示訊息
            }
            return _displayChar;
        }

        private void SetEndCanvas()
        {
            Time.timeScale = 0;

            CharacterManager.ShowCharacter(endCanvas.player1Prefabs, CharacterManager.g_Player1Index);
            CharacterManager.ShowCharacter(endCanvas.player2Prefabs, CharacterManager.g_Player2Index);

            endCanvas.player1Animators = endCanvas.player1.GetComponentsInChildren<Animator>();
            endCanvas.player2Animators = endCanvas.player2.GetComponentsInChildren<Animator>();

            if (g_Player1Score > g_Player2Score)
                player1IsWinner = true;
            else if (g_Player1Score < g_Player2Score)
                player2IsWinner = true;
            else
            {
                player1IsWinner = false;
                player2IsWinner = false;
            }

            if (player1IsWinner)
            {
                foreach (Animator animator in endCanvas.player1Animators)
                    animator.SetTrigger("Win");

                foreach (Animator animator in endCanvas.player2Animators)
                    animator.SetTrigger("Lose");
            }
            else if (player2IsWinner)
            {
                foreach (Animator animator in endCanvas.player1Animators)
                    animator.SetTrigger("Lose");

                foreach (Animator animator in endCanvas.player2Animators)
                    animator.SetTrigger("Win");
            }
            else
            {
                foreach (Animator animator in endCanvas.player1Animators)
                    animator.SetTrigger("Lose");

                foreach (Animator animator in endCanvas.player2Animators)
                    animator.SetTrigger("Lose");
            }
        }

        IEnumerator IE_EnterScene(string _scene)
        {
            yield return new WaitForSecondsRealtime(1);
            SceneManager.LoadScene(_scene);
        }

        private void RoundToString(float _round)
        {
            pkCanvas.roundTxt.text = "0" + _round.ToString();
        }

        private void TimeToString()
        {
            pkCanvas.timeTxt.text = g_Timer.ToString();
        }

        private void PlayButtonAudio()
        {
            buttonAudioSource.Play();
        }

        //設定回合
        public void SetRound(int _round) 
        {
            g_MaxRound = _round;
        }
        /// <summary>
        /// Click Button
        /// </summary>
        public void AddRound()
        {
            PlayButtonAudio();

            g_MaxRound += 2;
            if (g_MaxRound > 5)
                g_MaxRound = 5;

            RoundToString(g_MaxRound);
        }

        public void DecreaseRound()
        {
            PlayButtonAudio();

            g_MaxRound -= 2;
            if (g_MaxRound < 3)
                g_MaxRound = 3;

            RoundToString(g_MaxRound);
        }

        //設定時間
        public void SetTime(float _timed)
        {
            g_Timer = _timed;
        }
        public void AddTime()
        {
            PlayButtonAudio();

            g_Timer += 60;
            if (g_Timer > 120)
                g_Timer = 120;

            TimeToString();
        }

        public void DecreaseTime()
        {
            PlayButtonAudio();

            g_Timer -= 60;
            if (g_Timer < 60)
                g_Timer = 60;

            TimeToString();
        }

        public void OnClickStartButton()
        {
            //PlayButtonAudio();

            m_ScreenCanvas.SetActive(true);
            m_ScoreBoardCanvas.SetActive(true);
            g_IsStart = true;

            foreach (GameObject obj in m_GameObjects)
                obj.SetActive(false);
        }

        public void OnClickRestartButton()
        {
            //Time.timeScale = 1;
            //g_CurrentRound = 0;
            //isEnd = false;

            //PlayButtonAudio();

            //mainCamera.SetActive(true);
            //cam.SetActive(false);
            //m_ScreenCanvas.SetActive(true);
            //m_ScoreBoardCanvas.SetActive(true);
            //m_EndCanvas.SetActive(false);

            //endCanvas.player1.SetActive(false);
            //endCanvas.player2.SetActive(false);

            //g_IsStart = true;

            //foreach (GameObject obj in m_GameObjects)
            //    obj.SetActive(false);

            g_Player1Score = 0;
            g_Player2Score = 0;
            g_CurrentRound = 0;
            isEnd = false;
            g_IsStart = false;


            StartCoroutine(IE_EnterScene(gameData_scr.NowSceneName));//再次遊玩
        }

        public void OnClickReturnButton()
        {
            Time.timeScale = 1;
            //isEnd = false;
            //g_IsStart = false;//PS

            //g_Player1Score = 0;
            //g_Player2Score = 0;
            //g_CurrentRound = 0;

            g_Player1Score = 0;
            g_Player2Score = 0;
            g_CurrentRound = 0;
            isEnd  = g_IsStart = false;
            Debug.Log("g_Player1Score:" + g_Player1Score + "\n" +
                "g_Player2Score:" + g_Player2Score + "\n" +
                "g_CurrentRound:" + g_CurrentRound + "\n" +
                "isEnd:" + isEnd + "\n" +
                "g_IsStart:" + g_IsStart);

            //PlayButtonAudio();

            StartCoroutine(IE_EnterScene(GameMaster_koroshi.s_GameMaster.TitleSceneName));//返回標題
        }
    }
}
