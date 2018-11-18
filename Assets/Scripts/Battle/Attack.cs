using System;
using Assets.Scripts.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform.Battle
{
    public class Attack : MonoBehaviour
    {
        public Button sword;
        public Button bow;
        public Button magic;

        public event Action<BaseAttack> OnAttackSelected;

        private BaseAttack currentAttack;

        void Awake()
        {
            currentAttack = null;
            HighlightButton();
        }

        public void Bow()
        {
            SelectAttack<AttackBow>();
        }

        public void Sword()
        {
            SelectAttack<AttackSword>();
        }

        public void Magic()
        {
            SelectAttack<AttackMagic>();
        }

        private void SelectAttack<T>()
            where T : BaseAttack, new()
        {
            if (currentAttack != null && currentAttack.Locked) return;
            currentAttack = new T();
            AttackSelected();
        }

        private void AttackSelected()
        {
            HighlightButton();
            if (OnAttackSelected != null) OnAttackSelected(currentAttack);
        }

        void HighlightButton()
        {
            if (currentAttack is AttackSword) sword.GetComponent<Outline>().enabled = true;
            else sword.GetComponent<Outline>().enabled = false;

            if (currentAttack is AttackBow) bow.GetComponent<Outline>().enabled = true;
            else bow.GetComponent<Outline>().enabled = false;

            if (currentAttack is AttackMagic) magic.GetComponent<Outline>().enabled = true;
            else magic.GetComponent<Outline>().enabled = false;
        }

        public void ClearAttack()
        {
            currentAttack = null;
        }
    }
}