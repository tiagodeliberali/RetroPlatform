using System.Collections.Generic;
using RetroPlatform.Battle;
using UnityEngine;

namespace RetroPlatform
{
    public static class GameState
    {
        public static int Lives = -1;
        public static int Coins;

        public static string LastSceneName;
        public static Dictionary<string, Vector3> LastScenePositions = new Dictionary<string, Vector3>();
        public static Dictionary<string, object> GameFacts = new Dictionary<string, object>();

        public static Sprite BattleCollectable;
        public static BattleResult BattleResult;

        public static bool GetGameFactBoolean(string sceneName, string factName)
        {
            string factId = sceneName + "." + factName;
            object fact = GameState.GameFacts.ReturnIfExists(factId);

            return fact == null ? false : (bool)fact;
        }

        public static void SetGameFact(string sceneName, string factName, object fact)
        {
            string factId = sceneName + "." + factName;
            GameFacts.SetValue(factId, fact);
        }

        public static Vector3 GetLastScenePosition(string sceneName)
        {
            return LastScenePositions.ReturnIfExists(sceneName);
        }

        public static void SetLastScenePosition(string sceneName, Vector3 position)
        {
            LastScenePositions.SetValue(sceneName, position);
        }

        public static void UpdatePlayerData(PlayerCore core)
        {
            Lives = core.Lives;
            Coins = core.Coins;
        }

        public static void SetLastScene(string sceneName, Vector3 position)
        {
            LastSceneName = sceneName;
            SetLastScenePosition(sceneName, position);
        }
    }
}
