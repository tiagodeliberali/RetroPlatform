using UnityEngine;

namespace RetroPlatform
{
    public class PopUpMenu : MonoBehaviour
    {
        bool displayMenu;

        public CanvasGroup popUp;

        void Awake()
        {
            popUp = GetComponent<CanvasGroup>();
        }

        public void ToggleMenu()
        {
            if (displayMenu) DisableTheMenu();
            else EnableTheMenu();

            displayMenu = !displayMenu;
        }

        public void EnableTheMenu()
        {

            popUp.alpha = 1;
            popUp.interactable = true;
            popUp.blocksRaycasts = true;

        }

        public void DisableTheMenu()
        {
            popUp.alpha = 0;
            popUp.interactable = false;
            popUp.blocksRaycasts = false;
        }
    }
}