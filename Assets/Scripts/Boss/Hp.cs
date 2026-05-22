using UnityEngine;

public class BossHP : MonoBehaviour
{
    [Header("Vida")]
    public float maxHP = 5000f;

    private float currentHP;

    [Header("Dano")]
    public float damageFromPlayer = 100f;

    [Header("Player")]
    public Animator playerAnimator;

    [Header("Ataques")]
    public string attack1 = "attack1";
    public string attack2 = "attack2";

    private bool canTakeDamage = true;

    private bool dead = false;

    private Animator anim;

    void Start()
    {
        currentHP = maxHP;

        anim =
            GetComponent<
                Animator
            >();
    }

    public void TakeDamage(
        float damage
    )
    {
        if (
            dead
        )
            return;

        currentHP -= damage;

        currentHP =
            Mathf.Clamp(
                currentHP,
                0,
                maxHP
            );

        Debug.Log(
            "HP Boss: "
            + currentHP
        );

        // HIT
        if (
            currentHP > 0
        )
        {
            anim.SetTrigger(
                "hit"
            );
        }

        // MORTE
        if (
            currentHP <= 0
        )
        {
            BossDefeated();
        }
    }

    void BossDefeated()
    {
        dead = true;

        anim.SetBool(
            "walking",
            false
        );

        anim.ResetTrigger(
            "attack"
        );

        anim.SetBool(
            "death",
            true
        );

        BossAI ai =
            GetComponent<
                BossAI
            >();

        if (
            ai != null
        )
            ai.enabled = false;

        Collider2D col =
            GetComponent<
                Collider2D
            >();

        if (
            col != null
        )
            col.enabled = false;

        Destroy(
            gameObject,
            3f
        );
    }

    private void OnTriggerStay2D(
        Collider2D other
    )
    {
        if (
            dead
        )
            return;

        if (
            !other.CompareTag(
                "Player"
            )
        )
            return;

        AnimatorStateInfo state =
            playerAnimator
            .GetCurrentAnimatorStateInfo(
                0
            );

        bool attacking =
            state.IsName(
                attack1
            )
            ||
            state.IsName(
                attack2
            );

        if (
            attacking &&
            canTakeDamage
        )
        {
            TakeDamage(
                damageFromPlayer
            );

            canTakeDamage =
                false;

            Invoke(
                nameof(
                    ResetDamage
                ),
                0.5f
            );
        }
    }

    void ResetDamage()
    {
        canTakeDamage =
            true;
    }
}