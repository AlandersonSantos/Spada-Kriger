using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 10;

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (
            !other.CompareTag(
                "Player"
            )
        )
            return;

        HealthSystem hp =
            other.GetComponent<
                HealthSystem
            >();

        if (hp != null)
        {
            hp.ChangeHealth(
                healAmount
            );

            Destroy(
                gameObject
            );
        }
    }
}