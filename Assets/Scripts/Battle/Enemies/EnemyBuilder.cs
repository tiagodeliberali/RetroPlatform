using System;

namespace RetroPlatform.Battle.Enemies
{
    public class EnemyBuilder<T> : IEnemyBuilder
        where T : EnemyCore, new()
    {
        EnemyCore IEnemyBuilder.BuildNormalEnemy()
        {
            return new T();
        }

        EnemyCore IEnemyBuilder.BuildStrongEnemy()
        {
            T enemy = new T();
            enemy.Health = (int)Math.Round(enemy.Health * 1.2f);
            enemy.Attack = (int)Math.Round(enemy.Attack * 1.5f);
            enemy.Strength = EnemyStrength.Strong;

            return enemy;
        }

        EnemyCore IEnemyBuilder.BuildWeakEnemy()
        {
            T enemy = new T();
            enemy.Health = (int)Math.Round(enemy.Health * 0.7f);
            enemy.Attack = (int)Math.Round(enemy.Attack * 0.5f);
            enemy.Strength = EnemyStrength.Weak;

            return enemy;
        }
    }
}
