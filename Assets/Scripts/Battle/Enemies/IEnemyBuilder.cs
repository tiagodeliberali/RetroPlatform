namespace RetroPlatform.Battle.Enemies
{
    public interface IEnemyBuilder
    {
        EnemyCore BuildNormalEnemy();
        EnemyCore BuildStrongEnemy();
        EnemyCore BuildWeakEnemy();
    }
}