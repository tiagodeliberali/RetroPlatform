using System.Collections;
using UnityEngine;

namespace RetroPlatform.Navigation
{
    public class NavigationPrompt : MonoBehaviour
    {
        public Texture2D FadeTexture;
        
        float fadespeed = 0.5f;
        int drawDepth = -1000;
        float alpha = 0f;
        float fadeDir = -1f;
        bool fadeOut;

        void OnTriggerEnter2D(Collider2D collider)
        {
            string destination = tag;
            if (collider.gameObject.CompareTag("Player") && NavigationManager.CanNavigate(destination))
            {
                fadeOut = true;
                collider.gameObject.SetActive(false);
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
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);
        }
    }
}