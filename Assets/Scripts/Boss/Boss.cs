using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform player;

    [Header("Movimento")]
    public float moveSpeed = 3f;
    public float detectionRange = 10f;
    public float attackRange = 2f;

    [Header("Ataque")]
    public float attackCooldown = 2f;

    private Animator anim;
    private Rigidbody2D rb;

    private float nextAttackTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Desativa gravidade para evitar travamentos
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        float distanceX =
            Mathf.Abs(
                player.position.x -
                transform.position.x
            );

        FlipBoss();

        // Detectou o player
        if (distanceX <= detectionRange)
        {
            // Entrou no alcance do ataque
            if (distanceX <= attackRange)
            {
                StopMoving();
                Attack();
            }
            else
            {
                MoveToPlayer();

                anim.SetBool(
                    "walking",
                    true
                );
            }
        }
        else
        {
            StopMoving();
        }
    }

    void MoveToPlayer()
    {
        float direction =
            Mathf.Sign(
                player.position.x -
                transform.position.x
            );

        transform.position +=
            Vector3.right *
            direction *
            moveSpeed *
            Time.fixedDeltaTime;
    }

    void StopMoving()
    {
        anim.SetBool(
            "walking",
            false
        );
    }

    void Attack()
    {
        if (
            Time.time <
            nextAttackTime
        )
            return;

        anim.SetTrigger(
            "attack"
        );

        nextAttackTime =
            Time.time +
            attackCooldown;
    }

    void FlipBoss()
    {
        if (
            player.position.x >
            transform.position.x
        )
        {
            transform.localScale =
                new Vector3(
                    -1,
                    1,
                    1
                );
        }
        else
        {
            transform.localScale =
                new Vector3(
                    1,
                    1,
                    1
                );
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}