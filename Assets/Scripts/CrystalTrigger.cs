using UnityEngine;

public class ArmadilhaEspinho : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    [SerializeField] private Transform pontoInicio; // Arraste o objeto PontoInicio aqui

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se quem entrou no trigger foi o Player
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Kriger caiu nos espinhos!");
            
            // Teletransporta o player de volta ao início
            ResetarPlayer(collision.gameObject);
        }
    }

    private void ResetarPlayer(GameObject player)
    {
        if (pontoInicio != null)
        {
            // Pegamos o Rigidbody do player para zerar a velocidade acumulada da queda
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
            }

            // Move a posição do player para o ponto inicial
            player.transform.position = pontoInicio.position;
        }
        else
        {
            Debug.LogWarning("O Ponto de Início não foi atribuído no script de espinhos!");
        }
    }
}