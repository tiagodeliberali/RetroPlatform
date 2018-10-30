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
                case "ZombieBird":
                default:
                    return ScriptableObject.CreateInstance<EnemyZombieBird>();
            }
        }
    }
}
