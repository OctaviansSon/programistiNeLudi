using UnityEngine;
using UnityEngine.UI;

public class PauseSettingsController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    public Button backButton;
    public PauseMenuController pauseMenu;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", AudioManager.Instance.musicSource.volume);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", AudioManager.Instance.sfxSource.volume);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        backButton.onClick.AddListener(() => pauseMenu.CloseSettings());
    }

    void SetMusicVolume(float v)
    {
        AudioManager.Instance.SetMusicVolume(v);
    }

    void SetSFXVolume(float v)
    {
        AudioManager.Instance.SetSFXVolume(v);
    }
}
