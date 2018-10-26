using RetroPlatform.Navigation;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroPlatform
{
    public class RandomBattle : MonoBehaviour
    {
        public int battleProbability;
        int encounterChance = 100;
        public int secondsBetweenBattles;
        public string battleSceneName;
        public Vector2 GoBackPosition;


        void OnTriggerEnter2D(Collider2D col)
        {
            encounterChance = Random.Range(1, 100);
            if (encounterChance > battleProbability)
            {
                StartCoroutine(RecalculateChance());
            }
        }

        IEnumerator RecalculateChance()
        {
            while (encounterChance > battleProbability)
            {
                yield return new WaitForSeconds(secondsBetweenBattles);
                encounterChance = Random.Range(1, 100);
            }
        }
        void OnTriggerStay2D(Collider2D col)
        {
            if (encounterChance <= battleProbability)
            {
                GameState.LastSceneName = SceneManager.GetActiveScene().name;
                GameState.SetLastScenePosition(GameState.LastSceneName, new Vector3(GoBackPosition.x, GoBackPosition.y, 0));
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
