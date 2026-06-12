using UnityEngine;

public class InimigoPatrulha : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float velocidade = 2f;

    public Transform pontoA;
    public Transform pontoB;

    private Transform destinoAtual;

    private Rigidbody2D rb;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        destinoAtual = pontoB;
    }

    void Update()
    {
        Patrulhar();
    }

    void Patrulhar()
    {
        if (
            pontoA == null ||
            pontoB == null
        )
            return;

        Vector2 direcao =
            (
                destinoAtual.position -
                transform.position
            ).normalized;

        rb.linearVelocity =
            new Vector2(
                direcao.x * velocidade,
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

        float distancia =
            Vector2.Distance(
                transform.position,
                destinoAtual.position
            );

        if (distancia < 0.5f)
        {
            destinoAtual =
                destinoAtual == pontoA
                ? pontoB
                : pontoA;

            Flip();

            Debug.Log(
                "Novo destino: "
                + destinoAtual.name
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
}