using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    private PlayerMovement playerMovement;
    [SerializeField] private float idleDelay = 3f;
    private float idleTimer;
    private bool isIdling = false;
    private bool enteredIdle = false;
    private AnimatorStateInfo currentState;

    void Awake()
    {
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        animator.SetInteger("xVelocity", Mathf.RoundToInt(playerMovement.horizontalInput));
        if (playerMovement.isRunning)
        {
            animator.SetTrigger("isRunning");
        }
        currentState = animator.GetCurrentAnimatorStateInfo(0);
        
        if (currentState.IsName("Idle"))
        {
            if (!enteredIdle)
            {
                idleTimer = 0f;
                enteredIdle = true;
            }

            idleTimer += Time.deltaTime;

            if (idleTimer > idleDelay && !isIdling)
            {
                isIdling = true;
                int rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    animator.SetTrigger("Sit");
                }
                else
                {
                    animator.SetTrigger("Laying");
                }
                
            }
        }
        else
        {
            isIdling = false;
            enteredIdle = false;
        }

        if (currentState.IsName("Laying"))
        {
           int rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    animator.SetTrigger("Sleeping1");
                }
                else
                {
                    animator.SetTrigger("Sleeping2");
                }
        }
        
        // if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        // {
        //     int chooseSleeping = Random.Range(0, 2);
        //     if (chooseSleeping == 0)
        //     {
        //         animator.SetBool("idleness", true);
        //     }
        //     else
        //     {
        //         animator.SetBool("idleness", false);
        //     }
        // }

        // if (animator.GetCurrentAnimatorStateInfo(0).IsName("Laying"))
        // {
        //     int chooseSleeping = Random.Range(0, 2);
        //     if (chooseSleeping == 0)
        //     {
        //         animator.SetBool("sleep", true);
        //     }
        //     else
        //     {
        //         animator.SetBool("sleep", false);
        //     }
        // }
    }
}
