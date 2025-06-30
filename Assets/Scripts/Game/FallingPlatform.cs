using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallWait = 0.5f;
    public float destroyWait = 1f;

    bool isFalling;
    Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallWait);
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyWait);
    }
}
