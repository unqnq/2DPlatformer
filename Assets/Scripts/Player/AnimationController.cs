using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    private PlayerMovement playerMovement;

    void Awake()
    {

        playerMovement = transform.parent.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        animator.SetInteger("xVelocity", Mathf.RoundToInt(playerMovement.horizontalMovement));

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            int chooseSleeping = Random.Range(0, 2);
            if (chooseSleeping == 0)
            {
                animator.SetBool("idleness", true);
            }
            else
            {
                animator.SetBool("idleness", false);
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Laying"))
        {
            int chooseSleeping = Random.Range(0, 2);
            if (chooseSleeping == 0)
            {
                animator.SetBool("sleep", true);
            }
            else
            {
                animator.SetBool("sleep", false);
            }
        }
    }
}
