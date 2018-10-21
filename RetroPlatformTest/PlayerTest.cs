using RetroPlatform;
using UnityEngine;
using Xunit;

namespace RetroPlatformTest
{
    public class PlayerTest
    {
        [Fact]
        public void GetMovementShouldKeepStopedIfNoMovement()
        {
            Player player = new Player(new TestEnvironmentData());

            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0, movement.x);
            Assert.Equal(0, movement.y);
        }

        [Fact]
        public void GetMovementShouldPointUpOnJump()
        {
            Player player = new Player(new TestEnvironmentData());

            player.Jump();
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0, movement.x);
            Assert.Equal(player.jumpSpeed, movement.y);
        }

        [Fact]
        public void GetMovementShouldConsiderMovementVectorOnSecondCallAfterJump()
        {
            Player player = new Player(new TestEnvironmentData());

            player.Jump();
            player.GetMovement(Vector2.zero);
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0, movement.x);
            Assert.Equal(0, movement.y);
        }

        [Fact]
        public void GetMovementShouldConsiderMove()
        {
            Player player = new Player(new TestEnvironmentData());

            player.Move(1f);
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(4f, movement.x);
            Assert.Equal(0, movement.y);
        }

        [Fact]
        public void GetMovementShouldConsiderMoveAndJump()
        {
            Player player = new Player(new TestEnvironmentData());

            player.Jump();
            player.Move(1f);
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(4f, movement.x);
            Assert.Equal(player.jumpSpeed, movement.y);
        }
    }
}
