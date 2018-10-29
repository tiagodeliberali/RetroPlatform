using UnityEngine;

namespace RetroPlatform
{
    public class PlayerCore
    {
        public static readonly float SPEED = 400f;
        public static readonly float JUMP_SPEED = 20f;
        public static readonly int MAX_JUMPS = 2;

        public bool IsJumping { get; private set; }
        public bool IsRunning { get; private set; }
        public Direction Direction { get; private set; }
        public int Lives { get; private set; }
        public int MaxLives { get; private set; }
        public int Coins { get; private set; }

        public delegate void DataChanged();
        public event DataChanged OnLivesChanged;
        public event DataChanged OnLivesFinished;
        public event DataChanged OnCoinsChanged;

        public IEnvironmentData EnvironmentData;
        private float movePlayerHorizontal;
        private int currentJump = 0;
        private bool isTalking;

        public PlayerCore()
        {
            Direction = Direction.Rigth;
            MaxLives = 8;
        }

        public void Jump()
        {
            if (isTalking) return;

            IsJumping = true;
            currentJump++;
        }

        public void HitFloor()
        {
            currentJump = 0;
        }

        public void Move(float direction)
        {
            if (isTalking) return;

            movePlayerHorizontal = direction * SPEED * EnvironmentData.GetDeltaTime();
            IsRunning = movePlayerHorizontal != 0;

            if (movePlayerHorizontal < 0)
                Direction = Direction.Left;
            else if (movePlayerHorizontal > 0)
                Direction = Direction.Rigth;
        }

        public Vector2 GetMovement(Vector2 currentMovement)
        {
            if (isTalking) return new Vector2(0f, currentMovement.y);

            var movement = new Vector2(movePlayerHorizontal, ShouldApplyJump() ? JUMP_SPEED : currentMovement.y);
            IsJumping = false;

            return movement;
        }

        private bool ShouldApplyJump()
        {
            bool shouldJump = IsJumping && currentJump <= MAX_JUMPS;
            IsJumping = false;

            return shouldJump;
        }

        public void AddLives(int lives)
        {
            Lives += lives;
            CallLifeEvents();
        }

        public void GetDamage(int damage)
        {
            Lives -= damage;
            CallLifeEvents();
        }

        public void AddCoins(int coins)
        {
            Coins += coins;

            if (OnCoinsChanged != null)
                OnCoinsChanged();
        }

        private void CallLifeEvents()
        {
            if (OnLivesChanged != null)
                OnLivesChanged();

            if (Lives <= 0 && OnLivesFinished != null)
                OnLivesFinished();
        }

        public void FinishConversation()
        {
            isTalking = false;
            currentJump = 0;
        }

        public void StartConversation()
        {
            isTalking = true;
            IsRunning = false;
            IsJumping = false;
        }
    }
}
