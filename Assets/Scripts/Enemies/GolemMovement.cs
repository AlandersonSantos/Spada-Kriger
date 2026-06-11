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
    [SerializeField] private float raioPercepcao = 3f;

    [SerializeField] private float cooldownAtaque = 1.5f;

    [SerializeField] private int danoAtaque = 2;

    private float tempoUltimoAtaque;

    private Transform player;

    private HealthSystem playerHealth;

    private Rigidbody2D rb;

    private Animator anim;

    private bool estaMorto = false;

    [Header("Dano do Player")]
    public Animator playerAnimator;

    public string ataque1 = "attack1";
    public string ataque2 = "attack2";

    public float danoRecebido = 1f;

    private bool podeReceberDano = true;

    [Header("Drop de Vida")]
    [SerializeField] private GameObject hpDropPrefab;

    [Range(0, 100)]
    [SerializeField] private int chanceDropHP = 30;

    [Header("Dano por Contato")]
    [SerializeField] private float danoAoContato = 1f;

    [SerializeField] private float forcaArremesso = 8f;

    [Header("Áudio passos")]
    [SerializeField] private AudioClip somPassoEnemy;
    private float timerPassosEnemy;
    [SerializeField] private float intervaloPassoEnemy = 0.5f;

    [Header("Áudio Attack")]
    [SerializeField] private AudioClip somAttackEnemy;

    private void Awake()
    {
        rb =
            GetComponent<Rigidbody2D>();

        anim =
            GetComponent<Animator>();
    }

    void Start()
    {
        vidaAtual =
            vidaMaxima;

        destinoAtual =
            pontoB;

        GameObject playerObj =
            GameObject.FindGameObjectWithTag(
                "Player"
            );

        if (
            playerObj != null
        )
        {
            player =
                playerObj.transform;

            playerHealth =
                playerObj.GetComponent<
                    HealthSystem
                >();

            if (
                playerAnimator ==
                null
            )
            {
                playerAnimator =
                    playerObj.GetComponent<
                        Animator
                    >();
            }
        }
    }

    void Update()
    {
        if (
            estaMorto ||
            player == null
        )
            return;

        float distanciaParaPlayer =
            Vector2.Distance(
                transform.position,
                player.position
            );

        if (
            distanciaParaPlayer <=
            raioPercepcao
        )
        {
            if (
                Time.time >=
                tempoUltimoAtaque +
                cooldownAtaque
            )
            {
                AtacarPlayer();
            }
            else
            {
                rb.linearVelocity =
                    new Vector2(
                        0,
                        rb.linearVelocity.y
                    );

                if (anim != null)
                {
                    anim.SetBool(
                        "estaCaminhando",
                        false
                    );
                }
            }
        }
        else
        {
            Patrulhar();
        }
    }

    void Patrulhar()
    {
        Vector2 direcao =
            (
                destinoAtual.position -
                transform.position
            ).normalized;

        rb.linearVelocity =
            new Vector2(
                direcao.x *
                velocidade,

                rb.linearVelocity.y
            );

        if (anim != null)
        {
            anim.SetBool(
                "estaCaminhando",

                Mathf.Abs(
                    rb.linearVelocity.x
                ) > 0.1f
            );
        }

        timerPassosEnemy += Time.deltaTime;
        
         if (timerPassosEnemy >= intervaloPassoEnemy)
        {
            AudioManager.Instancia.PlayOneShotAudio(somPassoEnemy);
            timerPassosEnemy = 0f;
        }

        if (
            Vector2.Distance(
                transform.position,
                destinoAtual.position
            ) < 0.5f
        )
        {
            destinoAtual =
                (
                    destinoAtual ==
                    pontoA
                )
                ? pontoB
                : pontoA;

            Flip();
        }
    }

    void AtacarPlayer()
    {
        tempoUltimoAtaque =
            Time.time;

        rb.linearVelocity =
            new Vector2(
                0,
                rb.linearVelocity.y
            );

        if (anim != null)
        {
            anim.SetBool(
                "estaCaminhando",
                false
            );

            anim.SetTrigger(
                "atacar"
            );

            AudioManager.Instancia.PlayOneShotAudio(somAttackEnemy);
        }

        if (
            playerHealth != null
        )
        {
            playerHealth
            .ChangeHealth(
                -danoAtaque
            );

            Debug.Log(
                "Inimigo causou "
                + danoAtaque +
                " de dano"
            );
        }
    }

    void Flip()
    {
        Vector3 escala =
            transform.localScale;

        escala.x *= -1;

        transform.localScale =
            escala;
    }

    public void TomarDano(
        float dano
    )
    {
        if (
            estaMorto
        )
            return;

        vidaAtual -= dano;

        Debug.Log(
            "Vida restante: "
            + vidaAtual
        );

        if (anim != null)
        {
            anim.SetTrigger(
                "tomouDano"
            );
        }

        if (
            vidaAtual <= 0
        )
        {
            Morrer();
        }
    }

    void Morrer()
{
    estaMorto = true;

    if (
        hpDropPrefab != null &&
        Random.Range(0, 100) < chanceDropHP
    )
    {
        Instantiate(
            hpDropPrefab,
            transform.position +
            new Vector3(
                0f,
                1.5f,
                0f
            ),
            Quaternion.identity
        );
    }

    rb.linearVelocity =
        Vector2.zero;

    rb.bodyType =
        RigidbodyType2D.Static;

    if (anim != null)
    {
        anim.SetTrigger(
            "morreu"
        );
    }

    Destroy(
        gameObject,
        2f
    );
}

    private void OnTriggerStay2D(
        Collider2D other
    )
    {
        if (
            estaMorto
        )
            return;

        if (
            !other.CompareTag(
                "Player"
            )
        )
            return;

        if (
            playerAnimator ==
            null
        )
            return;

        AnimatorStateInfo state =
            playerAnimator
            .GetCurrentAnimatorStateInfo(
                0
            );

        bool atacando =
            state.IsName(
                ataque1
            )
            ||
            state.IsName(
                ataque2
            );

        if (
            atacando &&
            podeReceberDano
        )
        {
            TomarDano(
                danoRecebido
            );

            podeReceberDano =
                false;

            Invoke(
                nameof(
                    ResetarDano
                ),
                0.5f
            );
        }
    }

    void ResetarDano()
    {
        podeReceberDano =
            true;
    }

    private void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        if (
            collision.gameObject
            .CompareTag(
                "Player"
            )
        )
        {
            foreach (
                ContactPoint2D ponto
                in collision.contacts
            )
            {
                if (
                    ponto.normal.y <
                    -0.5f
                )
                {
                    PlayerMovement
                    playerScript =
                    collision.gameObject
                    .GetComponent<
                        PlayerMovement
                    >();

                    if (
                        playerScript !=
                        null
                    )
                    {
                        Vector2 direcao =
                            new Vector2(
                                ponto.normal.x
                                * -1,

                                1
                            )
                            .normalized;

                        playerScript
                        .TomarDano(
                            forcaArremesso,
                            direcao
                        );
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color =
            Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            raioPercepcao
        );
    }
}