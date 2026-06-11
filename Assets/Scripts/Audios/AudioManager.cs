using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instancia{get; private set;}
    [SerializeField] private AudioSource efeitoSource;

    [SerializeField] private AudioSource musicSource;

    void Awake()
    {
        if(Instancia != null && Instancia != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(this.gameObject);


    }

        public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioManager: Tentou tocar uma música, mas o AudioClip é nulo!");
            return;
        }
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayOneShotAudio(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioManager: Tentou tocar um efeito sonoro, mas o AudioClip é nulo!");
            return;
        }
        efeitoSource.PlayOneShot(clip);
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }
}
