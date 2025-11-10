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

    private void Start()
    {
        // Привязка кнопок
        newGameButton.onClick.AddListener(() => { PlayClick(); StartNewGame(); });
        continueButton.onClick.AddListener(() => { PlayClick(); ContinueGame(); });
        settingsButton.onClick.AddListener(() => { PlayClick(); OpenSettings(); });
        exitButton.onClick.AddListener(() => { PlayClick(); ExitGame(); });
        backButton.onClick.AddListener(() => { PlayClick(); CloseSettings(); });
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
        settingsPanel.SetActive(true); // включаем панель
    }

    void CloseSettings()
    {
        settingsPanel.SetActive(false); // закрываем панель
    }

    void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
