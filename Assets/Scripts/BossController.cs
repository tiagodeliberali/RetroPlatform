﻿using System;
using System.Collections;
using UnityEngine;

namespace RetroPlatform
{
    public class BossController : MonoBehaviour
    {
        public UIController UIController;

        public event Action OnTouchPlayer;

        BoxCollider2D boxCollider;
        Rigidbody2D bossRigidBody2D;
        SpriteRenderer bossSpriteImage;
        float velocity = 0f;

        void Awake()
        {
            boxCollider = (BoxCollider2D)GetComponent(typeof(BoxCollider2D));
            bossRigidBody2D = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
            bossSpriteImage = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && OnTouchPlayer != null) OnTouchPlayer();
        }

        void Update()
        {
            bossRigidBody2D.velocity = new Vector2(velocity, bossRigidBody2D.velocity.y);
        }

        public void RunAway()
        {
            StartCoroutine(RunAwayAnimation());
        }

        IEnumerator RunAwayAnimation()
        {
            boxCollider.isTrigger = true;
            bossSpriteImage.flipX = false;
            bossRigidBody2D.velocity = new Vector2(velocity, 25f);
            velocity = 15f;
            yield return new WaitForSeconds(3f);
            velocity = 0f;
            gameObject.SetActive(false);
        }
    }
}
