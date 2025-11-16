using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioSource musicSource;

    private void Start()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }

    void SetSFXVolume(float value)
    {
        // сюда можно подключить глобальный AudioManager для эффектов
        Debug.Log("SFX volume: " + value);
    }
}
