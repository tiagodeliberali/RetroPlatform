using System;
using System.Collections.Generic;
using Assets.Scripts.Battle;
using RetroPlatform.Battle.Enemies;

namespace RetroPlatform.Battle
{
    public class BattleCore
    {
        public HashSet<Enemy> Enemies { get; set; }
        public BaseAttack CurrentAttack { get; private set; }
        public HashSet<Enemy> EnemiesUderAttack { get; private set; }

        public event Action OnReadyToAttack;
        public event Action<Enemy> OnEnemySelected;

        private IEnvironmentData environmentData;
        private BattleDefinition battleDefinition;
        
        public BattleCore(BattleDefinition battleDefinition, IEnvironmentData environmentData)
        {
            this.battleDefinition = battleDefinition;
            this.environmentData = environmentData;
        }

        public void LoadEnemies()
        {
            int enemyCount = environmentData.GetRandom(battleDefinition.Info.MinEnemies, battleDefinition.Info.MaxEnemies);

            Enemies = new HashSet<Enemy>();
            for (int i = 0; i < enemyCount; i++)
            {
                Enemy enemy = BuildEnemy(i);
                
                enemy.OnSelectToBeAttacked += IncludeToBeAttacked;
                enemy.OnDie += RemoveEnemy;
                enemy.OnRunAway += RemoveEnemy;

                Enemies.Add(enemy);
            }
        }

        private void RemoveEnemy(Enemy enemy)
        {
            Enemies.Remove(enemy);
        }

        private void IncludeToBeAttacked(Enemy enemy)
        {
            if (CurrentAttack == null) return;

            if (EnemiesUderAttack == null) EnemiesUderAttack = new HashSet<Enemy>();

            if (EnemiesUderAttack.Count < CurrentAttack.Info.Range)
            {
                EnemiesUderAttack.Add(enemy);
                if (OnEnemySelected != null) OnEnemySelected(enemy);
            }

            CurrentAttack.Locked = true;

            if ((EnemiesUderAttack.Count == CurrentAttack.Info.Range || EnemiesUderAttack.Count == Enemies.Count)
                && OnReadyToAttack != null)
                OnReadyToAttack();
        }

        private Enemy BuildEnemy(int createdQuantity)
        {
            if (createdQuantity == 0)
                return battleDefinition.EnemyBuilder.BuildWeakEnemy();

            if (createdQuantity == 4)
                return battleDefinition.EnemyBuilder.BuildWeakEnemy();

            if (createdQuantity == 5)
                return battleDefinition.EnemyBuilder.BuildStrongEnemy();

            return battleDefinition.EnemyBuilder.BuildNormalEnemy();
        }

        public void ChooseAttack(BaseAttack attack)
        {
            if (CurrentAttack == null || !CurrentAttack.Locked)
                CurrentAttack = attack;
        }

        public void AttackEnemies()
        {
            foreach (var enemy in EnemiesUderAttack)
            {
                enemy.GetDamage(CurrentAttack.Info.Damage);
            }

            EnemiesUderAttack.Clear();
            CurrentAttack = null;
        }

        public void AttackPlayer(PlayerCore player)
        {
            int damage = 0;

            foreach (var enemy in Enemies)
            {
                damage += environmentData.RandomBool() ? enemy.Attack : 0;
            }
            player.GetDamage(damage);
        }
    }
}
