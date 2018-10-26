using System.Collections;
using UnityEngine;

namespace RetroPlatform.Navigation
{
    public class NavigationPrompt : MonoBehaviour
    {
        public Texture2D fadeTexture;
        public Player player;

        float fadespeed = 0.5f;
        int drawDepth = -1000;

        private float alpha = 0f;
        private float fadeDir = -1f;

        bool fadeOut;

        void OnTriggerEnter2D(Collider2D col)
        {
            string destination = tag;
            if (col.gameObject.CompareTag("Player") && NavigationManager.CanNavigate(destination))
            {
                GameState.UpdatePlayerData(player.Core);
                fadeOut = true;
                col.gameObject.SetActive(false);
                StartCoroutine(MoveToScene(destination));
            }
        }

        IEnumerator MoveToScene(string destination)
        {
            yield return new WaitForSeconds(1f);
            NavigationManager.NavigateTo(destination);
        }

        void OnGUI()
        {
            if (!fadeOut) return;

            alpha -= fadeDir * fadespeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            Color newColor = GUI.color;
            newColor.a = alpha;

            GUI.color = newColor;

            GUI.depth = drawDepth;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        }
    }
}