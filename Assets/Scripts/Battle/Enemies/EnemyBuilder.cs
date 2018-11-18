using System;

namespace RetroPlatform.Battle.Enemies
{
    public class EnemyBuilder<T> : IEnemyBuilder
        where T : Enemy, new()
    {
        Enemy IEnemyBuilder.BuildNormalEnemy()
        {
            return new T();
        }

        Enemy IEnemyBuilder.BuildStrongEnemy()
        {
            T enemy = new T();
            enemy.Health = (int)Math.Round(enemy.Health * 1.2f);
            enemy.Attack = (int)Math.Round(enemy.Attack * 1.5f);
            enemy.Strength = EnemyStrength.Strong;

            return enemy;
        }

        Enemy IEnemyBuilder.BuildWeakEnemy()
        {
            T enemy = new T();
            enemy.Health = (int)Math.Round(enemy.Health * 0.7f);
            enemy.Attack = (int)Math.Round(enemy.Attack * 0.5f);
            enemy.Strength = EnemyStrength.Weak;

            return enemy;
        }
    }
}
