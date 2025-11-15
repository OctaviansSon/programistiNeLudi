using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Playlist")]
    public AudioClip[] musicPlaylist;

    private int currentTrack = -1;
    private bool isAutoPlayEnabled = true; // <<<<< ВАЖНО

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadVolumes();
        StartCoroutine(MusicLoop());
    }

    // -------------------------------------
    // MUSIC LOOP WITH PAUSE SUPPORT
    // -------------------------------------

    IEnumerator MusicLoop()
    {
        while (true)
        {
            if (isAutoPlayEnabled && !musicSource.isPlaying)
            {
                PlayRandomTrack();
            }
            yield return null;
        }
    }

    public void ToggleMusicPause()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
            isAutoPlayEnabled = false;
        }
        else
        {
            musicSource.UnPause();
            isAutoPlayEnabled = true;
        }
    }

    public bool IsPlaying() => musicSource.isPlaying;

    public void PlayRandomTrack()
    {
        if (musicPlaylist.Length == 0) return;

        int newTrack;
        do
        {
            newTrack = Random.Range(0, musicPlaylist.Length);
        }
        while (newTrack == currentTrack && musicPlaylist.Length > 1);

        currentTrack = newTrack;
        musicSource.clip = musicPlaylist[currentTrack];
        musicSource.Play();
    }

    // -------------------------------------
    // VOLUME SAVE / LOAD
    // -------------------------------------

    public void SetMusicVolume(float v)
    {
        musicSource.volume = v;
        PlayerPrefs.SetFloat("MusicVolume", v);
    }

    public void SetSFXVolume(float v)
    {
        sfxSource.volume = v;
        PlayerPrefs.SetFloat("SFXVolume", v);
    }

    void LoadVolumes()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
}
