using UnityEngine;
using UnityEngine.Audio;

public class audioManager : MonoBehaviour
{
    public static audioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip confirmSound;
    public AudioClip backSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()  // Keep the audio playing across scenes
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public void PlayMusic(AudioClip clip, bool loop)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlayMusic called with a null AudioClip.");
            return;
        }

        musicSource.clip = clip;
        musicSource.volume = musicVolume * masterVolume;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlaySFX called with a null AudioClip.");
            return;
        }
        sfxSource.clip = clip;
        sfxSource.volume = sfxVolume * masterVolume;
        sfxSource.Play();
    }

    public void SetMasterVolume(float volume) 
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
    }

    public void UpdateVolumes()
    {
        musicSource.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
    }

    public void PlayConfirmSound()
    {
        PlaySFX(confirmSound);
    }

    public void PlayBackSound()
    {
        PlaySFX(backSound);
    }
}   
