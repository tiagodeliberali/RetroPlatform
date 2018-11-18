using RetroPlatform.Editor;
using UnityEditor;
using UnityEngine;

namespace RetroPlatform.Battle
{
    public class AttackDefinitionAssetCreator : MonoBehaviour
    {
        [MenuItem("Assets/Create/AttackDefinition")]
        public static void CreateAsset()
        {
            CustomAssetUtility.CreateAsset<AttackDefinitionArray>();
        }
    }
}
