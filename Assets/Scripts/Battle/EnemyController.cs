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
        public Enemy EnemyProfile;
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
            EnemyProfile.SelectToBeAttacked();
        }

        void CheckRunAway(EnemyBattleState status)
        {
            if (status == EnemyBattleState.Run_Away)
            {
                EnemyProfile.RunAway();
            }
        }

        void UpdateLives()
        {
            if (EnemyProfile != null)
            {
                if (lifeSlider.maxValue == 0)
                {
                    lifeSlider.maxValue = EnemyProfile.Health;
                    lifeCanvasGroup.alpha = 1;
                }

                lifeSlider.value = EnemyProfile.Health;
            }
        }

        void UpdateAI()
        {
            if (enemyAI != null && EnemyProfile != null)
            {
                enemyAI.SetInteger("EnemyHealth", EnemyProfile.Health);
                enemyAI.SetInteger("PlayerHealth", playerController.PlayerCore.Lives);
                enemyAI.SetInteger("EnemiesInBattle", BattleController.EnemyCount);
                enemyAI.SetBool("PlayerSeen", true);
                enemyAI.SetBool("PlayerAttacking", BattleController.CurrentBattleState != Battle.BattleState.Enemy_Attack);
            }
        }
    }
}