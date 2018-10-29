using UnityEngine;

namespace RetroPlatform
{
    public abstract class Enemy : ScriptableObject
    {
        public int health;
        public int attack;

        public string EnemyName { get; protected set; }

        public static Enemy GetByName(string name)
        {
            switch (name)
            {
                case "ZombieBird":
                default:
                    return ScriptableObject.CreateInstance<EnemyZombieBird>();
            }
        }
    }
}
