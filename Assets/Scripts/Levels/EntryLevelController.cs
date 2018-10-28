using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroPlatform.Levels
{
    public class EntryLevelController : MonoBehaviour
    {
        private const string LEFT_SCENE = "BossLeftScene";

        public UIController uiController;
        public BossController bossController;

        void Awake()
        {
            uiController.OnFinishConversation += () => GameState.SetGameFact(SceneManager.GetActiveScene().name, LEFT_SCENE, true);

            object fact = GameState.GetGameFact(SceneManager.GetActiveScene().name, LEFT_SCENE);
            bool leftScene = fact == null ? false : (bool)fact;

            if (leftScene) bossController.RunAway();
        }
    }
}
