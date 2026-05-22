using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public Transform player;

    public int damage = 2;

    public float damageRange = 2f;

    public float damageCooldown = 2f;

    private float nextDamageTime;

    private HealthSystem playerHealth;

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

    void Update()
    {
        if (
            player == null ||
            playerHealth == null
        )
            return;

        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        if (
            distance <=
            damageRange
        )
        {
            TryDamage();
        }
    }

    void TryDamage()
    {
        if (
            Time.time <
            nextDamageTime
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
            "Boss deu "
            + damage +
            " de dano"
        );
    }
}