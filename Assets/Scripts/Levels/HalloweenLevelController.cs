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
        public UIController uiController;
        public SpriteRenderer hammer;
        public GameObject ghost;
        public GameObject portal;
        public Texture2D FadeTexture;

        bool PlayerInitialTalk
        {
            get
            {
                return GameState.GetGameFactBoolean(GameFact.HalloweenLevelInitialPlayerTalk);
            }
            set
            {
                GameState.SetGameFact(GameFact.HalloweenLevelInitialPlayerTalk, value);
            }
        }
        bool GetPortalDestroyerHammer
        {
            get
            {
                return GameState.GetGameFactBoolean(GameFact.GetPortalDestroyerHammer);
            }
            set
            {
                GameState.SetGameFact(GameFact.GetPortalDestroyerHammer, value);
            }
        }
        public static bool LevelConcluded
        {
            get
            {
                return GameState.GetGameFactBoolean(GameFact.HalloweenLevelPortalDestroyed);
            }
            set
            {
                GameState.SetGameFact(GameFact.HalloweenLevelPortalDestroyed, value);
            }
        }
        BattleResult GhostsBattleResult
        {
            get
            {
                return GameState.GetBattleResult(BattleName.HalloweenLevelFishGosts);
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

            if (!PlayerInitialTalk)
            {
                uiController.StartConversation(conversationComponent.Conversations[0]);
                PlayerInitialTalk = true;
            }
            if (GhostsBattleResult == BattleResult.Win)
            {
                hammer.gameObject.SetActive(false);
                ghost.SetActive(false);
            }
            if (LevelConcluded)
            {
                portal.SetActive(false);
            }
        }

        private void Portal_OnPlayerTouch()
        {
            if (GetPortalDestroyerHammer)
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
            if (GhostsBattleResult == BattleResult.Win)
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

        void Update()
        {
            if (GhostsBattleResult == BattleResult.Win && !GetPortalDestroyerHammer)
            {
                GetPortalDestroyerHammer = true;
            }
        }

        void OnGUI()
        {
            uiHelper.FadeOut(fadeOut, FadeTexture);
        }
    }
}
