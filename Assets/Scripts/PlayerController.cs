using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RetroPlatform
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D playerRigidBody2D;
        private Animator playerAnim;
        private SpriteRenderer playerSpriteImage;
        
        public Player playerCore;

        // Use this for initialization
        void Awake()
        {
            playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
            playerAnim = (Animator)GetComponent(typeof(Animator));
            playerSpriteImage = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));

            playerCore = new Player(new UnityEnvironmentData());
        }

        // Update is called once per frame
        void Update()
        {
            MovePlayer();
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
