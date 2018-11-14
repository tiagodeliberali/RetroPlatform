using System.Collections;
using RetroPlatform.Battle;
using RetroPlatform.Conversation;
using RetroPlatform.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroPlatform.Levels
{
    public class HalloweenLevelController : MonoBehaviour
    {
        public static string HALLOWEEN_LEVEL = "EntryLevel";

        public UIController uiController;
        public SpriteRenderer hammer;
        public GameObject ghost;
        public GameObject portal;
        public Texture2D FadeTexture;

        const string FIRST_TALK = "FirstTalk";
        const string GET_HAMMER = "GetHammer";
        const string PORTAL_DESTROYED = "PortalDestroyed";

        bool HadFirstTalkFact
        {
            get
            {
                return GameState.GetGameFactBoolean(HALLOWEEN_LEVEL, FIRST_TALK);
            }
            set
            {
                GameState.SetGameFact(HALLOWEEN_LEVEL, FIRST_TALK, value);
            }
        }
        bool GetHammerFact
        {
            get
            {
                return GameState.GetGameFactBoolean(HALLOWEEN_LEVEL, GET_HAMMER);
            }
            set
            {
                GameState.SetGameFact(HALLOWEEN_LEVEL, GET_HAMMER, value);
            }
        }
        public static bool LevelConcluded
        {
            get
            {
                return GameState.GetGameFactBoolean(HALLOWEEN_LEVEL, PORTAL_DESTROYED);
            }
            set
            {
                GameState.SetGameFact(HALLOWEEN_LEVEL, PORTAL_DESTROYED, value);
            }
        }

        ConversationComponent conversationComponent;
        PortalController portalController;
        Animator portalAnimator;
        UIHelper uiHelper = new UIHelper();
        bool fadeOut;

        void Awake()
        {
            conversationComponent = (ConversationComponent)GetComponent(typeof(ConversationComponent));
            portalController = (PortalController)portal.GetComponent(typeof(PortalController));
            portalAnimator = (Animator)portal.GetComponent(typeof(Animator));

            uiController.OnFinishConversation += UiController_OnFinishConversation;
            portalController.OnPlayerTouch += Portal_OnPlayerTouch;

            if (!HadFirstTalkFact)
            {
                GameState.BattleResult = BattleResult.None;
                uiController.StartConversation(conversationComponent.Conversations[0]);
                HadFirstTalkFact = true;
            }
            if (GameState.BattleResult == BattleResult.Win)
            {
                hammer.gameObject.SetActive(false);
                ghost.SetActive(false);
                GetHammerFact = true;
            }
            if (LevelConcluded)
            {
                ghost.SetActive(false);
                portal.SetActive(false);
            }
        }

        private void Portal_OnPlayerTouch()
        {
            if (GetHammerFact)
            {
                uiController.StartConversation(conversationComponent.Conversations[2]);
            }
            else
            {
                uiController.StartConversation(conversationComponent.Conversations[1]);
            }
        }

        private void UiController_OnFinishConversation()
        {
            if (GameState.BattleResult == BattleResult.Win)
            {
                StartCoroutine(DestroyPortal());
            }
            if (LevelConcluded)
            {
                StartCoroutine(FinishLevel());
            }
        }

        IEnumerator DestroyPortal()
        {
            portalAnimator.SetBool("PortalDestroyed", true);
            yield return new WaitForSeconds(2f);
            LevelConcluded = true;
            portal.SetActive(false);
            uiController.StartConversation(conversationComponent.Conversations[3]);
        }

        IEnumerator FinishLevel()
        {
            GameState.SetLastScene(SceneManager.GetActiveScene().name, new Vector3(76.3f, -3f, 0));
            fadeOut = true;
            yield return new WaitForSeconds(1.2f);
            NavigationManager.NavigateTo("Overworld");
        }

        void OnGUI()
        {
            uiHelper.FadeOut(fadeOut, FadeTexture);
        }
    }
}
