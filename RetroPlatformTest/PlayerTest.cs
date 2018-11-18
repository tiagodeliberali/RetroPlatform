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
             var player = CreateUser();

            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0, movement.x);
            Assert.Equal(0, movement.y);
        }

        [Fact]
        public void GetMovementShouldPointUpOnJump()
        {
             var player = CreateUser();

            player.Jump();
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0, movement.x);
            Assert.Equal(PlayerCore.JUMP_SPEED, movement.y);
        }

        [Fact]
        public void GetMovementShouldConsiderMovementVectorOnSecondCallAfterJump()
        {
             var player = CreateUser();

            player.Jump();
            player.GetMovement(Vector2.zero);
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0, movement.x);
            Assert.Equal(0, movement.y);
        }

        [Fact]
        public void GetMovementShouldPointUpOnJumpOnlyTwoTimesBeforeHitFloor()
        {
             var player = CreateUser();

            player.Jump();
            var movement1 = player.GetMovement(Vector2.zero);
            player.Jump();
            var movement2 = player.GetMovement(Vector2.zero);
            player.Jump();
            var movement3 = player.GetMovement(Vector2.zero);

            Assert.Equal(PlayerCore.JUMP_SPEED, movement1.y);
            Assert.Equal(PlayerCore.JUMP_SPEED, movement2.y);
            Assert.Equal(0f, movement3.y);
        }

        [Fact]
        public void GetMovementShouldPointUpOnJumpAgainAfterHitFloor()
        {
             var player = CreateUser();

            player.Jump();
            var movement1 = player.GetMovement(Vector2.zero);
            player.Jump();
            var movement2 = player.GetMovement(Vector2.zero);

            player.HitFloor();
            player.Jump();
            var movement3 = player.GetMovement(Vector2.zero);

            Assert.Equal(PlayerCore.JUMP_SPEED, movement1.y);
            Assert.Equal(PlayerCore.JUMP_SPEED, movement2.y);
            Assert.Equal(PlayerCore.JUMP_SPEED, movement3.y);
        }

        [Fact]
        public void GetMovementShouldConsiderMove()
        {
             var player = CreateUser();

            player.Move(1f);
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(DEFAULT_MOVEMENT_VALUE, movement.x);
            Assert.Equal(0, movement.y);
        }

        [Fact]
        public void GetMovementShouldConsiderMoveAndJump()
        {
             var player = CreateUser();

            player.Jump();
            player.Move(1f);
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(DEFAULT_MOVEMENT_VALUE, movement.x);
            Assert.Equal(PlayerCore.JUMP_SPEED, movement.y);
        }

        [Fact]
        public void GetDirectionShouldHaveDefaultValue()
        {
             var player = CreateUser();

            var direction = player.Direction;

            Assert.Equal(Direction.Rigth, direction);
        }

        [Fact]
        public void GetDirectionShouldConsiderLastMovingLeft()
        {
             var player = CreateUser();

            player.Move(1f);
            player.Move(-1f);
            var direction = player.Direction;

            Assert.Equal(Direction.Left, direction);
        }

        [Fact]
        public void GetDirectionShouldConsiderLastMovingRigth()
        {
             var player = CreateUser();

            player.Move(-1f);
            player.Move(1f);
            var direction = player.Direction;

            Assert.Equal(Direction.Rigth, direction);
        }

        [Fact]
        public void GetDirectionShouldConsiderLastMovingWhenMoveIsZero()
        {
             var player = CreateUser();

            player.Move(-1f);
            player.Move(0f);
            var direction = player.Direction;

            Assert.Equal(Direction.Left, direction);
        }

        [Fact]
        public void IsRunningShouldBeFalseWhenLastMoveIsZero()
        {
             var player = CreateUser();

            player.Move(-1f);
            player.Move(0f);
            var isRunning = player.IsRunning;

            Assert.False(isRunning);
        }

        [Fact]
        public void IsRunningShouldBeTrueWhenLastMoveIsNotZero()
        {
             var player = CreateUser();

            player.Move(0f);
            player.Move(-1f);
            var isRunning = player.IsRunning;

            Assert.True(isRunning);
        }

        [Fact]
        public void GetLivesShouldConsiderTotalAmount()
        {
             var player = CreateUser();

            player.AddLives(2);
            player.AddLives(1);
            var totalLives = player.Lives;

            Assert.Equal(3, totalLives);
        }

        [Fact]
        public void GetLivesShouldConsiderDamages()
        {
             var player = CreateUser();

            player.AddLives(4);
            player.GetDamage(2);
            var totalLives = player.Lives;

            Assert.Equal(2, totalLives);
        }

        [Fact]
        public void OnLivesChangedShouldConsiderTotalAmount()
        {
            int totalLives = 0;
             var player = CreateUser();
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
             var player = CreateUser();
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
             var player = CreateUser();
            player.OnPlayerDie += delegate ()
            {
                livesFinished = true;
            };

            player.AddLives(5);
            player.GetDamage(5);

            Assert.True(livesFinished);
        }

        [Fact]
        public void OnLivesFinishedShouldExecuteOnLowerThanZeroLife()
        {
            bool livesFinished = false;
             var player = CreateUser();
            player.OnPlayerDie += delegate ()
            {
                livesFinished = true;
            };

            player.AddLives(5);
            player.GetDamage(6);

            Assert.True(livesFinished);
        }

        [Fact]
        public void OnGetDamageShouldKeepProtectedFromNewDamage()
        {
            var player = CreateUser();

            player.AddLives(5);
            player.GetDamage(2);
            player.GetDamage(3);

            Assert.Equal(3, player.Lives);
            Assert.True(player.Protected);
        }

        [Fact]
        public void OnGetZeroDamageShouldNotProtectedFromNewDamage()
        {
            var player = CreateUser();

            player.AddLives(5);
            player.GetDamage(0);
            player.GetDamage(3);

            Assert.Equal(2, player.Lives);
            Assert.True(player.Protected);
        }

        [Fact]
        public void OnDamageWithoutProtectionShouldNotProtectedFromNewDamage()
        {
            var player = CreateUser();

            player.AddLives(5);
            player.GetDamageWithoutProtection(3);
            
            Assert.Equal(2, player.Lives);
            Assert.False(player.Protected);
        }

        [Fact]
        public void OnFInishProtectionShouldGetDamaged()
        {
            var player = CreateUser();

            player.AddLives(5);
            player.GetDamage(2);
            player.FinishProtection();
            player.GetDamage(2);
            player.FinishProtection();

            Assert.Equal(1, player.Lives);
            Assert.False(player.Protected);
        }

        [Fact]
        public void OnGetDamageShouldExecuteOnProtected()
        {
            bool playerProtected = false;
            var player = CreateUser();
            player.OnPlayerProtected += delegate ()
            {
                playerProtected = true;
            };

            player.AddLives(5);
            player.GetDamage(3);

            Assert.True(playerProtected);
        }

        [Fact]
        public void OnGetDamageWithoutProtectionShouldNotExecuteOnProtected()
        {
            bool playerProtected = false;
            var player = CreateUser();
            player.OnPlayerProtected += delegate ()
            {
                playerProtected = true;
            };

            player.AddLives(5);
            player.GetDamageWithoutProtection(3);

            Assert.False(playerProtected);
        }

        [Fact]
        public void OnCoinsChangedShouldConsiderTotalAmount()
        {
            int totalCoins = 0;
             var player = CreateUser();
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
             var player = CreateUser();

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
             var player = CreateUser();

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
             var player = CreateUser();

            player.Jump();
            player.Move(1f);
            player.StartConversation();
            
            Assert.False(player.IsRunning);
            Assert.False(player.IsJumping);
        }

        [Fact]
        public void ShouldNotChangeDirectionAfterStartConversation()
        {
             var player = CreateUser();

            player.Move(-1f);
            player.StartConversation();
            player.Move(11f);
            var direction = player.Direction;

            Assert.Equal(Direction.Left, direction);
        }

        [Fact]
        public void ShouldZeroJumpsAfterFinishConversation()
        {
             var player = CreateUser();

            player.Jump();
            player.Jump();
            player.StartConversation();
            player.FinishConversation();
            player.Jump();
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0f, movement.x);
            Assert.Equal(PlayerCore.JUMP_SPEED, movement.y);
        }

        [Fact]
        public void ShouldClearJumpsDoneDuringConversation()
        {
            PlayerCore player = CreateUser();

            player.StartConversation();
            player.Jump();
            player.FinishConversation();
            
            var movement = player.GetMovement(Vector2.zero);

            Assert.Equal(0f, movement.x);
            Assert.Equal(0f, movement.y);
        }

        private PlayerCore CreateUser()
        {
            var player = new PlayerCore(new TestEnvironmentData());
            return player;
        }
    }
}
