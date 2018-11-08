using UnityEngine;

namespace RetroPlatform
{
    public abstract class Enemy : ScriptableObject
    {
        public int Health;
        public int Attack;

        public string EnemyName { get; protected set; }

        public static Enemy GetByName(string name)
        {
            switch (name)
            {
                case "Ghost":
                    return ScriptableObject.CreateInstance<EnemyGhost>();
                case "ZombieBird":
                default:
                    return ScriptableObject.CreateInstance<EnemyZombieBird>();
            }
        }

        public void GetDamage(int hitAmount)
        {
            Health -= hitAmount;
        }
    }
}
