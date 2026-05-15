using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealthSystem : MonoBehaviour
{
    public int currentHealth = 10;
    public int maxHealth = 10;

    public Image[] HeartImage;

    public Sprite FullHeart;
    public Sprite HalfHeart;
    public Sprite EmptyHeart;

    void Start()
    {
        currentHealth = maxHealth;
        healthLogic();
    }

    void Update()
    {
        // J = meio coração
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            ChangeHealth(-1);
        }

        // K = um coração
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            ChangeHealth(-2);
        }

        // R = restaura tudo
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            currentHealth = maxHealth;
            healthLogic();
        }

            if (Keyboard.current.jKey.wasPressedThisFrame) {
            Debug.Log("J apertado");
        }

    }

    public void ChangeHealth(int amount) {
        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Vida atual: " + currentHealth);

        healthLogic();
    }

    void healthLogic()
    {
        for (int i = 0; i < HeartImage.Length; i++)
        {
            if (currentHealth >= (i + 1) * 2)
                HeartImage[i].sprite = FullHeart;

            else if (currentHealth == (i * 2) + 1)
                HeartImage[i].sprite = HalfHeart;

            else
                HeartImage[i].sprite = EmptyHeart;
        }
    }

}