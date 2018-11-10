using System;
using System.Collections;
using RetroPlatform.Conversation;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform
{
    public class UIController : MonoBehaviour
    {
        public GameObject UILives;
        public Text CoinsAmount;
        public CanvasGroup DialogBox;
        public Image ImageHolder;
        public Text TextHolder;

        bool talking;
        bool skipText;
        ConversationEntry currentConversationLine;

        public event Action OnStartConversation;
        public event Action OnFinishConversation;

        public void UpdateLives(int totalLives, int maxLives)
        {
            var lives = UILives.GetComponentInChildren<Slider>();

            lives.maxValue = maxLives;
            lives.value = totalLives;
        }

        public void UpdateCoins(int totalCoins)
        {
            CoinsAmount.text = totalCoins.ToString();
        }

        public void StartConversation(ConversationArray conversation)
        {
            if (OnStartConversation != null) OnStartConversation();

            DialogBox = GameObject.Find("Dialog Box").GetComponent<CanvasGroup>();
            ImageHolder = GameObject.Find("Dialog Box Image").GetComponent<Image>();
            TextHolder = GameObject.Find("Dialog Box Text").GetComponent<Text>();

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
                ImageHolder.sprite = currentConversationLine.DisplayPic;

                int textySize = currentConversationLine.ConversationText.Length;
                for (int i = 0; i < textySize; i++)
                {
                    TextHolder.text = currentConversationLine.ConversationText.Substring(0, i);
                    if (skipText) yield return new WaitForEndOfFrame();
                    else yield return new WaitForSeconds(0.05f); ;
                }
                TextHolder.text = currentConversationLine.ConversationText;
                yield return new WaitForSeconds(2f);
                skipText = false;
            }
            talking = false;

            if (OnFinishConversation != null) OnFinishConversation();
        }

        void Update()
        {
            if (Input.anyKeyDown) skipText = true;
        }

        void OnGUI()
        {
            if (talking)
            {
                DialogBox.alpha = 1;
                DialogBox.blocksRaycasts = true;
            }
            else if (DialogBox != null)
            {
                DialogBox.alpha = 0;
                DialogBox.blocksRaycasts = false;
            }
        }
    }
}
