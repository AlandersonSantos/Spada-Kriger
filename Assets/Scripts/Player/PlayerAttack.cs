using UnityEngine;

public class PlayerAtaque : MonoBehaviour
{
    public Animator anim;
    public Transform pontoDeAtaque;
    public float raioDoAtaque = 0.5f;
    public LayerMask layerInimigo;

    private int comboPasso = 0; 
    private float tempoUltimoClique; 
    public float janelaCombo = 0.7f; 

    void Update()
    {
        // Reseta o combo se passar o tempo da janela
        if (Time.time - tempoUltimoClique > janelaCombo)
        {
            comboPasso = 0;
            anim.SetInteger("comboPasso", 0);
        }

        // Agora aceita Fire1 (Mouse/Ctrl) OU a tecla Z
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Z))
        {
            Atacar();
        }
    }

    void Atacar()
    {
        tempoUltimoClique = Time.time;
        comboPasso++;

        if (comboPasso > 2)
        {
            comboPasso = 1;
        }

        anim.SetInteger("comboPasso", comboPasso);
        anim.SetTrigger("atacar");

        // 1. Detecta todos os objetos na área do círculo de ataque
        Collider2D[] inimigosAtingidos = Physics2D.OverlapCircleAll(pontoDeAtaque.position, raioDoAtaque, layerInimigo);

        // 2. Loop para aplicar dano a cada inimigo detectado
        foreach (Collider2D colisor in inimigosAtingidos)
        {
            // Tenta encontrar o script do inimigo no objeto atingido
            InimigoSimples inimigo = colisor.GetComponent<InimigoSimples>();

            if (inimigo != null)
            {
                // Aplica 1 de dano (ou o valor que você desejar)
                inimigo.TomarDano(1);
                Debug.Log("Kriger acertou o inimigo: " + colisor.name);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pontoDeAtaque == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pontoDeAtaque.position, raioDoAtaque);
    }
}