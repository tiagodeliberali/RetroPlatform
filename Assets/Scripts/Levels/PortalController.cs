using System;
using System.Collections;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public event Action OnPlayerTouch;
    private bool waiting;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!waiting && collision.gameObject.CompareTag("Player") && OnPlayerTouch != null)
        {
            OnPlayerTouch();
            waiting = true;
            StartCoroutine(WaitBeforeAnouce());
        }
    }

    IEnumerator WaitBeforeAnouce()
    {
        yield return new WaitForSeconds(10);
        waiting = false;
    }
}
