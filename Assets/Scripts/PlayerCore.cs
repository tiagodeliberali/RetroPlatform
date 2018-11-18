using System.Collections.Generic;
using Assets.Scripts.Battle;
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
        public bool Protected { get; private set; }
        public List<AttackName> Attacks { get; set; }

        public delegate void DataChanged();
        public event DataChanged OnLivesChanged;
        public event DataChanged OnPlayerDie;
        public event DataChanged OnPlayerProtected;
        public event DataChanged OnCoinsChanged;
        public IEnvironmentData EnvironmentData;

        float movePlayerHorizontal;
        int currentJump = 0;
        bool isTalking;

        public PlayerCore()
        {
            Attacks = new List<AttackName>();
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
            if (!Protected)
            {
                Lives -= damage;
                CallLifeEvents();
                Protected = true;
                if (OnPlayerProtected != null) OnPlayerProtected();
            }
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

            if (Lives <= 0 && OnPlayerDie != null)
                OnPlayerDie();
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

        public void FinishProtection()
        {
            Protected = false;
        }
    }
}
