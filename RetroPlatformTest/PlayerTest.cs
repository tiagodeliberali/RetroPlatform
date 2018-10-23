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
            Assert.Equal(Player.JUMP_SPEED, movement.y);
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
        public void GetMovementShouldPointUpOnJumpOnlyTwoTimesBeforeHitFloor()
        {
            Player player = CreateUser();

            player.Jump();
            var movement1 = player.GetMovement(Vector2.zero);
            player.Jump();
            var movement2 = player.GetMovement(Vector2.zero);
            player.Jump();
            var movement3 = player.GetMovement(Vector2.zero);

            Assert.Equal(Player.JUMP_SPEED, movement1.y);
            Assert.Equal(Player.JUMP_SPEED, movement2.y);
            Assert.Equal(0f, movement3.y);
        }

        [Fact]
        public void GetMovementShouldPointUpOnJumpAgainAfterHitFloor()
        {
            Player player = CreateUser();

            player.Jump();
            var movement1 = player.GetMovement(Vector2.zero);
            player.Jump();
            var movement2 = player.GetMovement(Vector2.zero);

            player.HitFloor();
            player.Jump();
            var movement3 = player.GetMovement(Vector2.zero);

            Assert.Equal(Player.JUMP_SPEED, movement1.y);
            Assert.Equal(Player.JUMP_SPEED, movement2.y);
            Assert.Equal(Player.JUMP_SPEED, movement3.y);
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
            Assert.Equal(Player.JUMP_SPEED, movement.y);
        }

        [Fact]
        public void GetDirectionShouldHaveDefaultValue()
        {
            Player player = CreateUser();

            var direction = player.Direction;

            Assert.Equal(Direction.Rigth, direction);
        }

        [Fact]
        public void GetDirectionShouldConsiderLastMovingLeft()
        {
            Player player = CreateUser();

            player.Move(1f);
            player.Move(-1f);
            var direction = player.Direction;

            Assert.Equal(Direction.Left, direction);
        }

        [Fact]
        public void GetDirectionShouldConsiderLastMovingRigth()
        {
            Player player = CreateUser();

            player.Move(-1f);
            player.Move(1f);
            var direction = player.Direction;

            Assert.Equal(Direction.Rigth, direction);
        }

        [Fact]
        public void GetDirectionShouldConsiderLastMovingWhenMoveIsZero()
        {
            Player player = CreateUser();

            player.Move(-1f);
            player.Move(0f);
            var direction = player.Direction;

            Assert.Equal(Direction.Left, direction);
        }

        [Fact]
        public void IsRunningShouldBeFalseWhenLastMoveIsZero()
        {
            Player player = CreateUser();

            player.Move(-1f);
            player.Move(0f);
            var isRunning = player.IsRunning;

            Assert.False(isRunning);
        }

        [Fact]
        public void IsRunningShouldBeTrueWhenLastMoveIsNotZero()
        {
            Player player = CreateUser();

            player.Move(0f);
            player.Move(-1f);
            var isRunning = player.IsRunning;

            Assert.True(isRunning);
        }

        [Fact]
        public void GetLivesShouldConsiderTotalAmount()
        {
            Player player = CreateUser();

            player.AddLives(2);
            player.AddLives(1);
            var totalLives = player.Lives;

            Assert.Equal(3, totalLives);
        }

        [Fact]
        public void GetLivesShouldConsiderDamages()
        {
            Player player = CreateUser();

            player.AddLives(4);
            player.GetDamage(2);
            var totalLives = player.Lives;

            Assert.Equal(2, totalLives);
        }

        [Fact]
        public void OnLivesChangedShouldConsiderTotalAmount()
        {
            int totalLives = 0;
            Player player = CreateUser();
            player.OnLivesChanged += delegate ()
            {
                totalLives = player.Lives;
            };

            player.AddLives(5);

            Assert.Equal(5, totalLives);
        }

        [Fact]
        public void OnLivesChangedShouldConsiderDamages()
        {
            int totalLives = 0;
            Player player = CreateUser();
            player.OnLivesChanged += delegate ()
            {
                totalLives = player.Lives;
            };

            player.AddLives(5);
            player.GetDamage(2);

            Assert.Equal(3, totalLives);
        }

        [Fact]
        public void OnLivesFinishedShouldExecuteOnZeroLife()
        {
            bool livesFinished = false;
            Player player = CreateUser();
            player.OnLivesFinished += delegate ()
            {
                livesFinished = true;
            };

            player.AddLives(5);
            player.GetDamage(2);
            player.GetDamage(3);

            Assert.True(livesFinished);
        }

        [Fact]
        public void OnLivesFinishedShouldExecuteOnLowerThanZeroLife()
        {
            bool livesFinished = false;
            Player player = CreateUser();
            player.OnLivesFinished += delegate ()
            {
                livesFinished = true;
            };

            player.AddLives(5);
            player.GetDamage(6);

            Assert.True(livesFinished);
        }

        [Fact]
        public void OnCoinsChangedShouldConsiderTotalAmount()
        {
            int totalCoins = 0;
            Player player = CreateUser();
            player.OnCoinsChanged += delegate ()
            {
                totalCoins = player.Coins;
            };

            player.AddCoins(1);
            player.AddCoins(1);
            player.AddCoins(1);

            Assert.Equal(3, totalCoins);
        }

        [Fact]
        public void ShouldNotMoveAfterStartConversation()
        {
            Player player = CreateUser();

            player.Jump();
            player.Move(1f);
            player.StartConversation();
            var movement = player.GetMovement(new Vector2(2f, 3f));

            Assert.Equal(0f, movement.x);
            Assert.Equal(3f, movement.y); // should keep falling down
        }

        [Fact]
        public void ShouldMoveAfterFinishConversation()
        {
            Player player = CreateUser();

            player.Jump();
            player.Move(1f);
            player.StartConversation();
            player.FinishConversation();
            var movement = player.GetMovement(new Vector2(2f, 3f));

            Assert.Equal(DEFAULT_MOVEMENT_VALUE, movement.x);
            Assert.Equal(3f, movement.y);
        }

        [Fact]
        public void ShouldNotSetIsRunningStartConversation()
        {
            Player player = CreateUser();

            player.Jump();
            player.Move(1f);
            player.StartConversation();
            
            Assert.False(player.IsRunning);
            Assert.False(player.IsJumping);
        }

        [Fact]
        public void ShouldNotChangeDirectionAfterStartConversation()
        {
            Player player = CreateUser();

            player.Move(-1f);
            player.StartConversation();
            player.Move(11f);
            var direction = player.Direction;

            Assert.Equal(Direction.Left, direction);
        }

        [Fact]
        public void ShouldZeroJumpsAfterFinishConversation()
        {
            Player player = CreateUser();

            player.Jump();
            player.Jump();
            player.StartConversation();
            player.FinishConversation();
            player.Jump();
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0f, movement.x);
            Assert.Equal(Player.JUMP_SPEED, movement.y);
        }

        [Fact]
        public void ShouldClearJumpsDoneDuringConversation()
        {
            Player player = CreateUser();

            player.StartConversation();
            player.Jump();
            player.FinishConversation();
            
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0f, movement.x);
            Assert.Equal(0f, movement.y);
        }

        private Player CreateUser()
        {
            return new Player(new TestEnvironmentData());
        }
    }
}
