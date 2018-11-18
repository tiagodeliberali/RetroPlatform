using System;
using UnityEngine;

namespace RetroPlatform.Battle
{
    [Serializable]
    public class BattleDefinitionInfo
    {
        public BattleName Name;
        public Sprite Collectable;
        public GameObject EnemyPrefab;
        public Sprite Background;
        public Sprite EnemyPoster;
        public float EnemyScale;
        public int MinEnemies;
        public int MaxEnemies;
    }
}
