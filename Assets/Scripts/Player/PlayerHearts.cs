using System;
using System.Collections;
using UnityEngine;

public class PlayerHearts : MonoBehaviour
{
    public int maxHearts = 3;
    public HealthBarUI healthBarUI;

    private int currentHearts;
    private SpriteRenderer spriteRenderer;
    public static event Action OnPlayerDied;

    void Start()
    {
        currentHearts = maxHearts;
        healthBarUI.SetMaxHearts(maxHearts);
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        HealthItem.OnHealCollect += Heal;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
    }

    void TakeDamage(int damage)
    {
        currentHearts -= damage;
        SoundEffectManager.Play("PlayerHit");
        healthBarUI.UpdateHearts(currentHearts);
        StartCoroutine(FlashRed());
        if (currentHearts <= 0)
        {
            // Debug.Log("player is dead!");
            OnPlayerDied?.Invoke();
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    void Heal(int amount)
    {
        if (currentHearts >= maxHearts) return;
        currentHearts += amount;
        if (currentHearts > maxHearts) currentHearts = maxHearts;

        healthBarUI.UpdateHearts(currentHearts);
    }
}
