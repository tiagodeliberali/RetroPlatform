using System;
using System.Collections.Generic;
using Assets.Scripts.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform.Battle
{
    public class AttackController : MonoBehaviour
    {
        public AttackDefinitionArray AttackDefinitions;
        public GameObject AttackMenu;

        public event Action<BaseAttack> OnAttackSelected;

        private BaseAttack currentAttack;
        private Dictionary<AttackName, GameObject> attackButtons = new Dictionary<AttackName, GameObject>();

        void Awake()
        {
            currentAttack = null;
            HighlightButton();
        }

        public void LoadAttacks(PlayerCore player)
        {
            foreach (var attack in player.Attacks)
            {
                BaseAttack baseAttack = BaseAttack.GetAttack(attack, AttackDefinitions.AttackDefinitions);

                GameObject button = new GameObject();
                button.transform.parent = AttackMenu.transform;
                button.AddComponent<RectTransform>();
                button.AddComponent<Button>();
                button.AddComponent<CanvasRenderer>();
                button.AddComponent<Image>();
                button.AddComponent<Outline>();

                button.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                button.GetComponent<Image>().sprite = baseAttack.Info.Image;
                button.GetComponent<Outline>().enabled = false;
                button.GetComponent<Button>().onClick.AddListener(() => SelectAttack(baseAttack));

                attackButtons.Add(attack, button);
            }
        }

        private void SelectAttack(BaseAttack attack)
        {
            if (currentAttack != null && currentAttack.Locked) return;
            currentAttack = attack;
            AttackSelected();
        }

        private void AttackSelected()
        {
            HighlightButton();
            if (OnAttackSelected != null) OnAttackSelected(currentAttack);
        }

        void HighlightButton()
        {
            foreach (var buttonEntry in attackButtons)
            {
                if (currentAttack != null && currentAttack.Name == buttonEntry.Key)
                    buttonEntry.Value.GetComponent<Outline>().enabled = true;
                else
                    buttonEntry.Value.GetComponent<Outline>().enabled = false;
            }
        }

        public void ClearAttack()
        {
            if (currentAttack != null) currentAttack.Locked = false;
            currentAttack = null;
            HighlightButton();
        }
    }
}