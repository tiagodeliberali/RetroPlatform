using UnityEngine;
using RetroPlatform.Battle.Enemies;
using System.Linq;

namespace RetroPlatform.Battle
{
    public class BattleDefinition
    {
        public BattleName Name { get; private set; }
        public BattleDefinitionInfo Info { get; private set; }
        public IEnemyBuilder EnemyBuilder { get; internal set; }

        public BattleResult Result
        {
            get
            {
                return GameState.GetBattleResult(Name);
            }
            set
            {
                GameState.SetBattleResult(Name, value);
            }
        }

        public BattleDefinition(BattleName name)
        {
            Name = name;
        }

        public static BattleDefinition GetBattle(BattleName name, BattleDefinitionInfo[] definitionInfos)
        {
            var definition = new BattleDefinition(name);

            switch (name)
            {
                case BattleName.None:
                    throw new UnassignedReferenceException("Invalid BattleName: None");
                case BattleName.EntryLevelZombieBirds:
                    definition.EnemyBuilder = new EnemyBuilder<ZombieBird>();
                    break;
                case BattleName.HalloweenLevelFishGosts:
                    definition.EnemyBuilder = new EnemyBuilder<Ghost>();
                    break;
                default:
                    break;
            }

            definition.Info = definitionInfos.First(x => x.Name == name);


            return definition;
        }
    }
}
