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
        estaNoChao = Physics2D.OverlapCircle(checadorChao.position, raioChecador, camadaChao);

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
        }

        DetectarCliqueDuplo();
        VerificarDirecao();
    }

    void FixedUpdate()
    {
        // 4. Movimentação Física
        rb.linearVelocity = new Vector2(movement * velocidadeAtual, rb.linearVelocity.y);
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
}