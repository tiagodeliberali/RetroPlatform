using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Battle;
using RetroPlatform;
using RetroPlatform.Battle;
using RetroPlatform.Battle.Enemies;
using Xunit;

namespace RetroPlatformTest
{
    public class BattleCoreTest
    {
        TestEnvironmentData environmentData;

        [Fact]
        public void ShouldBuildEnemiesArray()
        {
            BattleCore battle = BuildBattle(3);

            battle.LoadEnemies();

            Assert.Equal(3, battle.Enemies.Count);
        }

        [Fact]
        public void ShouldHaveOneWeakAndNoStrongUntil4Enemies()
        {
            BattleCore battle = BuildBattle(4);

            battle.LoadEnemies();

            CountByStrength(battle, 1, EnemyStrength.Weak);
            CountByStrength(battle, 0, EnemyStrength.Strong);
        }

        [Fact]
        public void ShouldHaveTwoWeakAndNoStrongWhen5Enemies()
        {
            BattleCore battle = BuildBattle(5);

            battle.LoadEnemies();

            CountByStrength(battle, 2, EnemyStrength.Weak);
            CountByStrength(battle, 0, EnemyStrength.Strong);
        }

        [Fact]
        public void ShouldHaveTwoWeakAndOneStrongForMoreThan5Enemies()
        {
            BattleCore battle = BuildBattle(6);

            battle.LoadEnemies();

            CountByStrength(battle, 2, EnemyStrength.Weak);
            CountByStrength(battle, 1, EnemyStrength.Strong);
        }

        [Fact]
        public void AfterSelectingEnemiesShouldCallOnReadyToAttack()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();
            bool readyToAttack = false;

            BattleCore battle = BuildBattle(6);
            battle.LoadEnemies();

            battle.OnReadyToAttack += () => readyToAttack = true;

            battle.ChooseAttack(attackTwoEnemiesRange);
            battle.Enemies.First().SelectToBeAttacked();
            battle.Enemies.Last().SelectToBeAttacked();

            Assert.True(readyToAttack);
        }

        [Fact]
        public void AfterSelectingAllEnemiesShouldCallOnReadyToAttack()
        {
            BaseAttack attackThreeEnemiesRange = GetThreeEnemiesRangeAttack();
            bool readyToAttack = false;

            BattleCore battle = BuildBattle(2);
            battle.LoadEnemies();

            battle.OnReadyToAttack += () => readyToAttack = true;

            battle.ChooseAttack(attackThreeEnemiesRange);
            battle.Enemies.First().SelectToBeAttacked();
            battle.Enemies.Last().SelectToBeAttacked();

            Assert.True(readyToAttack);
        }

        [Fact]
        public void ShouldNotAllowSelectMoreEnemiesThanAttackRange()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();

            BattleCore battle = BuildBattle(4);
            battle.LoadEnemies();
            EnemyCore enemy1 = battle.Enemies.ElementAt(0);
            EnemyCore enemy2 = battle.Enemies.ElementAt(1);
            EnemyCore enemy3 = battle.Enemies.ElementAt(2);

            battle.ChooseAttack(attackTwoEnemiesRange);
            enemy1.SelectToBeAttacked();
            enemy2.SelectToBeAttacked();
            enemy3.SelectToBeAttacked();

            Assert.Equal(2, battle.EnemiesUderAttack.Count);
            Assert.Contains(enemy1, battle.EnemiesUderAttack);
            Assert.Contains(enemy2, battle.EnemiesUderAttack);
        }

        [Fact]
        public void IfnotEnoughEnemiesShouldNotCallOnReadyToAttack()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();
            bool readyToAttack = false;

            BattleCore battle = BuildBattle(6);
            battle.LoadEnemies();
            
            battle.OnReadyToAttack += () => readyToAttack = true;

            battle.ChooseAttack(attackTwoEnemiesRange);
            battle.Enemies.First().SelectToBeAttacked();
            
            Assert.False(readyToAttack);
        }

        [Fact]
        public void AfterSelectEnemyShouldNotAllowChangeAttack()
        {
            BaseAttack attack1 = GetTwoEnemiesRangeAttack();
            BaseAttack attack2 = GetThreeEnemiesRangeAttack();

            BattleCore battle = BuildBattle(6);
            battle.LoadEnemies();

            battle.ChooseAttack(attack1);
            battle.Enemies.First().SelectToBeAttacked();

            battle.ChooseAttack(attack2);

            Assert.True(attack1.Locked);
            Assert.False(attack2.Locked);
            Assert.Equal(attack1, battle.CurrentAttack);
        }

        [Fact]
        public void SelectingEnemiesShouldCallOnEnemySelected()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();

            BattleCore battle = BuildBattle(6);
            battle.LoadEnemies();
            EnemyCore enemy1 = battle.Enemies.First();
            EnemyCore enemySelected = null;

            battle.OnEnemySelected += (EnemyCore enemy) => enemySelected = enemy;

            battle.ChooseAttack(attackTwoEnemiesRange);
            enemy1.SelectToBeAttacked();

            Assert.Equal(enemy1, enemySelected);
        }

        [Fact]
        public void SelectingMoreEnemiesThanRangeShouldNotCallOnEnemySelected()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();

            BattleCore battle = BuildBattle(6);
            battle.LoadEnemies();
            EnemyCore enemy1 = battle.Enemies.ElementAt(0);
            EnemyCore enemy2 = battle.Enemies.ElementAt(1);
            EnemyCore enemy3 = battle.Enemies.ElementAt(2);
            List<EnemyCore> enemiesSelected = new List<EnemyCore>();

            battle.OnEnemySelected += (EnemyCore enemy) => enemiesSelected.Add(enemy);

            battle.ChooseAttack(attackTwoEnemiesRange);
            enemy1.SelectToBeAttacked();
            enemy2.SelectToBeAttacked();
            enemy3.SelectToBeAttacked();

            Assert.Equal(2, enemiesSelected.Count);
            Assert.Contains(enemy1, enemiesSelected);
            Assert.Contains(enemy2, enemiesSelected);
        }

        [Fact]
        public void SelectingEnemiesWithoutAttackShouldNotCallOnEnemySelected()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();
            EnemyCore enemySelected = null;

            BattleCore battle = BuildBattle(6);
            battle.LoadEnemies();
            EnemyCore enemy1 = battle.Enemies.First();

            battle.OnEnemySelected += (EnemyCore enemy) => enemySelected = enemy;

            enemy1.SelectToBeAttacked();

            Assert.Null(enemySelected);
        }

        [Fact]
        public void CallAttackEnemiesShouldHitEnemiesUnderAttack()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();

            BattleCore battle = BuildBattle(4);
            battle.LoadEnemies();
            EnemyCore enemy1 = battle.Enemies.ElementAt(0);
            EnemyCore enemy2 = battle.Enemies.ElementAt(1);
            
            battle.ChooseAttack(attackTwoEnemiesRange);
            enemy1.SelectToBeAttacked();
            enemy2.SelectToBeAttacked();
            battle.AttackEnemies();

            Assert.Equal(-2, enemy1.Health);
            Assert.Equal(-1, enemy2.Health);
        }

        [Fact]
        public void CallAttackEnemiesShouldClearEnemiesUnderAttack()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();

            BattleCore battle = BuildBattle(4);
            battle.LoadEnemies();
            EnemyCore enemy1 = battle.Enemies.ElementAt(0);
            EnemyCore enemy2 = battle.Enemies.ElementAt(1);

            battle.ChooseAttack(attackTwoEnemiesRange);
            enemy1.SelectToBeAttacked();
            enemy2.SelectToBeAttacked();
            battle.AttackEnemies();

            Assert.Empty(battle.EnemiesUderAttack);
        }

        [Fact]
        public void CallAttackEnemiesShouldClearCurrentAttack()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();

            BattleCore battle = BuildBattle(4);
            battle.LoadEnemies();
            EnemyCore enemy1 = battle.Enemies.ElementAt(0);
            EnemyCore enemy2 = battle.Enemies.ElementAt(1);

            battle.ChooseAttack(attackTwoEnemiesRange);
            enemy1.SelectToBeAttacked();
            enemy2.SelectToBeAttacked();
            battle.AttackEnemies();

            Assert.Null(battle.CurrentAttack);
        }

        [Fact]
        public void DeathEnemiesShouldBeRemovedFromEnemiesList()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();

            BattleCore battle = BuildBattle(4);
            battle.LoadEnemies();
            EnemyCore enemy1 = battle.Enemies.ElementAt(0);
            EnemyCore enemy2 = battle.Enemies.ElementAt(1);

            battle.ChooseAttack(attackTwoEnemiesRange);
            enemy1.SelectToBeAttacked();
            enemy2.SelectToBeAttacked();
            battle.AttackEnemies();

            Assert.Equal(2, battle.Enemies.Count);
            Assert.DoesNotContain(enemy1, battle.Enemies);
            Assert.DoesNotContain(enemy2, battle.Enemies);
        }

        [Fact]
        public void EnemiesThatRunAwayShouldBeRemovedFromEnemiesList()
        {
            BaseAttack attackTwoEnemiesRange = GetTwoEnemiesRangeAttack();

            BattleCore battle = BuildBattle(4);
            battle.LoadEnemies();
            EnemyCore enemy1 = battle.Enemies.ElementAt(0);
            
            enemy1.RunAway();
            
            Assert.Equal(3, battle.Enemies.Count);
            Assert.DoesNotContain(enemy1, battle.Enemies);
        }

        [Fact]
        public void ShouldConsiderEnemiesToHitPlayer()
        {
            BattleCore battle = BuildBattle(4);
            battle.LoadEnemies();

            environmentData.RandomBoolResults.Push(true);
            environmentData.RandomBoolResults.Push(false);
            environmentData.RandomBoolResults.Push(true);
            environmentData.RandomBoolResults.Push(false);

            PlayerCore player = new PlayerCore(new TestEnvironmentData());
            player.AddLives(5);

            battle.AttackPlayer(player);

            Assert.Equal(3, player.Lives);
        }

        [Fact]
        public void DuringBattlePlayerShouldNotGetProtected()
        {
            BattleCore battle = BuildBattle(4);
            battle.LoadEnemies();

            environmentData.RandomBoolResults.Push(true);
            environmentData.RandomBoolResults.Push(false);
            environmentData.RandomBoolResults.Push(true);
            environmentData.RandomBoolResults.Push(false);

            PlayerCore player = new PlayerCore(new TestEnvironmentData());
            player.AddLives(5);

            battle.AttackPlayer(player);

            Assert.False(player.Protected);
        }

        private BaseAttack GetTwoEnemiesRangeAttack()
        {
            AttackDefinitionInfo info = new AttackDefinitionInfo()
            {
                Range = 2,
                Damage = 4
            };
            return new BaseAttack(AttackName.Magic, info);
        }

        private BaseAttack GetThreeEnemiesRangeAttack()
        {
            AttackDefinitionInfo info = new AttackDefinitionInfo()
            {
                Range = 3,
                Damage = 2
            };
            return new BaseAttack(AttackName.Bow, info);
        }

        private void CountByStrength(BattleCore battle, int expectedCount, EnemyStrength strength)
        {
            Assert.Equal(expectedCount, battle.Enemies.Where(x => x.Strength == strength).Count());
        }

        private BattleCore BuildBattle(int numberOfEnemies)
        {
            environmentData = new TestEnvironmentData();
            environmentData.RandomResult.Push(numberOfEnemies);

            BattleDefinitionInfo[] definitionInfos = new BattleDefinitionInfo[1];
            definitionInfos[0] = new BattleDefinitionInfo()
            {
                Name = BattleName.EntryLevelZombieBirds,
                MinEnemies = 2,
                MaxEnemies = 5
            };

            BattleDefinition definition = BattleDefinition.GetBattle(BattleName.EntryLevelZombieBirds, definitionInfos);
            BattleCore battle = new BattleCore(definition, environmentData);
            return battle;
        }
    }
}
