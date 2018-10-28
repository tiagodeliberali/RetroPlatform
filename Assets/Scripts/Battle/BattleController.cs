using RetroPlatform.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform.Battle
{
    public class BattleController : MonoBehaviour
    {
        public GameObject[] EnemySpawnPoints;
        public GameObject[] EnemyPrefabs;
        public AnimationCurve SpawnAnimationCurve;
        public CanvasGroup buttons;
        public Text DebugInfo;

        public Animator battleStateManager;
        private Dictionary<int, BattleState> battleStateHash = new Dictionary<int, BattleState>();
        private BattleState currentBattleState;

        public GameObject introPanel;
        Animator introPanelAnim;


        public GameObject selectionCircle;
        public List<EnemyController> selectedEnemies = new List<EnemyController>();
        
        public GameObject swordParticle;
        private GameObject attackParticle;

        private Attack attack;

        bool attacking = false;

        bool canSelectEnemy;
        public int EnemyCount { get; private set; }

        void Awake()
        {
            battleStateManager = GetComponent<Animator>();
            introPanelAnim = introPanel.GetComponent<Animator>();
            attack = GetComponent<Attack>();
        }

        void Start()
        {
            EnemyCount = Random.Range(1, EnemySpawnPoints.Length);
            StartCoroutine(SpawnEnemies());

            GetAnimationStates();
        }

        IEnumerator SpawnEnemies()
        {
            battleStateManager.SetBool("BattleReady", true);

            for (int i = 0; i < EnemyCount; i++)
            {
                var newEnemy = (GameObject)Instantiate(EnemyPrefabs[0]);
                newEnemy.transform.position = new Vector3(10, -1, 0);
                yield return StartCoroutine(MoveCharacterToPoint(EnemySpawnPoints[i], newEnemy));
                newEnemy.transform.parent = EnemySpawnPoints[i].transform;

                var enemyController = newEnemy.GetComponent<EnemyController>();
                enemyController.BattleController = this;
                enemyController.OnEnemySelected += EnemyController_OnEnemySelected;
                enemyController.OnEnemyDie += EnemyController_OnEnemyDie;

                var EnemyProfile = ScriptableObject.CreateInstance<Enemy>();
                EnemyProfile.enemyClass = EnemyClass.Dragon;
                EnemyProfile.health = 20;
                EnemyProfile.name = EnemyProfile.enemyClass + " " + i.ToString();

                enemyController.EnemyProfile = EnemyProfile;
            }
        }

        private void EnemyController_OnEnemyDie(EnemyController enemy)
        {
            EnemyCount--;
            enemy.gameObject.SetActive(false);
            Destroy(enemy);
        }

        IEnumerator MoveCharacterToPoint(GameObject destination, GameObject character)
        {
            float timer = 0f;
            var StartPosition = character.transform.position;
            if (SpawnAnimationCurve.length > 0)
            {
                while (timer < SpawnAnimationCurve.keys[SpawnAnimationCurve.length - 1].time)
                {
                    character.transform.position = Vector3.Lerp(StartPosition, destination.transform.position, SpawnAnimationCurve.Evaluate(timer));
                    timer += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                character.transform.position = destination.transform.position;
            }
        }

        private void EnemyController_OnEnemySelected(EnemyController enemy)
        {
            if (!canSelectEnemy) return;

            var selectionCircleInstance = (GameObject)GameObject.Instantiate(selectionCircle);
            selectionCircleInstance.transform.parent = enemy.transform;
            selectionCircleInstance.transform.localPosition = new Vector3(0f, -1f, 0f);
            selectionCircleInstance.transform.localScale = new Vector3(4f, 4f, 1f);
            selectionCircleInstance.tag = "SelectionCircle";
            StartCoroutine("SpinObject", selectionCircleInstance);
            SelectEnemy(enemy);

            if (attack.IsReadyToAttack(selectedEnemies.Count) || selectedEnemies.Count == EnemyCount)
            {
                battleStateManager.SetBool("PlayerReady", true);
            }
        }

        IEnumerator SpinObject(GameObject target)
        {
            while (true)
            {
                target.transform.Rotate(0, 0, 180 * Time.deltaTime);
                yield return null;
            }
        }

        void GetAnimationStates()
        {
            foreach (BattleState state in (BattleState[])
              System.Enum.GetValues(typeof(BattleState)))
            {
                battleStateHash.Add(Animator.StringToHash(state.ToString()), state);
            }
        }

        void Update()
        {
            currentBattleState = battleStateHash[battleStateManager.GetCurrentAnimatorStateInfo(0).shortNameHash];
            DebugInfo.text = currentBattleState.ToString();

            switch (currentBattleState)
            {
                case BattleState.Intro:
                    introPanelAnim.SetTrigger("Intro");
                    break;
                case BattleState.Player_Move:
                    canSelectEnemy = attack.ReadyToAttack;
                    break;
                case BattleState.Player_Attack:
                    canSelectEnemy = false;
                    if (!attacking && attack.ReadyToAttack)
                    {
                        StartCoroutine(AttackTarget());
                    }
                    break;
                case BattleState.Change_Control:
                    ClearEnemySelection();
                    break;
                case BattleState.Enemy_Attack:
                    battleStateManager.SetBool("BattleReady", EnemyCount > 0);
                    break;
                case BattleState.Battle_Result:
                    break;
                case BattleState.Battle_End:
                    break;
                default:
                    break;
            }

            DisplayPlayerHUD();
        }

        private void ClearEnemySelection()
        {
            StopCoroutine("SpinObject");

            var circles = GameObject.FindGameObjectsWithTag("SelectionCircle");
            foreach (var target in circles)
            {
                Destroy(target);
            }

            var particles = GameObject.FindGameObjectsWithTag("SwordParticleSystem");
            foreach (var target in particles)
            {
                Destroy(target);
            }

            selectedEnemies.Clear();
        }

        private void DisplayPlayerHUD()
        {
            if (currentBattleState == BattleState.Player_Move)
            {
                buttons.alpha = 1;
                buttons.interactable = true;
                buttons.blocksRaycasts = true;
            }
            else
            {
                buttons.alpha = 0;
                buttons.interactable = false;
                buttons.blocksRaycasts = false;
            }
        }

        IEnumerator AttackTarget()
        {
            attacking = true;
            yield return new WaitForSeconds(1);

            foreach (var target in selectedEnemies)
            {
                attackParticle = (GameObject)GameObject.Instantiate(swordParticle);
                attackParticle.tag = "SwordParticleSystem";

                if (attackParticle != null)
                {
                    attackParticle.transform.position = target.transform.position;
                }
                yield return new WaitForSeconds(1);
                target.EnemyProfile.health -= attack.CurrentAttack.HitAmount;
            }
            attack.ClearAttack();
            attacking = false;
            battleStateManager.SetBool("PlayerReady", false);
        }

        public void RunAway()
        {
            NavigationManager.NavigateTo(GameState.LastSceneName);
        }

        public void SelectEnemy(EnemyController enemy)
        {
            selectedEnemies.Add(enemy);
        }
    }

    public enum BattleState
    {
        Begin_Battle,
        Intro,
        Player_Move,
        Player_Attack,
        Change_Control,
        Enemy_Attack,
        Battle_Result,
        Battle_End
    }
}