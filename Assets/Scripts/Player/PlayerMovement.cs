using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Componentes
    private Rigidbody2D rb;
    private Animator anim;

    // Variáveis de Movimento Base
    private float movement;
    private bool olhandoParaDireita = true;
    private float velocidadeAtual;

    [Header("Configurações de Velocidade")]
    [SerializeField] [Range(1, 10)] private float velocidadeCaminhada = 5.0f;
    [SerializeField] [Range(5, 20)] private float velocidadeCorrida = 10.0f;

    [Header("Configurações de Som corrida e andando")]

    [SerializeField] private AudioClip somPassoCorrendo;
    [SerializeField] private float intervaloPasso = 0.5f;
    [SerializeField] private float intervaloPassoCorrendo = 0.2f;

    private float timerPassos;

    [Header("Configurações de pulo")]
    [SerializeField] private AudioClip somPulo;

    [Header("Configurações de Pulo")]
    [SerializeField] private float forcaPulo = 10.0f;
    [SerializeField] private Transform checadorChao; // Arraste o objeto "PeNoChao" aqui
    [SerializeField] private LayerMask camadaChao;   // Selecione a Layer do chão aqui
    [SerializeField] private float raioChecador = 0.2f;
    private bool estaNoChao;

    [Header("Lógica de Corrida (Clique Duplo)")]
    [SerializeField] private float intervaloCliqueDuplo = 0.25f;
    private float tempoUltimoClique;
    private KeyCode ultimaTeclaPressionada;
    private bool estaCorrendo = false;

    [Header("Sons dos biomas")]

    [SerializeField] private AudioClip[] passosNeve;
    [SerializeField] private AudioClip[] passosPedra;
    [SerializeField] private AudioClip[] passosTijolo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        velocidadeAtual = velocidadeCaminhada;
    }

    void Update()
    {
        // 1. Input e Detecção de Chão
        movement = Input.GetAxisRaw("Horizontal");
        Collider2D chaoAtual = Physics2D.OverlapCircle(checadorChao.position,raioChecador,camadaChao);

        estaNoChao = chaoAtual != null;

        // 2. Atualização do Animator
        anim.SetBool("estaNoChao", estaNoChao);
        anim.SetFloat("velocidade", Mathf.Abs(movement));
        anim.SetBool("estaCorrendo", estaCorrendo);

        // 3. Lógica de Pulo (Atualizada com W e Seta para Cima)
        if (estaNoChao && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            // Aplica força vertical mantendo a velocidade horizontal atual
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
            anim.SetTrigger("pular");
            AudioManager.Instancia.PlayOneShotAudio(somPulo);
        }

        DetectarCliqueDuplo();
        VerificarDirecao();

        if (movement != 0 && estaNoChao)
        {
            Debug.Log("Paasos");
            timerPassos += Time.deltaTime;

        if (timerPassos >= intervaloPasso)
        {


            TocarSomPasso(chaoAtual);

            timerPassos = 0f;
        }
        }
        else
        {
            timerPassos = 0f;
        }
    }



    private void TocarSomPasso(Collider2D chaoAtual)
{

    if (chaoAtual == null)
        return;


    AudioClip somEscolhido = null;

    if (chaoAtual.CompareTag("Neve"))
    {
        if (passosNeve.Length > 0)
        {
            somEscolhido = passosNeve[
                Random.Range(0, passosNeve.Length)
            ];
        }
    }
    else if (chaoAtual.CompareTag("Pedras"))
    {
        if (passosPedra.Length > 0)
        {
            somEscolhido = passosPedra[
                Random.Range(0, passosPedra.Length)
            ];
        }
    }
    else if (chaoAtual.CompareTag("Tijolos"))
    {
        if (passosTijolo.Length > 0)
        {
            somEscolhido = passosTijolo[
                Random.Range(0, passosTijolo.Length)
            ];
        }
    }

    if (somEscolhido != null)
    {
        AudioManager.Instancia.PlayOneShotAudio(somEscolhido);
    }
}
    void FixedUpdate()
    {
        // Criamos um Vector2 para a velocidade desejada
        float targetVelocityX = movement * velocidadeAtual;
        
        // Se estivermos a mover, mantemos a velocidade vertical da física (importante para rampas)
        // Adicionamos uma pequena verificação: se o movimento for 0 e estiver no chão, paramos totalmente para não deslizar
        if (movement != 0)
        {
            rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);
        }
        else if (estaNoChao)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void DetectarCliqueDuplo()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || 
            Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            KeyCode teclaAtual = GetCurrentKey();

            if (teclaAtual == ultimaTeclaPressionada && (Time.time - tempoUltimoClique) < intervaloCliqueDuplo)
            {
                estaCorrendo = true;
                velocidadeAtual = velocidadeCorrida;
            }

            tempoUltimoClique = Time.time;
            ultimaTeclaPressionada = teclaAtual;
        }

        if (movement == 0)
        {
            estaCorrendo = false;
            velocidadeAtual = velocidadeCaminhada;
        }
    }

    private KeyCode GetCurrentKey()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) return KeyCode.A;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) return KeyCode.D;
        return KeyCode.None;
    }

    private void VerificarDirecao()
    {
        if (movement > 0 && !olhandoParaDireita)
        {
            Flip();
        }
        else if (movement < 0 && olhandoParaDireita)
        {
            Flip();
        }
    }

    private void Flip()
    {
        olhandoParaDireita = !olhandoParaDireita;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmosSelected()
    {
        if (checadorChao != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checadorChao.position, raioChecador);
        }
    }

    // No script PlayerMovement.cs
    public void TomarDano(float forcaArremesso, Vector2 direcao)
    {
        // Aplica o arremesso (knockback)
        rb.linearVelocity = Vector2.zero; // Reseta a velocidade para o impacto ser limpo
        rb.AddForce(direcao * forcaArremesso, ForceMode2D.Impulse);
        
        // Dispara animação de dano se tiver
        if (anim != null) anim.SetTrigger("tomouDano");
        
        Debug.Log("Kriger foi atingido e arremessado!");
    }
}
