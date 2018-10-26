using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroPlatform
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
            bossController.LeftScene = fact == null ? false : (bool)fact;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
