using UnityEngine;

public class BossDamage : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    private HealthSystem playerHealth;

    [Header("Dano")]
    public int damage = 2;

    public float damageRange = 2f;

    public float damageCooldown = 2f;

    private float nextDamageTime;

    void Start()
    {
        if (player != null)
        {
            playerHealth =
                player.GetComponent<
                    HealthSystem
                >();
        }
    }

    // CHAMADO PELO ANIMATION EVENT
    public void DealDamage()
    {
        if (
            player == null
            ||
            playerHealth == null
        )
            return;

        if (
            Time.time <
            nextDamageTime
        )
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        if (
            distance >
            damageRange
        )
            return;

        playerHealth
            .ChangeHealth(
                -damage
            );

        nextDamageTime =
            Time.time +
            damageCooldown;

        Debug.Log(
            "Boss causou "
            + damage +
            " de dano"
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color =
            Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            damageRange
        );
    }
}