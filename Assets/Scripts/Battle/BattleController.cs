using System.Collections;
using System.Collections.Generic;
using RetroPlatform.Battle.Enemies;
using RetroPlatform.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform.Battle
{
    public class BattleController : MonoBehaviour
    {
        public GameObject[] EnemySpawnPoints;
        public AnimationCurve SpawnAnimationCurve;
        public CanvasGroup Buttons;
        public PlayerController PlayerController;
        public GameObject IntroPanel;
        public GameObject SwordParticle;
        public GameObject SelectionCircle;
        public GameObject CollectableItem;
        public BattleDefinitionArray BattleDefinitionArray;

        public BattleState CurrentBattleState { get; private set; }
        public int EnemyCount
        {
            get
            {
                return battleCore == null ? 0 : battleCore.Enemies.Count;
            }
        }

        public Text DebugInfo;

        Animator battleStateManager;
        AnimatorView<BattleState> battleAnimatorView;
        Animator battlePanelAnim;
        Text battlePanelAnimText;
        Image[] enemyImage;
        GameObject attackParticle;
        Attack attack;
        PlayerCore playerCore;
        SpriteRenderer background;
        
        BattleDefinition battleDefinition;
        BattleCore battleCore;
        Dictionary<Enemy, EnemyController> enemyControllerDictionary = new Dictionary<Enemy, EnemyController>();

        void Awake()
        {
            if (GameState.BattleName == BattleName.None)
                throw new UnassignedReferenceException("Should not left BattleName as None");

            battleDefinition = BattleDefinition.GetBattle(GameState.BattleName, BattleDefinitionArray.BattleDefinitions);
            
            playerCore = PlayerController.PlayerCore;
            playerCore.StartConversation();

            battleCore = new BattleCore(battleDefinition, new UnityEnvironmentData());
            battleCore.OnReadyToAttack += BattleCore_OnReadyToAttack;
            battleCore.OnEnemySelected += BattleCore_OnEnemySelected;

            LoadComponents();
            LoadCollectable();
            SetBackground();
        }

        private void BattleCore_OnReadyToAttack()
        {
            battleStateManager.SetBool("PlayerReady", true);
            StartCoroutine(AttackTarget());
        }

        private void BattleCore_OnEnemySelected(Enemy enemy)
        {
            DrawSelectionCircle(enemyControllerDictionary[enemy]);
        }

        private void DrawSelectionCircle(EnemyController enemy)
        {
            var selectionCircleInstance = Instantiate(SelectionCircle);
            selectionCircleInstance.transform.parent = enemy.transform;
            selectionCircleInstance.transform.localPosition = new Vector3(0f, -1f, 0f);
            selectionCircleInstance.transform.localScale = new Vector3(4f, 4f, 1f);
            selectionCircleInstance.tag = "SelectionCircle";
            StartCoroutine("SpinObject", selectionCircleInstance);
        }

        private void LoadComponents()
        {
            battleStateManager = GetComponent<Animator>();
            battleAnimatorView = new AnimatorView<BattleState>(battleStateManager);
            battlePanelAnim = IntroPanel.GetComponent<Animator>();
            battlePanelAnimText = battlePanelAnim.GetComponentInChildren<Text>();
            enemyImage = battlePanelAnim.GetComponentsInChildren<Image>();
            enemyImage[2].sprite = battleDefinition.Info.EnemyPoster;
            attack = GetComponent<Attack>();
            attack.OnAttackSelected += (attack) => battleCore.ChooseAttack(attack);
        }

        private void SetBackground()
        {
            background = GetComponentInChildren<SpriteRenderer>();
            background.sprite = battleDefinition.Info.Background;
        }

        private void LoadCollectable()
        {
            if (battleDefinition.Info.Collectable)
            {
                CollectableItem.GetComponent<SpriteRenderer>().sprite = battleDefinition.Info.Collectable;
            }
            else
            {
                CollectableItem.SetActive(false);
            }
        }

        void Start()
        {
            battleCore.LoadEnemies();
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            battleStateManager.SetBool("BattleReady", true);
            int position = 0;

            foreach (var enemy in battleCore.Enemies)
            {
                var newEnemy = Instantiate(battleDefinition.Info.EnemyPrefab);
                newEnemy.transform.position = new Vector3(10, -1, 0);
                newEnemy.transform.localScale = new Vector3(battleDefinition.Info.EnemyScale, battleDefinition.Info.EnemyScale, 0);
                yield return StartCoroutine(MoveObjectToPoint(EnemySpawnPoints[position].transform.position, newEnemy, 1));
                newEnemy.transform.parent = EnemySpawnPoints[position].transform;

                var enemyController = newEnemy.GetComponent<EnemyController>();
                enemyController.BattleController = this;
                enemyController.EnemyProfile = enemy;

                enemy.OnDie += Enemy_OnDie;
                enemy.OnRunAway += Enemy_OnRunAway;

                enemyControllerDictionary.Add(enemy, enemyController);

                position++;
            }

            battleStateManager.SetBool("IntroFinished", true);
        }

        private void Enemy_OnRunAway(Enemy enemy)
        {
            EnemyController controller = enemyControllerDictionary[enemy];
            StartCoroutine(MoveObjectToPoint(new Vector3(1300, controller.transform.position.y, controller.transform.position.z), controller.gameObject, 0.1f));
        }

        private void Enemy_OnDie(Enemy enemy)
        {
            EnemyController controller = enemyControllerDictionary[enemy];
            controller.gameObject.SetActive(false);
            Destroy(controller);
        }

        private IEnumerator MoveObjectToPoint(Vector3 destination, GameObject gameObject, float speed)
        {
            float timer = 0f;
            var StartPosition = gameObject.transform.position;
            if (SpawnAnimationCurve.length > 0)
            {
                while (timer < SpawnAnimationCurve.keys[SpawnAnimationCurve.length - 1].time)
                {
                    gameObject.transform.position = Vector3.Lerp(StartPosition, destination, SpawnAnimationCurve.Evaluate(timer));
                    timer += Time.deltaTime * speed;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                gameObject.transform.position = destination;
            }
        }

        private IEnumerator SpinObject(GameObject target)
        {
            while (true)
            {
                target.transform.Rotate(0, 0, 180 * Time.deltaTime);
                yield return null;
            }
        }

        void Update()
        {
            if (CurrentBattleState != battleAnimatorView.GetCurrentStatus())
            {
                CurrentBattleState = battleAnimatorView.GetCurrentStatus();
                ExecuteBattleStateAction();
            }

            if (DebugInfo != null) DebugInfo.text = CurrentBattleState.ToString();

            DisplayPlayerHUD();
        }

        private void ExecuteBattleStateAction()
        {
            switch (CurrentBattleState)
            {
                case BattleState.Intro:
                    battlePanelAnim.SetTrigger("Intro");
                    break;
                case BattleState.Player_Move:
                    break;
                case BattleState.Player_Attack:
                    break;
                case BattleState.Change_Control:
                    break;
                case BattleState.Enemy_Attack:
                    battleCore.AttackPlayer(playerCore);
                    battleStateManager.SetBool("BattleReady", battleCore.Enemies.Count > 0);
                    attack.ClearAttack();
                    break;
                case BattleState.Battle_Result:
                    if (playerCore.Lives > 0)
                    {
                        if (CollectableItem != null && battleDefinition.Result == BattleResult.None)
                        {
                            Vector3 collectablePosition = new Vector3(PlayerController.transform.position.x + 2, PlayerController.transform.position.y + 1);
                            StartCoroutine(MoveObjectToPoint(collectablePosition, CollectableItem, 2f));
                        }
                        battleDefinition.Result = BattleResult.Win;
                        battlePanelAnimText.text = "Você venceu!";
                    }
                    else
                    {
                        battleDefinition.Result = BattleResult.Lose;
                        battlePanelAnimText.text = "Você perdeu";
                    }
                    battlePanelAnimText.fontSize = 60;
                    battlePanelAnim.SetTrigger("Finish");
                    break;
                case BattleState.Battle_End:
                    NavigationManager.NavigateTo(GameState.LastSceneName);
                    break;
                default:
                    break;
            }
        }

        private void DisplayPlayerHUD()
        {
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

        private IEnumerator AttackTarget()
        {
            yield return new WaitForSeconds(1);
            
            foreach (var target in battleCore.EnemiesUderAttack)
            {
                attackParticle = Instantiate(SwordParticle);
                attackParticle.tag = "SwordParticleSystem";
                attackParticle.transform.position = enemyControllerDictionary[target].transform.position;
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(2);
            battleCore.AttackEnemies();
            battleStateManager.SetBool("NoMoreEnemies", battleCore.Enemies.Count == 0);
            battleStateManager.SetBool("PlayerReady", false);
            ClearEnemySelection();
        }

        private void ClearEnemySelection()
        {
            StopCoroutine("SpinObject");
            DestroyGameObjects("SelectionCircle");
            DestroyGameObjects("SwordParticleSystem");
        }

        private void DestroyGameObjects(string tag)
        {
            var gameObjectList = GameObject.FindGameObjectsWithTag(tag);
            foreach (var target in gameObjectList) Destroy(target);
        }

        public void RunAway()
        {
            battleDefinition.Result = BattleResult.RunAway;
            NavigationManager.NavigateTo(GameState.LastSceneName);
        }
    }
}