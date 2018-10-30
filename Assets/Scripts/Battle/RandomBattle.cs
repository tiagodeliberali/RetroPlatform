using System.Collections;
using RetroPlatform.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroPlatform.Battle
{
    public class RandomBattle : MonoBehaviour
    {
        int encounterChance = 100;
        const string battleSceneName = "BattleScene";

        public int BattleProbability;
        public int SecondsBetweenBattles;
        public Vector2 GoBackPosition;
        public PlayerController Player;

        void OnTriggerEnter2D(Collider2D col)
        {
            encounterChance = Random.Range(1, 100);
            if (encounterChance > BattleProbability)
            {
                StartCoroutine(RecalculateChance());
            }
        }

        IEnumerator RecalculateChance()
        {
            while (encounterChance > BattleProbability)
            {
                yield return new WaitForSeconds(SecondsBetweenBattles);
                encounterChance = Random.Range(1, 100);
            }
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (encounterChance <= BattleProbability)
            {
                GameState.UpdatePlayerData(Player.PlayerCore);
                GameState.SetLastScene(SceneManager.GetActiveScene().name, new Vector3(GoBackPosition.x, GoBackPosition.y, 0));
                NavigationManager.NavigateTo(battleSceneName);
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            encounterChance = 100;
            StopCoroutine(RecalculateChance());
        }
    }
}
