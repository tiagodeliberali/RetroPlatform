namespace RetroPlatform.Battle.Enemies
{
    public interface IEnemyBuilder
    {
        Enemy BuildNormalEnemy();
        Enemy BuildStrongEnemy();
        Enemy BuildWeakEnemy();
    }
}