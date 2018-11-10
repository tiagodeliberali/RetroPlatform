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
        public SpriteRenderer collectable;
        public int enemy;
        public int maxEnemies;
        public float enemyScale;
        public SpriteRenderer background;
        public bool FightOnTouch = false;

        public void DisableZone()
        {
            BoxCollider2D boxColider = (BoxCollider2D)GetComponent(typeof(BoxCollider2D));
            boxColider.gameObject.SetActive(false);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (FightOnTouch) return;
            Calculate();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!FightOnTouch) return;
            Calculate();
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (FightOnTouch) return;
            Fight();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!FightOnTouch) return;
            Fight();
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (FightOnTouch) return;
            FinishCalculate();
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!FightOnTouch) return;
            FinishCalculate();
        }

        void Calculate()
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

        void Fight()
        {
            if (encounterChance <= BattleProbability)
            {
                GameState.SetLastScene(SceneManager.GetActiveScene().name, new Vector3(GoBackPosition.x, GoBackPosition.y, 0));
                GameState.BattleCollectable = collectable == null ? null : collectable.sprite;
                GameState.BattleEnemy = enemy;
                GameState.BattleMaxEnemies = maxEnemies;
                GameState.BattleMaxEnemyScale = enemyScale;
                GameState.BattleBackground = background == null ? null : background.sprite;
                GameState.BattleResult = BattleResult.None;
                NavigationManager.NavigateTo(battleSceneName);
            }
        }

        private void FinishCalculate()
        {
            encounterChance = 100;
            StopCoroutine(RecalculateChance());
        }
    }
}
