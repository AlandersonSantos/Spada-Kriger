using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startPos;

    private Transform cam;

    [Range(0,1)]
    public float parallaxFactor;

    void Start()
    {
        // procura automaticamente a câmera principal
        cam = Camera.main.transform;

        startPos = transform.position.x;

        length = GetComponent<SpriteRenderer>()
            .bounds.size.x;
    }

    void LateUpdate()
    {
        if(cam == null) return;

        float dist = cam.position.x *
                     (1 - parallaxFactor);

        transform.position =
            new Vector3(
                startPos + dist,
                transform.position.y,
                transform.position.z
            );
    }
}