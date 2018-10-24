using UnityEngine;

namespace RetroPlatform
{
    public class CoinController : MonoBehaviour
    {
        PlayerCore playerCore;

        void Awake()
        {
            var player = GameObject.FindWithTag("Player").GetComponent<Player>();
            playerCore = player.Core;
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
