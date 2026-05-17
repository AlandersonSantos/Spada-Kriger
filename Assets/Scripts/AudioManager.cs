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
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayOneShotAudio(AudioClip clip)
    {
        efeitoSource.PlayOneShot(clip);
    }

     public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }
}
