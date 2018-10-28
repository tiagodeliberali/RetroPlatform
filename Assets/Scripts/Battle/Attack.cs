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

        public BaseAttack CurrentAttack;

        private void Awake()
        {
            HighlightButton();
        }

        public void Bow()
        {
            CurrentAttack = new AttackBow();
            AttackSelected();
        }

        public void Sword()
        {
            CurrentAttack = new AttackSword();
            AttackSelected();
        }

        public void Magic()
        {
            CurrentAttack = new AttackMagic();
            AttackSelected();
        }

        private void AttackSelected()
        {
            HighlightButton();
        }

        void HighlightButton()
        {
            if (CurrentAttack is AttackSword) sword.GetComponent<Outline>().enabled = true;
            else sword.GetComponent<Outline>().enabled = false;

            if (CurrentAttack is AttackBow) bow.GetComponent<Outline>().enabled = true;
            else bow.GetComponent<Outline>().enabled = false;

            if (CurrentAttack is AttackMagic) magic.GetComponent<Outline>().enabled = true;
            else magic.GetComponent<Outline>().enabled = false;
        }

        public bool ReadyToAttack
        {
            get
            {
                return CurrentAttack != null;
            }
        }

        public bool IsReadyToAttack(int count)
        {
            return count >= CurrentAttack.EnemiesRange;
        }

        public void ClearAttack()
        {
            HighlightButton();
            CurrentAttack = null;
        }
    }
}