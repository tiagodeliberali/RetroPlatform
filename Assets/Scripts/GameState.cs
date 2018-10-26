using System.Collections.Generic;
using UnityEngine;

namespace RetroPlatform
{
    public static class GameState
    {
        public static int lives = 3;
        public static int coins;

        public static string LastSceneName;
        public static Dictionary<string, Vector3> LastScenePositions = new Dictionary<string, Vector3>();
        public static Dictionary<string, object> GameFacts = new Dictionary<string, object>();

        public static object GetGameFact(string sceneName, string factName)
        {
            string factId = sceneName + "." + factName;
            return GameState.GameFacts.ReturnIfExists(factId);
        }

        public static void SetGameFact(string sceneName, string factName, object fact)
        {
            string factId = sceneName + "." + factName;
            GameState.GameFacts.SetValue(factId, fact);
        }

        public static Vector3 GetLastScenePosition(string sceneName)
        {
            return GameState.LastScenePositions.ReturnIfExists(sceneName);
        }

        public static void SetLastScenePosition(string sceneName, Vector3 position)
        {
            GameState.LastScenePositions.SetValue(sceneName, position);
        }

        public static void UpdatePlayerData(PlayerCore core)
        {
            lives = core.Lives;
            coins = core.Coins;
        }

        public static void SetLastScene(string sceneName, Vector3 position)
        {
            LastSceneName = sceneName;
            SetLastScenePosition(sceneName, position);
        }
    }
}
