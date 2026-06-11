using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealthSystem : MonoBehaviour
{
    [Header("Vida")]
    public int currentHealth = 50;
    public int maxHealth = 50;

    [Header("Configurações de Dano / Invulnerabilidade")]
    [SerializeField] private float tempoInvulnerabilidade = 0.5f;
    private float cronometroInvulnerabilidade;
    private bool estaInvulneravel = false;

    [Header("UI")]
    public Image[] HeartImage;
    public Sprite FullHeart;
    public Sprite HalfHeart;
    public Sprite EmptyHeart;

    // 50 HP / 5 corações
    private int hpPorCoracao = 10;

    [Header("Áudio")]
    [SerializeField] private AudioClip somHit;

    // Referência privada para o Animator do jogador
    private Animator anim;

    void Start()
    {
        // Pega o componente Animator anexado ao mesmo GameObject
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        healthLogic();
    }

    void Update()
    {
        // Controla o tempo de imunidade a cada frame
        if (estaInvulneravel)
        {
            cronometroInvulnerabilidade -= Time.deltaTime;
            if (cronometroInvulnerabilidade <= 0)
            {
                estaInvulneravel = false;
            }
        }

        // J = meio coração
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            ChangeHealth(-5);
        }

        // K = coração inteiro
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            ChangeHealth(-10);
        }

        // Restaurar
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            currentHealth = maxHealth;
            estaInvulneravel = false; // Reseta a imunidade ao restaurar a vida
            healthLogic();
        }
    }

    public void ChangeHealth(int amount)
    {
        // Se for dano (amount negativo) e o player já estiver invulnerável, ignora o golpe
        if (amount < 0 && estaInvulneravel)
        {
            return;
        }

        // Se a quantidade for negativa (dano) e passou pela checagem acima, ele pode ser atingido
        if (amount < 0)
        {
            // Ativa a invulnerabilidade imediatamente para travar novos hits seguidos
            estaInvulneravel = true;
            cronometroInvulnerabilidade = tempoInvulnerabilidade;

            // Toca o áudio se ele estiver configurado
            if (somHit != null)
            {
                AudioManager.Instancia.PlayOneShotAudio(somHit);
            }

            // Dispara a animação de hit usando o Any State se o player sobreviver
            if (anim != null && (currentHealth + amount) > 0)
            {
                anim.SetTrigger("levarHit");
            }
        }

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Vida atual: " + currentHealth);

        healthLogic();

        // Checagem de derrota
        if (currentHealth <= 0)
        {
            Debug.Log("O jogador morreu!");
            // Caso tenha uma animação de morte, pode ativar aqui:
            // anim.SetTrigger("morrer");
        }
    }

    void healthLogic()
    {
        for (int i = 0; i < HeartImage.Length; i++)
        {
            int vidaDoCoracao = currentHealth - (i * hpPorCoracao);

            if (vidaDoCoracao >= 10)
            {
                HeartImage[i].sprite = FullHeart;
            }
            else if (vidaDoCoracao >= 5)
            {
                HeartImage[i].sprite = HalfHeart;
            }
            else
            {
                HeartImage[i].sprite = EmptyHeart;
            }
        }
    }
}