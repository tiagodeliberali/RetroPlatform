using RetroPlatform.Battle;
using RetroPlatform.Conversation;
using UnityEngine;

namespace RetroPlatform.Levels
{
    public class EntryLevelController : MonoBehaviour
    {
        public static string ENTRY_LEVEL = "EntryLevel";
        const string FIRST_TALK = "FirstTalk";
        const string LEFT_SCENE = "BossLeftScene";
        
        public UIController uiController;
        public BossController bossController;
        public SpriteRenderer map;
        public RandomBattle battleZone;
        public GameObject bossProtection;

        public static bool LevelConcluded
        {
            get
            {
                return GameState.GetGameFactBoolean(ENTRY_LEVEL, LEFT_SCENE);
            }
            set
            {
                GameState.SetGameFact(ENTRY_LEVEL, LEFT_SCENE, value);
            }
        }
        bool HadFirstTalkFact
        {
            get
            {
                return GameState.GetGameFactBoolean(ENTRY_LEVEL, FIRST_TALK);
            }
            set
            {
                GameState.SetGameFact(ENTRY_LEVEL, FIRST_TALK, value);
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
            }
            if (GameState.BattleResult == BattleResult.Win)
            {
                map.gameObject.SetActive(false);
                battleZone.DisableZone();
            }
        }

        private void BossController_OnTouchPlayer()
        {
            if (GameState.BattleResult == BattleResult.Win)
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
            if (GameState.BattleResult == BattleResult.Win)
            {
                bossProtection.SetActive(false);
                LevelConcluded = true;
                bossController.RunAway();
            }
        }
    }
}
