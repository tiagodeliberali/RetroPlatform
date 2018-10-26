using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroPlatform
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D playerRigidBody2D;
        private Animator playerAnim;
        private SpriteRenderer playerSpriteImage;
        private PlayerCore playerCore;

        public Player player;
        public UIController uiController;

        void Awake()
        {
            playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
            playerAnim = (Animator)GetComponent(typeof(Animator));
            playerSpriteImage = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));

            playerCore = player.Core;
            playerCore.EnvironmentData = new UnityEnvironmentData();

            if (uiController != null)
            {
                playerCore.OnLivesChanged += () => uiController.UpdateLives(playerCore.Lives);
                playerCore.OnLivesFinished += PlayerCore_OnLivesFinished;
                playerCore.OnCoinsChanged += () => uiController.UpdateCoins(playerCore.Coins);

                uiController.OnStartConversation += () => playerCore.StartConversation();
                uiController.OnFinishConversation += () => playerCore.FinishConversation();

                playerCore.AddLives(GameState.lives);
                playerCore.AddCoins(GameState.coins);

                var lastPosition = GameState.GetLastScenePosition(SceneManager.GetActiveScene().name);
                if (lastPosition != Vector3.zero) transform.position = lastPosition;
            }
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
