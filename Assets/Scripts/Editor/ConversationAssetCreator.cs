using RetroPlatform.Editor;
using UnityEditor;
using UnityEngine;

namespace RetroPlatform.Conversation
{
    public class ConversationAssetCreator : MonoBehaviour
    {
        [MenuItem("Assets/Create/Conversation")]
        public static void CreateAsset()
        {
            CustomAssetUtility.CreateAsset<ConversationArray>();
        }
    }
}
