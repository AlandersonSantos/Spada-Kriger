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

    [Header("IA de Ataque")]
    [SerializeField] private float raioPercepcao = 3.0f; // Distância que ele nota o Player
    [SerializeField] private float cooldownAtaque = 1.5f; // Intervalo entre ataques
    private float tempoUltimoAtaque;
    private Transform player;

    private Rigidbody2D rb;
    private Animator anim;
    private bool estaMorto = false;


    [Header("Dano por Contato")]
    [SerializeField] private float danoAoContato = 1f;
    [SerializeField] private float forcaArremesso = 8f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Verifica se o player está acima do centro do inimigo
            foreach (ContactPoint2D ponto in collision.contacts)
            {
                if (ponto.normal.y < -0.5f) // Normal negativa indica impacto vindo de cima
                {
                    PlayerMovement playerScript = collision.gameObject.GetComponent<PlayerMovement>();
                    
                    if (playerScript != null)
                    {
                        // Calcula direção do arremesso (para cima e levemente para o lado oposto)
                        Vector2 direcaoArremesso = new Vector2(ponto.normal.x * -1, 1).normalized;
                        playerScript.TomarDano(forcaArremesso, direcaoArremesso);
                    }
                }
            }
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        vidaAtual = vidaMaxima;
        destinoAtual = pontoB; 
        
        // Busca o player pela Tag (Certifique-se que o Kriger tenha a tag "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (estaMorto || player == null) return;

        // Calcula a distância para o Kriger
        float distanciaParaPlayer = Vector2.Distance(transform.position, player.position);

        // Lógica de decisão: Atacar ou Patrulhar
        if (distanciaParaPlayer <= raioPercepcao)
        {
            if (Time.time >= tempoUltimoAtaque + cooldownAtaque)
            {
                AtacarPlayer();
            }
            else
            {
                // Para enquanto espera o cooldown para não "atropelar" o player
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                if (anim != null) anim.SetBool("estaCaminhando", false);
            }
        }
        else
        {
            Patrulhar();
        }
    }

    void Patrulhar()
    {
        Vector2 direcao = (destinoAtual.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direcao.x * velocidade, rb.linearVelocity.y);

        if (anim != null)
        {
            anim.SetBool("estaCaminhando", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        }

        if (Vector2.Distance(transform.position, destinoAtual.position) < 0.5f)
        {
            destinoAtual = (destinoAtual == pontoA) ? pontoB : pontoA;
            Flip();
        }
    }

    void AtacarPlayer()
    {
        tempoUltimoAtaque = Time.time;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Para para atacar

        if (anim != null)
        {
            anim.SetBool("estaCaminhando", false);
            anim.SetTrigger("atacar");
        }

        Debug.Log("Golem percebeu o Kriger e atacou!");
    }

    void Flip()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void TomarDano(float dano)
    {
        if (estaMorto) return;
        vidaAtual -= dano;
        
        if (anim != null) anim.SetTrigger("tomouDano");

        if (vidaAtual <= 0) Morrer();
    }

    void Morrer()
    {
        estaMorto = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; 

        if (anim != null) anim.SetTrigger("morreu");

        Destroy(gameObject, 2f);
    }

    // Desenha o círculo amarelo no Editor para você ajustar o alcance
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioPercepcao);
    }
}