using UnityEngine;

namespace RetroPlatform.Levels
{
    public class CoinController : MonoBehaviour
    {
        PlayerCore playerCore;

        void Awake()
        {
            var playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerCore = playerController.PlayerCore;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerCore.AddCoins(1);
                gameObject.SetActive(false);
            }
        }
    }
}
