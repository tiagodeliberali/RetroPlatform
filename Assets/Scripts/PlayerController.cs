using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Battle;
using RetroPlatform.Levels;
using RetroPlatform.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroPlatform
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody2D playerRigidBody2D;
        Animator playerAnim;
        SpriteRenderer playerSpriteImage;

        UIHelper uiHelper = new UIHelper();
        bool fadeOut;

        PlayerCore _core;
        public PlayerCore PlayerCore
        {
            get
            {
                if (_core == null) _core = new PlayerCore();
                return _core;
            }
        }

        public UIController uiController;
        public Texture2D FadeTexture;
        public bool mapMovement;

        void Awake()
        {
            playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
            playerAnim = (Animator)GetComponent(typeof(Animator));
            playerSpriteImage = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));

            PlayerCore.EnvironmentData = new UnityEnvironmentData();

            if (uiController != null)
            {
                PlayerCore.OnLivesChanged += () => uiController.UpdateLives(PlayerCore.Lives, PlayerCore.MaxLives);
                PlayerCore.OnCoinsChanged += () => uiController.UpdateCoins(PlayerCore.Coins);

                uiController.OnStartConversation += () => PlayerCore.StartConversation();
                uiController.OnFinishConversation += () => PlayerCore.FinishConversation();
            }

            GameState.LoadPlayer(PlayerCore);

            PlayerCore.OnPlayerDie += PlayerCore_OnPlayerDie;
            PlayerCore.OnPlayerProtected += PlayerCore_OnPlayerProtected;
            PlayerCore.OnLivesChanged += PlayerCore_UpdatePlayerData;
            PlayerCore.OnCoinsChanged += PlayerCore_UpdatePlayerData;

            var lastPosition = GameState.GetLastScenePosition(SceneManager.GetActiveScene().name);
            if (lastPosition != Vector3.zero) transform.position = lastPosition;
        }

        private void PlayerCore_UpdatePlayerData()
        {
            GameState.UpdatePlayerData(PlayerCore);
        }

        private void PlayerCore_OnPlayerProtected()
        {
            StartCoroutine(FinishPlayerProtection());
        }

        void Update()
        {
            if (!mapMovement) MovePlayerOnPlatform();
            else MovePlayerOnMap();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
                PlayerCore.HitFloor();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("FloorLimit") || collision.gameObject.CompareTag("Trap"))
                PlayerCore.GetDamage(1);
        }

        IEnumerator FinishPlayerProtection()
        {
            yield return new WaitForSeconds(0.5f);
            PlayerCore.FinishProtection();
        }

        void PlayerCore_OnPlayerDie()
        {
            fadeOut = true;
            StartCoroutine(GameOver());
        }

        IEnumerator GameOver()
        {
            yield return new WaitForSeconds(5f);
            GameState.Lives = PlayerCore.MaxLives;
            if (!EntryLevelController.LevelConcluded)
            {
                NavigationManager.NavigateTo(LevelName.EntryLevel);
            }
            else
            {
                NavigationManager.NavigateTo("Overworld");
            }
        }

        void OnGUI()
        {
            uiHelper.FadeOut(fadeOut, FadeTexture);
        }

        void MovePlayerOnPlatform()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                PlayerCore.Jump();

            PlayerCore.Move(Input.GetAxis("Horizontal"));
            playerRigidBody2D.velocity = PlayerCore.GetMovement(playerRigidBody2D.velocity);

            playerSpriteImage.flipX = PlayerCore.Direction == Direction.Left;
            playerAnim.SetBool("isRunning", PlayerCore.IsRunning);
        }

        void MovePlayerOnMap()
        {
            float velocity = 3f;
            playerRigidBody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * velocity, Input.GetAxis("Vertical") * velocity);
            playerAnim.SetBool("isRunning", Math.Pow(Input.GetAxis("Horizontal"), 2) + Math.Pow(Input.GetAxis("Vertical"), 2) != 0);

            if (Input.GetAxis("Horizontal") > 0) playerSpriteImage.flipX = false;
            else if (Input.GetAxis("Horizontal") < 0) playerSpriteImage.flipX = true;
        }
    }
}
