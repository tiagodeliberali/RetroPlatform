using UnityEngine;

namespace RetroPlatform
{
    public class Player
    {
        private IEnvironmentData environmentData;
        private float movePlayerHorizontal;

        public float speed = 400f;
        public float jumpSpeed = 20;

        bool isJumping = false;

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
        }

        public Vector2 GetMovement(Vector2 currentMovement)
        {
            var movement = new Vector2(movePlayerHorizontal, isJumping ? jumpSpeed : currentMovement.y);
            isJumping = false;

            return movement;
        }
    }
}
