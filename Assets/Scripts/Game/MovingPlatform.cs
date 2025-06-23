using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;

    private Vector3 nextPosition;
    void Start()
    {
        pointA = transform.parent.Find("PointA");
        pointB = transform.parent.Find("PointB");
        nextPosition = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DetachPlayerNextFrame(collision.transform, true));
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DetachPlayerNextFrame(collision.transform, false));
        }
    }
    IEnumerator DetachPlayerNextFrame(Transform player, bool onGround)
    {
        yield return null; // зачекати 1 кадр
        if (player != null && player.parent == transform)
        {
            player.SetParent(null, onGround);
        }
    }
}
