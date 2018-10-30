using System.Collections;
using System.Collections.Generic;
using RetroPlatform.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform.Battle
{
    public class BattleController : MonoBehaviour
    {
        public GameObject[] EnemySpawnPoints;
        public GameObject[] EnemyPrefabs;
        public AnimationCurve SpawnAnimationCurve;
        public CanvasGroup Buttons;
        public PlayerController PlayerController;
        public Slider PlayerLife;
        public GameObject IntroPanel;
        public GameObject SwordParticle;
        public GameObject SelectionCircle;

        public BattleState CurrentBattleState { get; private set; }
        public int EnemyCount { get; private set; }

        public Text DebugInfo;

        Animator battleStateManager;
        AnimatorView<BattleState> battleAnimatorView;
        Animator introPanelAnim;
        GameObject attackParticle;
        Attack attack;
        PlayerCore playerCore;
        List<EnemyController> selectedEnemies = new List<EnemyController>();

        bool attacking = false;
        bool canSelectEnemy;
        bool canBeAttacked;
        int enemyAttackForce;

        void Awake()
        {
            battleStateManager = GetComponent<Animator>();
            battleAnimatorView = new AnimatorView<BattleState>(battleStateManager);
            introPanelAnim = IntroPanel.GetComponent<Animator>();
            attack = GetComponent<Attack>();
            playerCore = PlayerController.PlayerCore;

            playerCore.StartConversation();
        }

        void Start()
        {
            EnemyCount = Random.Range(1, EnemySpawnPoints.Length);
            StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            battleStateManager.SetBool("BattleReady", true);

            for (int i = 0; i < EnemyCount; i++)
            {
                var newEnemy = (GameObject)Instantiate(EnemyPrefabs[0]);
                newEnemy.transform.position = new Vector3(10, -1, 0);
                yield return StartCoroutine(MoveCharacterToPoint(EnemySpawnPoints[i].transform.position, newEnemy, 1));
                newEnemy.transform.parent = EnemySpawnPoints[i].transform;

                var enemyController = newEnemy.GetComponent<EnemyController>();
                enemyController.BattleController = this;
                enemyController.OnEnemySelected += EnemyController_OnEnemySelected;
                enemyController.OnEnemyDie += EnemyController_OnEnemyDie;
                enemyController.OnEnemyRunAway += EnemyController_OnEnemyRunAway;
                enemyController.DebugInfo = DebugInfo;

                var EnemyProfile = Enemy.GetByName(EnemyPrefabs[0].name);
                EnemyProfile.name = EnemyProfile.EnemyName + " " + i.ToString();
                enemyAttackForce += EnemyProfile.Attack;

                enemyController.EnemyProfile = EnemyProfile;
            }

            battleStateManager.SetBool("IntroFinished", true);
        }

        void EnemyController_OnEnemyRunAway(EnemyController enemy)
        {
            enemyAttackForce -= enemy.EnemyProfile.Attack;
            EnemyCount--;
            StartCoroutine(MoveCharacterToPoint(new Vector3(1300, enemy.transform.position.y, enemy.transform.position.z), enemy.gameObject, 0.1f));
        }

        void EnemyController_OnEnemyDie(EnemyController enemy)
        {
            enemyAttackForce -= enemy.EnemyProfile.Attack;
            EnemyCount--;
            enemy.gameObject.SetActive(false);
            Destroy(enemy);
        }

        void EnemyController_OnEnemySelected(EnemyController enemy)
        {
            if (!canSelectEnemy) return;

            var selectionCircleInstance = (GameObject)GameObject.Instantiate(SelectionCircle);
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

        IEnumerator MoveCharacterToPoint(Vector3 destination, GameObject character, float speed)
        {
            float timer = 0f;
            var StartPosition = character.transform.position;
            if (SpawnAnimationCurve.length > 0)
            {
                while (timer < SpawnAnimationCurve.keys[SpawnAnimationCurve.length - 1].time)
                {
                    character.transform.position = Vector3.Lerp(StartPosition, destination, SpawnAnimationCurve.Evaluate(timer));
                    timer += Time.deltaTime * speed;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                character.transform.position = destination;
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

        void Update()
        {
            CurrentBattleState = battleAnimatorView.GetCurrentStatus();
            if (DebugInfo != null) DebugInfo.text = CurrentBattleState.ToString();

            switch (CurrentBattleState)
            {
                case BattleState.Intro:
                    introPanelAnim.SetTrigger("Intro");
                    break;
                case BattleState.Player_Move:
                    canSelectEnemy = attack.ReadyToAttack;
                    break;
                case BattleState.Player_Attack:
                    canSelectEnemy = false;
                    if (!attacking && attack.ReadyToAttack) StartCoroutine(AttackTarget());
                    break;
                case BattleState.Change_Control:
                    ClearEnemySelection();
                    canBeAttacked = true;
                    break;
                case BattleState.Enemy_Attack:
                    if (canBeAttacked && EnemyCount > 0)
                    {
                        int damage = Random.Range(0, EnemyCount) * enemyAttackForce / EnemyCount;
                        Debug.Log("Damage: " + damage);
                        playerCore.GetDamage(damage);
                        GameState.UpdatePlayerData(playerCore);
                    }
                    canBeAttacked = false;
                    battleStateManager.SetBool("BattleReady", EnemyCount > 0);
                    break;
                case BattleState.Battle_Result:
                    break;
                case BattleState.Battle_End:
                    NavigationManager.NavigateTo(GameState.LastSceneName);
                    break;
                default:
                    break;
            }

            DisplayPlayerHUD();
        }

        void ClearEnemySelection()
        {
            StopCoroutine("SpinObject");

            DestroyGameObjects("SelectionCircle");
            DestroyGameObjects("SwordParticleSystem");

            selectedEnemies.Clear();
        }

        void DestroyGameObjects(string tag)
        {
            var gameObjectList = GameObject.FindGameObjectsWithTag(tag);
            foreach (var target in gameObjectList) Destroy(target);
        }

        void DisplayPlayerHUD()
        {
            PlayerLife.maxValue = playerCore.MaxLives;
            PlayerLife.value = playerCore.Lives;
            if (CurrentBattleState == BattleState.Player_Move)
            {
                Buttons.alpha = 1;
                Buttons.interactable = true;
                Buttons.blocksRaycasts = true;
            }
            else
            {
                Buttons.alpha = 0;
                Buttons.interactable = false;
                Buttons.blocksRaycasts = false;
            }
        }

        IEnumerator AttackTarget()
        {
            attacking = true;
            yield return new WaitForSeconds(1);

            foreach (var target in selectedEnemies)
            {
                attackParticle = (GameObject)GameObject.Instantiate(SwordParticle);
                attackParticle.tag = "SwordParticleSystem";

                if (attackParticle != null)
                {
                    attackParticle.transform.position = target.transform.position;
                }
                yield return new WaitForSeconds(1);
                target.EnemyProfile.Health -= attack.CurrentAttack.HitAmount;
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
}