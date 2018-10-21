using System;
using UnityEngine;

namespace RetroPlatform
{
    public class Player
    {
        private IEnvironmentData environmentData;
        private float movePlayerHorizontal;

        public float speed = 400f;
        public float jumpSpeed = 20;

        public delegate void LivesChanged(int totalLives);
        public event LivesChanged OnLivesChanged;

        bool isJumping;
        bool isRunning;
        int lives;
        Direction currentDirection = Direction.Rigth;

        public Player(IEnvironmentData environmentData)
        {
            this.environmentData = environmentData;
        }

        public void Jump()
        {
            isJumping = true;
        }

        public void Move(float direction)
        {
            movePlayerHorizontal = direction * speed * environmentData.GetDeltaTime();

            isRunning = movePlayerHorizontal != 0;
            if (movePlayerHorizontal < 0) currentDirection = Direction.Left;
            else if (movePlayerHorizontal > 0) currentDirection = Direction.Rigth;
        }

        public Vector2 GetMovement(Vector2 currentMovement)
        {
            var movement = new Vector2(movePlayerHorizontal, isJumping ? jumpSpeed : currentMovement.y);
            isJumping = false;

            return movement;
        }

        public Direction GetDirection()
        {
            return currentDirection;
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public void AddLives(int lives)
        {
            this.lives += lives;
            if (OnLivesChanged != null) OnLivesChanged(lives);
        }

        public object GetLives()
        {
            return lives;
        }

        public void GetDamage(int damage)
        {
            lives -= damage;
            if (OnLivesChanged != null) OnLivesChanged(lives);
        }
    }
}
