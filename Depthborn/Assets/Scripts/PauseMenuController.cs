using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup pausePanel;
    public GameObject settingsPanel;

    [Header("Buttons")]
    public Button resumeButton;
    public Button settingsButton;
    public Button mainMenuButton;

    private bool isPaused = false;

    void Start()
    {
        // Кнопки
        resumeButton.onClick.AddListener(ResumeGame);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Если открыты настройки — закрываем
            if (settingsPanel.activeSelf)
            {
                CloseSettings();
                return;
            }

            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;

        pausePanel.alpha = 1;
        pausePanel.interactable = true;
        pausePanel.blocksRaycasts = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;

        pausePanel.alpha = 0;
        pausePanel.interactable = false;
        pausePanel.blocksRaycasts = false;

        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        pausePanel.alpha = 0;
        pausePanel.interactable = false;
        pausePanel.blocksRaycasts = false;

        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        pausePanel.alpha = 1;
        pausePanel.interactable = true;
        pausePanel.blocksRaycasts = true;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
