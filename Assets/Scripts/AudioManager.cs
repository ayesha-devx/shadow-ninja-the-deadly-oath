using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;

    public AudioClip introMusic;
    public AudioClip menuMusic;

    void Start()
    {
        PlayIntroMusic();
    }

    public void PlayIntroMusic()
    {
        musicSource.clip = introMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayMenuMusic()
    {
        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
}