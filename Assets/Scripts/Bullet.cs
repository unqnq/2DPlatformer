using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy)
        {
            enemy.TakeDamage(bulletDamage);
        }
    }
}
