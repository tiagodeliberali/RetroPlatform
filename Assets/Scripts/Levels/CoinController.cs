using UnityEngine;

namespace RetroPlatform.Levels
{
    public class CoinController : MonoBehaviour
    {
        PlayerCore playerCore;

        void Awake()
        {
            var player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerCore = player.PlayerCore;
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
