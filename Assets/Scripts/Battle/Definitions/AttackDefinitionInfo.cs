using System;
using Assets.Scripts.Battle;
using UnityEngine;

namespace RetroPlatform.Battle
{
    [Serializable]
    public class AttackDefinitionInfo
    {
        public AttackName Name;
        public Sprite Image;
        public int Range;
        public int Damage;
    }
}
