using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Components and objects")]
    private Rigidbody2D rb;
    public ParticleSystem smokeFx;
    public ParticleSystem speedFx;
    private Vector3 speedFxInitialScale;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 15f;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float doubleTapTime = 0.2f;

    [HideInInspector] public bool isRunning = false;
    private float lastTapTime = -1f;
    private int lastDirection = 0;
    private int currentDirection = 0;
    private Transform spriteTransform;
    [HideInInspector] public float horizontalInput = 0f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpPower = 15f;
    [SerializeField] private int maxJumps = 2;
    private int currentJumps = 0;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Gravity Settings")]
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float fallMultiplier = 4f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = baseGravity;
        spriteTransform = transform.GetChild(0);
        speedFxInitialScale = speedFx.transform.localScale;
        SpeedBooster.OnSpeedCollected += StartSpeedBooster;
    }
    void OnDestroy()
    {
        SpeedBooster.OnSpeedCollected -= StartSpeedBooster;
    }

    void StartSpeedBooster(float multiplier)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier));
    }

    IEnumerator SpeedBoostCoroutine(float multiplier)
    {
        speedMultiplier = multiplier;
        speedFx.Play();
        yield return new WaitForSeconds(10f);
        speedMultiplier = 1f;
        speedFx.Stop();
    }
    private void Update()
    {
        CheckGrounded();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        ApplyGravity();
        FlipSprite();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;

        if (context.performed && horizontalInput != 0)
        {
            HandleDoubleTap();
        }

        if (context.canceled)
        {
            horizontalInput = 0f;
            isRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (currentJumps <= 0) return;

        if (context.performed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            currentJumps--;
            smokeFx.Play();
        }
        else if (context.canceled)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower / 2);
            currentJumps--;
            smokeFx.Play();
        }
    }

    private void MoveCharacter()
    {
        float speed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(horizontalInput * speed * speedMultiplier, rb.linearVelocity.y);
    }

    private void ApplyGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void CheckGrounded()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer))
        {
            currentJumps = maxJumps;
        }
    }

    private void FlipSprite()
    {
        if (horizontalInput == 0) return;

        currentDirection = horizontalInput > 0 ? 1 : -1;
        spriteTransform.localScale = new Vector3(currentDirection*1.5f, 1.5f, 1.5f);
        speedFx.transform.localScale = new Vector3(speedFxInitialScale.x * currentDirection, speedFxInitialScale.y, speedFxInitialScale.z);
    }

    private void HandleDoubleTap()
    {
        currentDirection = horizontalInput > 0 ? 1 : -1;

        if (currentDirection == lastDirection && Time.time - lastTapTime < doubleTapTime)
        {
            isRunning = true;
            smokeFx.Play();
        }
        else
        {
            isRunning = false;
        }

        lastTapTime = Time.time;
        lastDirection = currentDirection;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}