using UnityEngine;

namespace Frank
{
    public class CharacterManager : MonoBehaviour
    {
        //[Range(0, 2)]
        public int player1Index, player2Index;
        public static int g_Player1Index, g_Player2Index;
        public GameObject[] player1Prefabs, player2Prefabs;

        void Start()
        {
            g_Player1Index = player1Index;
            g_Player2Index = player2Index;

            ShowCharacter(player1Prefabs, g_Player1Index);
            ShowCharacter(player2Prefabs, g_Player2Index);
        }

        public static void ShowCharacter(GameObject[] _objs, int _playerIndex)
        {
            _objs[_playerIndex].SetActive(true);

            for (int i = 0; i < _objs.Length; i++)
            {
                if (i != _playerIndex)
                    _objs[i].SetActive(false);
            }
        }
    }
}