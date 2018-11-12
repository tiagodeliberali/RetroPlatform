using System.Collections;
using UnityEngine;

namespace RetroPlatform.Navigation
{
    public class NavigationPrompt : MonoBehaviour
    {
        public Texture2D FadeTexture;

        UIHelper uiHelper = new UIHelper();
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
            uiHelper.FadeOut(fadeOut, FadeTexture);
        }
    }
}