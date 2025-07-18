using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 1;

    public float speed = 2f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public Transform player;

    public int maxHealth = 3;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private bool isGroundedRight, isGroundedLeft;
    private Vector2 leftRayOrigin, rightRayOrigin;
    private bool shouldJump;
    private bool hasJumped = false;

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        leftRayOrigin = new Vector2(transform.position.x - 1.2f, transform.position.y);
        rightRayOrigin = new Vector2(transform.position.x + 1f, transform.position.y);
        isGroundedLeft = Physics2D.Raycast(leftRayOrigin, Vector2.down, 1.5f, groundLayer);
        isGroundedRight = Physics2D.Raycast(rightRayOrigin, Vector2.down, 1.5f, groundLayer);
        Debug.DrawRay(leftRayOrigin, Vector2.down * 1.5f, Color.red);
        Debug.DrawRay(rightRayOrigin, Vector2.down * 1.5f, Color.red);
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, 1 << player.gameObject.layer);
        if (isGroundedRight || isGroundedLeft)
        {
            hasJumped = false;
            transform.localScale = (direction > 0) ? new Vector3(-1.5f, 1.5f, 1.5f) : new Vector3(1.5f, 1.5f, 1.5f);
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocityY);
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);
            if (!groundInFront.collider && !gapAhead.collider)
            {
                shouldJump = true;
            }
            else if (platformAbove.collider && isPlayerAbove)
            {
                shouldJump = true;
            }
        }
    }

    void FixedUpdate()
    {
        if ((isGroundedLeft || isGroundedRight) && shouldJump && !hasJumped)
        {
            shouldJump = false;
            hasJumped = true;
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 jumpDirection = direction * jumpForce;
            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        foreach (LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                IntantiateLoot(lootItem.lootPrefab);
            }
            break;
        }
        Destroy(gameObject);
    }

    void IntantiateLoot(GameObject loot)
    {
        if (loot)
        {
            GameObject droppetLoot = Instantiate(loot, transform.position, Quaternion.identity);
        }
    }
}
