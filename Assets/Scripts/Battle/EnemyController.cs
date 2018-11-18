using RetroPlatform.Battle;
using RetroPlatform.Battle.Enemies;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform
{
    public class EnemyController : MonoBehaviour
    {
        private PlayerController playerController;
        private Animator enemyAI;
        private AnimatorView<EnemyBattleState> enemyAnimatorView;
        private Slider lifeSlider;
        private CanvasGroup lifeCanvasGroup;
        
        public AnimationCurve SpawnAnimationCurve;
        public EnemyCore EnemyCore;
        public BattleController BattleController;
        public GameObject LifeCanvas;
        
        public EnemyBattleState BattleState { get; private set; }

        void Awake()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            enemyAI = GetComponent<Animator>();
            enemyAnimatorView = new AnimatorView<EnemyBattleState>(enemyAI);
            enemyAnimatorView.OnStatusChanged += BattleStateChanged;

            var life = Instantiate(LifeCanvas.gameObject);
            life.transform.SetParent(gameObject.transform);
            lifeSlider = life.GetComponent<Canvas>().GetComponentInChildren<Slider>();
            lifeCanvasGroup = life.GetComponent<CanvasGroup>();
        }

        private void BattleStateChanged(EnemyBattleState status)
        {
            CheckRunAway(status);
        }

        void Update()
        {
            enemyAnimatorView.GetCurrentStatus();

            UpdateAI();
            UpdateLives();
        }

        void OnMouseDown()
        {
            EnemyCore.SelectToBeAttacked();
        }

        void CheckRunAway(EnemyBattleState status)
        {
            if (status == EnemyBattleState.Run_Away)
            {
                EnemyCore.RunAway();
            }
        }

        void UpdateLives()
        {
            if (EnemyCore != null)
            {
                if (lifeSlider.maxValue == 0)
                {
                    lifeSlider.maxValue = EnemyCore.Health;
                    lifeCanvasGroup.alpha = 1;
                }

                lifeSlider.value = EnemyCore.Health;
            }
        }

        void UpdateAI()
        {
            if (enemyAI != null && EnemyCore != null)
            {
                enemyAI.SetInteger("EnemyHealth", EnemyCore.Health);
                enemyAI.SetInteger("PlayerHealth", playerController.PlayerCore.Lives);
                enemyAI.SetInteger("EnemiesInBattle", BattleController.EnemyCount);
                enemyAI.SetBool("PlayerSeen", true);
                enemyAI.SetBool("PlayerAttacking", BattleController.CurrentBattleState != Battle.BattleState.Enemy_Attack);
            }
        }
    }
}