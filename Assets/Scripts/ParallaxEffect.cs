using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam;
    [Range(0, 1)] public float parallaxFactor; // 0 = segue 100%, 1 = não se move

    void Start()
    {
        startPos = transform.position.x;
        // Pega a largura do sprite para repetir o fundo se necessário
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Usamos LateUpdate para sincronizar com a câmera do Kriger
    void LateUpdate()
    {
        float dist = (cam.transform.position.x * (1 - parallaxFactor));
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}