using UnityEngine;

public class InimigoSimples : MonoBehaviour
{
    [Header("Configurações de Vida")]
    [SerializeField] private float vidaMaxima = 3;
    private float vidaAtual;

    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidade = 2f;
    public Transform pontoA;
    public Transform pontoB;
    private Transform destinoAtual;

    private Rigidbody2D rb;
    private Animator anim;
    private bool estaMorto = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        vidaAtual = vidaMaxima;
        destinoAtual = pontoB; // Começa patrulhando em direção ao Ponto B
    }

    void Update()
    {
        if (estaMorto) return;

        Patrulhar();
    }

    void Patrulhar()
    {
        // Calcula a direção para o próximo ponto
        Vector2 direcao = (destinoAtual.position - transform.position).normalized;
        
        // Aplica a velocidade no Rigidbody (mantendo a gravidade no eixo Y)
        rb.linearVelocity = new Vector2(direcao.x * velocidade, rb.linearVelocity.y);

        // --- ATUALIZAÇÃO DO ANIMATOR ---
        // Se a velocidade horizontal for diferente de zero, ativa o bool no Animator
        if (anim != null)
        {
            anim.SetBool("estaCaminhando", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        }

        // Verifica se chegou perto o suficiente do ponto para trocar de direção
        if (Vector2.Distance(transform.position, destinoAtual.position) < 0.5f)
        {
            if (destinoAtual == pontoA)
            {
                destinoAtual = pontoB;
            }
            else
            {
                destinoAtual = pontoA;
            }
            
            Flip();
        }
    }

    void Flip()
    {
        // Inverte a escala X do inimigo para ele olhar para o lado certo
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    // Chamado pelo script PlayerAtaque do Kriger
    public void TomarDano(float dano)
    {
        if (estaMorto) return;

        vidaAtual -= dano;
        
        // Toca o Trigger de dano no Animator
        if (anim != null)
        {
            anim.SetTrigger("tomouDano");
        }

        Debug.Log("Inimigo atingido! Vida: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        estaMorto = true;
        
        // 1. Para o movimento e ignora a gravidade
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; // Isso "prega" o Golem no ar/chão

        // 2. Toca a animação
        if (anim != null)
        {
            anim.SetTrigger("morreu");
        }

        // 3. Opcional: Desative o colisor apenas se o corpo estiver atrapalhando o caminho, 
        // mas agora ele não vai cair porque o bodyType é Static.
        // GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 2f);
    }
}