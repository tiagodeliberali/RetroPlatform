using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroPlatform
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody2D playerRigidBody2D;
        Animator playerAnim;
        SpriteRenderer playerSpriteImage;

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

        void Awake()
        {
            playerRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
            playerAnim = (Animator)GetComponent(typeof(Animator));
            playerSpriteImage = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));

            PlayerCore.EnvironmentData = new UnityEnvironmentData();

            if (uiController != null)
            {
                PlayerCore.OnLivesChanged += () => uiController.UpdateLives(PlayerCore.Lives, PlayerCore.MaxLives);
                PlayerCore.OnLivesFinished += PlayerCore_OnLivesFinished;
                PlayerCore.OnCoinsChanged += () => uiController.UpdateCoins(PlayerCore.Coins);

                uiController.OnStartConversation += () => PlayerCore.StartConversation();
                uiController.OnFinishConversation += () => PlayerCore.FinishConversation();
            }

            if (GameState.Lives == -1) GameState.Lives = PlayerCore.MaxLives;

            PlayerCore.AddLives(GameState.Lives);
            PlayerCore.AddCoins(GameState.Coins);

            var lastPosition = GameState.GetLastScenePosition(SceneManager.GetActiveScene().name);
            if (lastPosition != Vector3.zero) transform.position = lastPosition;
        }

        void Update()
        {
            MovePlayer();
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("FloorLimit"))
                PlayerCore.GetDamage(1);

            if (col.gameObject.CompareTag("Floor"))
                PlayerCore.HitFloor();
        }

        void PlayerCore_OnLivesFinished()
        {
            Debug.Log("GAME OVER!");
        }

        void MovePlayer()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                PlayerCore.Jump();

            PlayerCore.Move(Input.GetAxis("Horizontal"));
            playerRigidBody2D.velocity = PlayerCore.GetMovement(playerRigidBody2D.velocity);

            playerSpriteImage.flipX = PlayerCore.Direction == Direction.Left;
            playerAnim.SetBool("isRunning", PlayerCore.IsRunning);
        }
    }
}
