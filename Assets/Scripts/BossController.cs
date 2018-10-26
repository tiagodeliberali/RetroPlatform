using RetroPlatform.Conversation;
using System.Collections;
using UnityEngine;

namespace RetroPlatform
{
    public class BossController : MonoBehaviour
    {
        public ConversationComponent conversationComponent;
        public UIController uiController;
        public BoxCollider2D boxCollider;
        public bool LeftScene;

        Rigidbody2D bossRigidBody2D;
        SpriteRenderer bossSpriteImage;
        
        float velocity = 0f;

        void Awake()
        {
            bossRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
            bossSpriteImage = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));

            uiController.OnFinishConversation += () => StartCoroutine(RunAway());
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !LeftScene)
            {
                uiController.StartConversation(conversationComponent.Conversations[0]);
            }
        }

        void Update()
        {
            bossRigidBody2D.velocity = new Vector2(velocity, bossRigidBody2D.velocity.y);

            if (LeftScene)
            {
                StartCoroutine(RunAway());
                LeftScene = false;
            }
        }

        IEnumerator RunAway()
        {
            boxCollider.isTrigger = true;
            bossSpriteImage.flipX = false;
            bossRigidBody2D.velocity = new Vector2(velocity, 25f);
            velocity = 15f;
            yield return new WaitForSeconds(3f);
            velocity = 0f;
            gameObject.SetActive(false);
        }
    }
}
