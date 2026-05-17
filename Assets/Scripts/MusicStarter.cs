using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip musica;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("MusicStarter iniciou");
        AudioManager.Instancia.PlayMusic(musica);
    }
}
