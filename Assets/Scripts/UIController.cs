using RetroPlatform.Conversation;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform
{
    public class UIController : MonoBehaviour
    {
        public GameObject UILives;
        public Text CoinsAmount;

        public CanvasGroup dialogBox;
        public Image imageHolder;
        public Text textHolder;

        bool talking = false;
        ConversationEntry currentConversationLine;

        public event Action OnStartConversation;
        public event Action OnFinishConversation;

        public void UpdateLives(int totalLives)
        {
            var lives = UILives.GetComponentsInChildren<CanvasRenderer>();
            for (int i = 0; i < lives.Length; i++)
                lives[i].SetAlpha(i < totalLives ? 100f : 0f);
        }

        public void UpdateCoins(int totalCoins)
        {
            CoinsAmount.text = totalCoins.ToString();
        }

        public void StartConversation(ConversationArray conversation)
        {
            if (OnStartConversation != null) OnStartConversation();

            dialogBox = GameObject.Find("Dialog Box").GetComponent<CanvasGroup>();
            imageHolder = GameObject.Find("Dialog Box Image").GetComponent<Image>();
            textHolder = GameObject.Find("Dialog Box Text").GetComponent<Text>();

            if (!talking)
            {
                StartCoroutine(DisplayConversation(conversation));
            }
        }

        IEnumerator DisplayConversation(ConversationArray conversation)
        {
            talking = true;
            foreach (var conversationLine in conversation.ConversationLines)
            {
                currentConversationLine = conversationLine;
                imageHolder.sprite = currentConversationLine.DisplayPic;

                int textySize = currentConversationLine.ConversationText.Length;
                for (int i = 0; i < textySize; i++)
                {
                    textHolder.text = currentConversationLine.ConversationText.Substring(0, i);
                    yield return new WaitForSeconds(0.05f);
                }
                textHolder.text = currentConversationLine.ConversationText;
                yield return new WaitForSeconds(2f);
            }
            talking = false;

            if (OnFinishConversation != null) OnFinishConversation();
        }

        void OnGUI()
        {
            if (talking)
            {
                dialogBox.alpha = 1;
                dialogBox.blocksRaycasts = true;
            }
            else if (dialogBox != null)
            {
                dialogBox.alpha = 0;
                dialogBox.blocksRaycasts = false;
            }
        }
    }
}
