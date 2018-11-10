using System.Collections;
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

        float alpha = 0f;
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

            if (GameState.Lives == -1) GameState.Lives = PlayerCore.MaxLives;

            PlayerCore.OnPlayerDie += PlayerCore_OnPlayerDie;
            PlayerCore.OnPlayerProtected += PlayerCore_OnPlayerProtected;

            PlayerCore.AddLives(GameState.Lives);
            PlayerCore.AddCoins(GameState.Coins);

            var lastPosition = GameState.GetLastScenePosition(SceneManager.GetActiveScene().name);
            if (lastPosition != Vector3.zero) transform.position = lastPosition;
        }

        private void PlayerCore_OnPlayerProtected()
        {
            StartCoroutine(FinishPlayerProtection());
        }

        void Update()
        {
            MovePlayer();
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
            Debug.Log("GAME OVER!");
            fadeOut = true;
            StartCoroutine(GameOver());
        }

        IEnumerator GameOver()
        {
            yield return new WaitForSeconds(5f);
            GameState.Lives = PlayerCore.MaxLives;
            if (!EntryLevelController.BossLeftTheScene)
            {
                NavigationManager.NavigateTo(EntryLevelController.ENTRY_LEVEL);
            }
            else
            {
                NavigationManager.NavigateTo("Overworld");
            }
        }

        void OnGUI()
        {
            float fadespeed = 0.5f;

            if (!fadeOut) return;

            alpha -= -1 * fadespeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            Color newColor = GUI.color;
            newColor.a = alpha;
            GUI.color = newColor;
            GUI.depth = -1000;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);
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
