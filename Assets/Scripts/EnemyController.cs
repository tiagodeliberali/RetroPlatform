using RetroPlatform.Battle;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform
{
    public class EnemyController : MonoBehaviour
    {
        public AnimationCurve SpawnAnimationCurve;
        public Enemy EnemyProfile;
        public BattleController BattleController;
        PlayerController player;
        Animator enemyAI;
        AnimatorView<EnemyBattleState> enemyAnimatorView;
        public GameObject lifeCanvas;
        Slider lifeSlider;
        CanvasGroup lifeCanvasGroup;
        public EnemyBattleState curentSatus;

        public event Action<EnemyController> OnEnemySelected;
        public event Action<EnemyController> OnEnemyDie;
        public event Action<EnemyController> OnEnemyRunAway;

        public Text DebugInfo;

        public EnemyBattleState BattleState { get; private set; }

        void OnMouseDown()
        {
            if (OnEnemySelected != null) OnEnemySelected(this);
        }

        public void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            enemyAI = GetComponent<Animator>();
            enemyAnimatorView = new AnimatorView<EnemyBattleState>(enemyAI);

            var life = Instantiate(lifeCanvas.gameObject);
            life.transform.SetParent(gameObject.transform);
            lifeSlider = life.GetComponent<Canvas>().GetComponentInChildren<Slider>();
            lifeCanvasGroup = life.GetComponent<CanvasGroup>();
        }

        void Update()
        {
            curentSatus = enemyAnimatorView.GetCurrentStatus();
            if (DebugInfo != null) DebugInfo.text = curentSatus.ToString();

            if (OnEnemyRunAway != null && curentSatus == EnemyBattleState.Run_Away)
            {
                OnEnemyRunAway(this);
                OnEnemyRunAway = null;
            }

            UpdateAI();

            if (EnemyProfile != null)
            {
                if (lifeSlider.maxValue == 0)
                {
                    lifeSlider.maxValue = EnemyProfile.health;
                    lifeCanvasGroup.alpha = 1;
                }
                lifeSlider.value = EnemyProfile.health;
                if (EnemyProfile.health <= 0 && OnEnemyDie != null) OnEnemyDie(this);
            }
        }

        public void UpdateAI()
        {
            if (enemyAI != null && EnemyProfile != null)
            {
                enemyAI.SetInteger("EnemyHealth", EnemyProfile.health);
                enemyAI.SetInteger("PlayerHealth", player.PlayerCore.Lives);
                enemyAI.SetInteger("EnemiesInBattle", BattleController.EnemyCount);
                enemyAI.SetBool("PlayerSeen", true);
                enemyAI.SetBool("PlayerAttacking", BattleController.currentBattleState != Battle.BattleState.Enemy_Attack);
            }
        }
    }

    public enum EnemyBattleState
    {
        Idle,
        Attack,
        Defend,
        Run_Away
    }
}