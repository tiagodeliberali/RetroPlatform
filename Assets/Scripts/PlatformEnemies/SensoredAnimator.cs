using UnityEngine;

namespace RetroPlatform.PlatformEnemies
{
    public class SensoredAnimator : MonoBehaviour
    {
        Animator gameObjectAnimator;

        void Awake()
        {
            gameObjectAnimator = GetComponent<Animator>();
        }

        public void DestroyGameObject()
        {
            this.gameObject.SetActive(false);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && collision.gameObject.CompareTag("Player"))
            {
                gameObjectAnimator.SetBool("PlayerSeen", true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision != null && collision.gameObject.CompareTag("Player"))
            {
                gameObjectAnimator.SetBool("PlayerSeen", false);
            }
        }
    }
}
