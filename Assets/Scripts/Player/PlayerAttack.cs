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

        // Detecção de inimigos
        Collider2D[] inimigosAtingidos = Physics2D.OverlapCircleAll(pontoDeAtaque.position, raioDoAtaque, layerInimigo);
        foreach (Collider2D inimigo in inimigosAtingidos)
        {
            Debug.Log("Kriger acertou o combo " + comboPasso + " em: " + inimigo.name);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pontoDeAtaque == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pontoDeAtaque.position, raioDoAtaque);
    }
}