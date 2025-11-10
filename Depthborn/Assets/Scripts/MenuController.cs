using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;

    [Header("Buttons")]
    public Button newGameButton;
    public Button continueButton;
    public Button settingsButton;
    public Button exitButton;
    public Button backButton;

    [Header("Audio")]
    public AudioSource buttonClickSFX; // звук кнопки
    public AudioSource musicSource; // музыка
    public Slider musicSlider; // регулятор музыки
    public Slider sfxSlider;   // регулятор SFX

    private void Start()
    {
        // Привязка кнопок
        newGameButton.onClick.AddListener(() => { PlayClick(); StartNewGame(); });
        continueButton.onClick.AddListener(() => { PlayClick(); ContinueGame(); });
        settingsButton.onClick.AddListener(() => { PlayClick(); OpenSettings(); });
        exitButton.onClick.AddListener(() => { PlayClick(); ExitGame(); });
        backButton.onClick.AddListener(() => { PlayClick(); CloseSettings(); });

        // Привязка слайдеров громкости
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Установим начальные значения
        if (musicSource != null && musicSlider != null)
            musicSlider.value = musicSource.volume;

        if (buttonClickSFX != null && sfxSlider != null)
            sfxSlider.value = buttonClickSFX.volume;
    }

    void PlayClick()
    {
        if (buttonClickSFX != null)
            buttonClickSFX.Play();
    }

    void SetMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }

    void SetSFXVolume(float value)
    {
        if (buttonClickSFX != null)
            buttonClickSFX.volume = value;
    }

    void StartNewGame()
    {
        SceneManager.LoadScene("CampScene");
    }

    void ContinueGame()
    {
        SceneManager.LoadScene("CampScene");
    }

    void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
