﻿using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D playerRigidBody2D;
        private Animator playerAnim;
        private SpriteRenderer playerSpriteImage;
        private Player playerCore;

        public GameObject UILives;
        public Text CoinsAmount;

        void Awake()
        {
            playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
            playerAnim = (Animator)GetComponent(typeof(Animator));
            playerSpriteImage = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));

            playerCore = new Player(new UnityEnvironmentData());
            playerCore.OnLivesChanged += PlayerCore_OnLivesChanged;
            playerCore.OnLivesFinished += PlayerCore_OnLivesFinished;
            playerCore.OnCoinsChanged += PlayerCore_OnCoinsChanged;

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

        private void PlayerCore_OnLivesChanged()
        {
            var lives = UILives.GetComponentsInChildren<CanvasRenderer>();
            for (int i = 0; i < lives.Length; i++)
                lives[i].SetAlpha(i < playerCore.Lives ? 100f : 0f);
        }

        private void PlayerCore_OnCoinsChanged()
        {
            CoinsAmount.text = playerCore.Coins.ToString();
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
