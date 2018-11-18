using RetroPlatform.Editor;
using UnityEditor;
using UnityEngine;

namespace RetroPlatform.Battle
{
    public class BattleDefinitionAssetCreator : MonoBehaviour
    {
        [MenuItem("Assets/Create/BattleDefinition")]
        public static void CreateAsset()
        {
            CustomAssetUtility.CreateAsset<BattleDefinitionArray>();
        }
    }
}
