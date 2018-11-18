using System;

namespace RetroPlatform.Battle.Enemies
{
    public abstract class EnemyCore
    {
        public int Health { get; set; }
        public int Attack { get; set; }
        public EnemyStrength Strength { get; set; }

        public event Action<EnemyCore> OnSelectToBeAttacked;
        public event Action<EnemyCore> OnRunAway;
        public event Action<EnemyCore> OnDie;

        public EnemyCore()
        {
            Strength = EnemyStrength.Normal;
        }

        public string EnemyName { get; protected set; }

        public void GetDamage(int hitAmount)
        {
            Health -= hitAmount;

            if (Health <= 0 && OnDie != null) OnDie(this);
        }

        public void SelectToBeAttacked()
        {
            if (OnSelectToBeAttacked != null)
                OnSelectToBeAttacked(this);
        }

        public void RunAway()
        {
            if (OnRunAway != null) OnRunAway(this);
        }
    }
}
