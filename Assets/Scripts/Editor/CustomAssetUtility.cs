using System.IO;
using UnityEditor;
using UnityEngine;

namespace RetroPlatform.Editor
{
    public class CustomAssetUtility : MonoBehaviour
    {
        //Define a menu option in the editor to create the new asset 
        [MenuItem("Assets/Create/PositionManager")]
        public static void CreateAsset<T>() where T: ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (File.Exists(path))
            {
                path = path.Replace("/" + Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).Name + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();

            //Now switch the inspector to our new object 
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
