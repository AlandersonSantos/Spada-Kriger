using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealthSystem : MonoBehaviour
{
    [Header("Vida")]
    public int currentHealth = 50;

    public int maxHealth = 50;

    [Header("UI")]
    public Image[] HeartImage;

    public Sprite FullHeart;

    public Sprite HalfHeart;

    public Sprite EmptyHeart;

    // 50 HP / 5 corações
    private int hpPorCoracao = 10;

    void Start()
    {
        currentHealth =
            maxHealth;

        healthLogic();
    }

    void Update()
    {
        // J = meio coração
        if (
            Keyboard.current
            .jKey
            .wasPressedThisFrame
        )
        {
            ChangeHealth(
                -5
            );
        }

        // K = coração inteiro
        if (
            Keyboard.current
            .kKey
            .wasPressedThisFrame
        )
        {
            ChangeHealth(
                -10
            );
        }

        // Restaurar
        if (
            Keyboard.current
            .rKey
            .wasPressedThisFrame
        )
        {
            currentHealth =
                maxHealth;

            healthLogic();
        }
    }

    public void ChangeHealth(
        int amount
    )
    {
        currentHealth +=
            amount;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0,
                maxHealth
            );

        Debug.Log(
            "Vida atual: "
            + currentHealth
        );

        healthLogic();
    }

    void healthLogic()
{
    for (int i = 0; i < HeartImage.Length; i++)
    {
        int vidaDoCoracao =
            currentHealth - (i * hpPorCoracao);

        if (vidaDoCoracao >= 10)
        {
            HeartImage[i].sprite =
                FullHeart;
        }
        else if (vidaDoCoracao >= 5)
        {
            HeartImage[i].sprite =
                HalfHeart;
        }
        else
        {
            HeartImage[i].sprite =
                EmptyHeart;
        }
    }
}
}