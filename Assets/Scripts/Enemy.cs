using UnityEngine;

namespace RetroPlatform
{
    public class Enemy : ScriptableObject
    {
        public int health;

        public EnemyClass enemyClass;
    }

    public enum EnemyClass
    {
        Dragon,
        Blob,
        NastyPeiceOfWork
    }
}
