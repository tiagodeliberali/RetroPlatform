﻿using Assets.Scripts.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace RetroPlatform.Battle
{
    public class Attack : MonoBehaviour
    {
        bool locked;
        public Button sword;
        public Button bow;
        public Button magic;
        public BaseAttack CurrentAttack;

        void Awake()
        {
            CurrentAttack = null;
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
            if (locked) return;
            CurrentAttack = new T();
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
            CurrentAttack = null;
            HighlightButton();
            locked = false;
        }

        public void Lock()
        {
            locked = true;
        }
    }
}