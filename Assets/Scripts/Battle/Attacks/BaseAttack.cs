using System.Linq;
using RetroPlatform.Battle;

namespace Assets.Scripts.Battle
{
    public class BaseAttack
    {
        public AttackName Name { get; private set; }
        public AttackDefinitionInfo Info { get; private set; }
        public bool Locked { get; set; }

        public BaseAttack(AttackName name, AttackDefinitionInfo info)
        {
            Name = name;
            Info = info;
        }

        public static BaseAttack GetAttack(AttackName name, AttackDefinitionInfo[] attackDefinitions)
        {
            AttackDefinitionInfo info = attackDefinitions.First(x => x.Name == name);
            return new BaseAttack(name, info);
        }
    }
}
