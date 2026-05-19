using UnityEngine;

public class NPCInterativo : MonoBehaviour
{
    [SerializeField] private GameObject balaoDiálogo; // Opcional: Um balão visual de "Aperte E"
    private bool playerEstaPerto = false;

    void Update()
    {
        // Se o player estiver perto e apertar a tecla de interação (ex: E)
        if (playerEstaPerto && Input.GetKeyDown(KeyCode.E))
        {
            FalarComNPC();
        }
    }

    private void FalarComNPC()
    {
        Debug.Log("Olá, viajante! Kriger precisa de ajuda...");
        // Aqui depois você vai conectar o seu sistema de interface (UI) de texto
    }

    // Detecta quando o player entrou na área do Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEstaPerto = true;
            if (balaoDiálogo != null) balaoDiálogo.SetActive(true);
        }
    }

    // Detecta quando o player se afastou
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEstaPerto = false;
            if (balaoDiálogo != null) balaoDiálogo.SetActive(false);
        }
    }
}