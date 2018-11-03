using UnityEngine;

public class SpiderController : MonoBehaviour {
    Animator spiderAnimator;

    void Awake () {
        spiderAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            spiderAnimator.SetBool("PlayerSeen", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            spiderAnimator.SetBool("PlayerSeen", false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
