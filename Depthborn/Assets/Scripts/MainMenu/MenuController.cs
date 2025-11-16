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
    public AudioSource buttonClickSFX;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // ---- КНОПКИ ----
        newGameButton.onClick.AddListener(() => { PlayClick(); StartNewGame(); });
        continueButton.onClick.AddListener(() => { PlayClick(); ContinueGame(); });
        settingsButton.onClick.AddListener(() => { PlayClick(); OpenSettings(); });
        exitButton.onClick.AddListener(() => { PlayClick(); ExitGame(); });
        backButton.onClick.AddListener(() => { PlayClick(); CloseSettings(); });

        // ---- ГРОМКОВСТЬ ИЗ АУДИО МЕНЕДЖЕРА ----
        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            musicSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetMusicVolume(v));
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            sfxSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetSFXVolume(v));
        }
    }

    void PlayClick()
    {
        if (buttonClickSFX != null)
            buttonClickSFX.Play();
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
