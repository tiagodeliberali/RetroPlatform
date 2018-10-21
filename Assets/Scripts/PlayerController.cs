using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        void Awake()
        {
            playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
            playerAnim = (Animator)GetComponent(typeof(Animator));
            playerSpriteImage = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));

            playerCore = new Player(new UnityEnvironmentData());
            playerCore.OnLivesChanged += PlayerCore_OnLivesChanged;

            playerCore.AddLives(3);
        }

        private void PlayerCore_OnLivesChanged(int totalLives)
        {
            var lives = UILives.GetComponentsInChildren<CanvasRenderer>();
            for (int i = 0; i < lives.Length; i++)
            {
                lives[i].SetAlpha(i < totalLives ? 100f : 0f);
            }
        }

        void Update()
        {
            MovePlayer();
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("FloorLimit"))
            {
                Debug.Log("Caiu no buraco!");
                playerCore.GetDamage(1);
            }
        }

        private void MovePlayer()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                playerCore.Jump();

            playerCore.Move(Input.GetAxis("Horizontal"));
            playerRigidBody2D.velocity = playerCore.GetMovement(playerRigidBody2D.velocity);

            playerSpriteImage.flipX = playerCore.GetDirection() == Direction.Left;
            playerAnim.SetBool("isRunning", playerCore.IsRunning());
        }
    }
}
