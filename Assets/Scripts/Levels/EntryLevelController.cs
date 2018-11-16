using RetroPlatform.Battle;
using RetroPlatform.Conversation;
using UnityEngine;

namespace RetroPlatform.Levels
{
    public class EntryLevelController : MonoBehaviour
    {
        public UIController uiController;
        public BossController bossController;
        public SpriteRenderer map;
        public RandomBattle battleZone;
        public GameObject bossProtection;

        public static bool LevelConcluded
        {
            get
            {
                return GameState.GetGameFactBoolean(GameFact.EntryLevelConcluded);
            }
            set
            {
                GameState.SetGameFact(GameFact.EntryLevelConcluded, value);
            }
        }
        bool HadFirstTalkFact
        {
            get
            {
                return GameState.GetGameFactBoolean(GameFact.EntryLevelFirstTalk);
            }
            set
            {
                GameState.SetGameFact(GameFact.EntryLevelFirstTalk, value);
            }
        }
        BattleResult BirdsBattleResult
        {
            get
            {
                return GameState.GetBattleResult(BattleName.EntryLevelZombieBirds);
            }
        }

        ConversationComponent conversationComponent;

        void Awake()
        {
            conversationComponent = (ConversationComponent)GetComponent(typeof(ConversationComponent));
            
            uiController.OnFinishConversation += UiController_OnFinishConversation;
            bossController.OnTouchPlayer += BossController_OnTouchPlayer;

            if (LevelConcluded)
            {
                bossController.gameObject.SetActive(false);
                bossProtection.SetActive(false);
            }
            if (BirdsBattleResult == BattleResult.Win)
            {
                map.gameObject.SetActive(false);
                battleZone.DisableZone();
            }
        }

        private void BossController_OnTouchPlayer()
        {
            if (BirdsBattleResult == BattleResult.Win)
            {
                uiController.StartConversation(conversationComponent.Conversations[2]);
            }
            else if (!HadFirstTalkFact)
            {
                uiController.StartConversation(conversationComponent.Conversations[0]);
                HadFirstTalkFact = true;
            }
            else
            {
                uiController.StartConversation(conversationComponent.Conversations[1]);
            }
        }

        void UiController_OnFinishConversation()
        {
            if (BirdsBattleResult == BattleResult.Win)
            {
                bossProtection.SetActive(false);
                LevelConcluded = true;
                bossController.RunAway();
            }
        }
    }
}
