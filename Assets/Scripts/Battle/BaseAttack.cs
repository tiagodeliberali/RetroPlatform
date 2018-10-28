namespace Assets.Scripts.Battle
{
    public abstract class BaseAttack
    {
        public int HitAmount { get; private set; }
        public int EnemiesRange { get; private set; }

        public BaseAttack(int hitAmount, int enemiesRange)
        {
            HitAmount = hitAmount;
            EnemiesRange = enemiesRange;
        }

    }
}
