using RetroPlatform.Battle;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform
{
    public class EnemyController : MonoBehaviour
    {
        public Enemy EnemyProfile;
        public BattleController BattleController;
        PlayerController player;
        Animator enemyAI;
        public GameObject lifeCanvas;
        Slider lifeSlider;
        CanvasGroup lifeCanvasGroup;

        public event Action<EnemyController> OnEnemySelected;
        public event Action<EnemyController> OnEnemyDie;

        void OnMouseDown()
        {
            if (OnEnemySelected != null) OnEnemySelected(this);
        }

        public void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            enemyAI = GetComponent<Animator>();

            var life = Instantiate(lifeCanvas.gameObject);
            life.transform.SetParent(gameObject.transform);
            lifeSlider = life.GetComponent<Canvas>().GetComponentInChildren<Slider>();
            lifeCanvasGroup = life.GetComponent<CanvasGroup>();
        }

        void Update()
        {
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
            }
        }
    }
}