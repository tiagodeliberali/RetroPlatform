using RetroPlatform.Conversation;
using UnityEngine;

namespace RetroPlatform
{
    public class BossController : MonoBehaviour
    {
        public ConversationComponent conversationComponent;
        public UIController uiController;

        void Awake()
        {
            uiController.OnFinishConversation += () => gameObject.SetActive(false);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                uiController.StartConversation(conversationComponent.Conversations[0]);
            }
        }
    }
}
