using System;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public event Action OnPlayerTouch;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && OnPlayerTouch != null)
        {
            OnPlayerTouch();
        }
    }
}
