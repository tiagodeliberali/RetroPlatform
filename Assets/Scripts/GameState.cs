using System;
using System.Collections.Generic;
using Assets.Scripts.Battle;
using RetroPlatform.Battle;
using UnityEngine;

namespace RetroPlatform
{
    public static class GameState
    {
        public static int Lives = -1;
        public static int Coins;
        public static List<AttackName> Attacks;

        public static string LastSceneName;
        public static Dictionary<string, Vector3> LastScenePositions = new Dictionary<string, Vector3>();
        public static Dictionary<GameFact, object> GameFacts = new Dictionary<GameFact, object>();
        public static Dictionary<BattleName, BattleResult> BattleResults = new Dictionary<BattleName, BattleResult>();

        public static BattleName BattleName { get; set; }

        public static bool GetGameFactBoolean(GameFact factName)
        {
            object fact = GameState.GameFacts.ReturnIfExists(factName);

            return fact == null ? false : (bool)fact;
        }

        public static void SetGameFact(GameFact factName, object fact)
        {
            GameFacts.SetValue(factName, fact);
        }

        public static Vector3 GetLastScenePosition(string sceneName)
        {
            return LastScenePositions.ReturnIfExists(sceneName);
        }

        public static void SetLastScenePosition(string sceneName, Vector3 position)
        {
            LastScenePositions.SetValue(sceneName, position);
        }

        public static BattleResult GetBattleResult(BattleName battle)
        {
            return BattleResults.ReturnIfExists(battle);
        }

        public static void SetBattleResult(BattleName battle, BattleResult result)
        {
            BattleResults.SetValue(battle, result);
        }

        internal static PlayerCore LoadPlayer()
        {
            PlayerCore playerCore = new PlayerCore(new UnityEnvironmentData());

            if (Lives == -1) Lives = playerCore.MaxLives;
            if (Attacks == null) LoadAttacks();

            playerCore.AddLives(Lives);
            playerCore.AddCoins(Coins);
            playerCore.Attacks.Clear();
            playerCore.Attacks.AddRange(Attacks);

            return playerCore;
        }

        private static void LoadAttacks()
        {
            GameState.Attacks = new List<AttackName>()
            {
                AttackName.Sword,
                AttackName.Magic
            };
        }

        public static void UpdatePlayerData(PlayerCore core)
        {
            Lives = core.Lives;
            Coins = core.Coins;
            Attacks = core.Attacks;
        }

        public static void SetLastScene(string sceneName, Vector3 position)
        {
            LastSceneName = sceneName;
            SetLastScenePosition(sceneName, position);
        }
    }
}
