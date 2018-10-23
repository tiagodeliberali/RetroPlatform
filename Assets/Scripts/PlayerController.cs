﻿using RetroPlatform.Conversation;
using UnityEngine;

namespace RetroPlatform
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D playerRigidBody2D;
        private Animator playerAnim;
        private SpriteRenderer playerSpriteImage;
        private Player playerCore;

        public UIController uiController;

        void Awake()
        {
            playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
            playerAnim = (Animator)GetComponent(typeof(Animator));
            playerSpriteImage = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));

            playerCore = new Player(new UnityEnvironmentData());
            playerCore.OnLivesChanged += () => uiController.UpdateLives(playerCore.Lives);
            playerCore.OnLivesFinished += PlayerCore_OnLivesFinished;
            playerCore.OnCoinsChanged += () => uiController.UpdateCoins(playerCore.Coins);

            playerCore.AddLives(3);
        }

        void Update()
        {
            MovePlayer();
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("FloorLimit"))
                playerCore.GetDamage(1);

            if (col.gameObject.CompareTag("Floor"))
                playerCore.HitFloor();

            if (col.gameObject.CompareTag("Boss"))
            {
                playerCore.StartConversation();
                uiController.StartConversation(
                    col.gameObject.GetComponent<ConversationComponent>().Conversations[0],
                    () =>
                    {
                        playerCore.FinishConversation();
                        col.gameObject.SetActive(false);
                    });
            }

            if (col.gameObject.CompareTag("Coin"))
            {
                playerCore.AddCoins(1);
                col.gameObject.SetActive(false);
            }
        }

        private void PlayerCore_OnLivesFinished()
        {
            Debug.Log("GAME OVER!");
        }

        private void MovePlayer()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                playerCore.Jump();

            playerCore.Move(Input.GetAxis("Horizontal"));
            playerRigidBody2D.velocity = playerCore.GetMovement(playerRigidBody2D.velocity);

            playerSpriteImage.flipX = playerCore.Direction == Direction.Left;
            playerAnim.SetBool("isRunning", playerCore.IsRunning);
        }
    }
}
