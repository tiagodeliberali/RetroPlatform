using RetroPlatform.Battle;
using RetroPlatform.Conversation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroPlatform.Levels
{
    public class EntryLevelController : MonoBehaviour
    {
        const string FIRST_TALK = "FirstTalk";
        const string LEFT_SCENE = "BossLeftScene";
        
        public UIController uiController;
        public BossController bossController;
        public SpriteRenderer map;
        public RandomBattle battleZone;

        bool LeftTheScene
        {
            get
            {
                return GameState.GetGameFactBoolean(SceneManager.GetActiveScene().name, LEFT_SCENE);
            }
            set
            {
                GameState.SetGameFact(SceneManager.GetActiveScene().name, LEFT_SCENE, value);
            }
        }
        bool HadFirstTalkFact
        {
            get
            {
                return GameState.GetGameFactBoolean(SceneManager.GetActiveScene().name, FIRST_TALK);
            }
            set
            {
                GameState.SetGameFact(SceneManager.GetActiveScene().name, FIRST_TALK, value);
            }
        }

        ConversationComponent conversationComponent;

        void Awake()
        {
            conversationComponent = (ConversationComponent)GetComponent(typeof(ConversationComponent));
            
            uiController.OnFinishConversation += UiController_OnFinishConversation;
            bossController.OnTouchPlayer += BossController_OnTouchPlayer;

            if (LeftTheScene)
            {
                bossController.gameObject.SetActive(false);
            }
            if (GameState.BattleResult == Battle.BattleResult.Win)
            {
                map.gameObject.SetActive(false);
                battleZone.DisableZone();
            }
        }

        private void BossController_OnTouchPlayer()
        {
            if (GameState.BattleResult == Battle.BattleResult.Win)
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
            if (GameState.BattleResult == Battle.BattleResult.Win)
            {
                LeftTheScene = true;
                bossController.RunAway();
            }
        }
    }
}
