using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class audioManager : MonoBehaviour
{
    public static audioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("SFX")]
    public AudioClip confirmSound;
    public AudioClip backSound;
    public AudioClip menuOpenSound;

    [Header("Music")]
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;


    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 0.75f;
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Sliders")]

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Awake()  // Keep the audio playing across scenes
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject); 
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
        sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value / 100f);
        UpdateVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value / 100f);
        UpdateVolumes();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value / 100f);
        UpdateVolumes();
    }

    public void UpdateVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }

    public void PlayConfirmSound()
    {
        PlaySFX(confirmSound);
    }

    public void PlayBackSound()
    {
        PlaySFX(backSound);
    }
    public void PlayMenuOpenSound()
    {
        PlaySFX(menuOpenSound);
    }
    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic, true);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameMusic, true);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    //method for syncing audio sliders

    public void SyncAudioSliders(Slider master, Slider music, Slider sfx)

        //Note: SetValueWithoutNotify is used so i can to set the value of the slider without triggering the onValueChanged event
    {
        if (master != null) master.SetValueWithoutNotify(masterVolume * 100f);
        if (music != null) music.SetValueWithoutNotify(musicVolume * 100f);
        if (sfx != null) sfx.SetValueWithoutNotify(sfxVolume * 100f);
    }

}   
