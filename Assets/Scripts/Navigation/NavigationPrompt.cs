using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroPlatform.Navigation
{
    public class NavigationPrompt : MonoBehaviour
    {
        public Texture2D FadeTexture;
        public Vector2 GoBackPosition;

        UIHelper uiHelper = new UIHelper();
        bool fadeOut;

        void OnTriggerEnter2D(Collider2D collider)
        {
            string destination = tag;
            if (collider.gameObject.CompareTag("Player") && NavigationManager.CanNavigate(destination))
            {
                GameState.SetLastScene(SceneManager.GetActiveScene().name, new Vector3(GoBackPosition.x, GoBackPosition.y, 0));
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