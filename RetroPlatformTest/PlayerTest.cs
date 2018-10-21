using RetroPlatform;
using UnityEngine;
using Xunit;

namespace RetroPlatformTest
{
    public class PlayerTest
    {
        const float DEFAULT_MOVEMENT_VALUE = 4f;

        [Fact]
        public void GetMovementShouldKeepStopedIfNoMovement()
        {
            Player player = CreateUser();

            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0, movement.x);
            Assert.Equal(0, movement.y);
        }

        [Fact]
        public void GetMovementShouldPointUpOnJump()
        {
            Player player = CreateUser();

            player.Jump();
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0, movement.x);
            Assert.Equal(player.jumpSpeed, movement.y);
        }

        [Fact]
        public void GetMovementShouldConsiderMovementVectorOnSecondCallAfterJump()
        {
            Player player = CreateUser();

            player.Jump();
            player.GetMovement(Vector2.zero);
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0, movement.x);
            Assert.Equal(0, movement.y);
        }

        [Fact]
        public void GetMovementShouldConsiderMove()
        {
            Player player = CreateUser();

            player.Move(1f);
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(DEFAULT_MOVEMENT_VALUE, movement.x);
            Assert.Equal(0, movement.y);
        }

        [Fact]
        public void GetMovementShouldConsiderMoveAndJump()
        {
            Player player = CreateUser();

            player.Jump();
            player.Move(1f);
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(DEFAULT_MOVEMENT_VALUE, movement.x);
            Assert.Equal(player.jumpSpeed, movement.y);
        }

        [Fact]
        public void GetDirectionShouldHaveDefaultValue()
        {
            Player player = CreateUser();

            var direction = player.GetDirection();

            Assert.Equal(Direction.Rigth, direction);
        }

        [Fact]
        public void GetDirectionShouldConsiderLastMovingLeft()
        {
            Player player = CreateUser();

            player.Move(1f);
            player.Move(-1f);
            var direction = player.GetDirection();

            Assert.Equal(Direction.Left, direction);
        }

        [Fact]
        public void GetDirectionShouldConsiderLastMovingRigth()
        {
            Player player = CreateUser();

            player.Move(-1f);
            player.Move(1f);
            var direction = player.GetDirection();

            Assert.Equal(Direction.Rigth, direction);
        }

        [Fact]
        public void GetDirectionShouldConsiderLastMovingWhenMoveIsZero()
        {
            Player player = CreateUser();

            player.Move(-1f);
            player.Move(0f);
            var direction = player.GetDirection();

            Assert.Equal(Direction.Left, direction);
        }

        [Fact]
        public void IsRunningShouldBeFalseWhenLastMoveIsZero()
        {
            Player player = CreateUser();

            player.Move(-1f);
            player.Move(0f);
            var isRunning = player.IsRunning();

            Assert.False(isRunning);
        }

        [Fact]
        public void IsRunningShouldBeTrueWhenLastMoveIsNotZero()
        {
            Player player = CreateUser();

            player.Move(0f);
            player.Move(-1f);
            var isRunning = player.IsRunning();

            Assert.True(isRunning);
        }

        [Fact]
        public void GetLivesShouldConsiderTotalAmount()
        {
            Player player = CreateUser();

            player.AddLives(2);
            player.AddLives(1);
            var totalLives = player.GetLives();

            Assert.Equal(3, totalLives);
        }

        [Fact]
        public void GetLivesShouldConsiderDamages()
        {
            Player player = CreateUser();

            player.AddLives(4);
            player.GetDamage(2);
            var totalLives = player.GetLives();

            Assert.Equal(2, totalLives);
        }

        [Fact]
        public void OnLivesChangedShouldConsiderTotalAmount()
        {
            int totalLives = 0;
            Player player = CreateUser();
            player.OnLivesChanged += delegate (int lives)
            {
                totalLives = lives;
            };

            player.AddLives(5);

            Assert.Equal(5, totalLives);
        }

        [Fact]
        public void OnLivesChangedShouldConsiderDamages()
        {
            int totalLives = 0;
            Player player = CreateUser();
            player.OnLivesChanged += delegate (int lives)
            {
                totalLives = lives;
            };

            player.AddLives(5);
            player.GetDamage(2);

            Assert.Equal(3, totalLives);
        }

        private Player CreateUser()
        {
            return new Player(new TestEnvironmentData());
        }
    }
}
