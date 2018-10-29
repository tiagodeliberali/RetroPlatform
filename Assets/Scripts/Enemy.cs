using UnityEngine;

namespace RetroPlatform
{
    public class Enemy : ScriptableObject
    {
        public int health;
        public int attack;

        public EnemyClass enemyClass;
    }

    public enum EnemyClass
    {
        ZombieBird,
        Blob,
        NastyPeiceOfWork
    }
}
