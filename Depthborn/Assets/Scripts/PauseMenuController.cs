using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup pausePanel;     // основное меню паузы
    public GameObject settingsPanel;   // панель настроек

    private bool isPaused = false;

    void Start()
    {
        // при старте скрываем всё
        HidePausePanel();
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // если настройки открыты → просто закрыть их
            if (settingsPanel.activeSelf)
            {
                CloseSettings();
                return;
            }

            // стандартное поведение ESC
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    // ---------- ПАУЗА ----------

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        ShowPausePanel();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        HidePausePanel();
        settingsPanel.SetActive(false);
    }

    void ShowPausePanel()
    {
        pausePanel.alpha = 1;
        pausePanel.interactable = true;
        pausePanel.blocksRaycasts = true;
    }

    void HidePausePanel()
    {
        pausePanel.alpha = 0;
        pausePanel.interactable = false;
        pausePanel.blocksRaycasts = false;
    }

    // ---------- НАСТРОЙКИ ----------

    public void OpenSettings()
    {
        HidePausePanel();
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        ShowPausePanel();
    }

    // ---------- ВЫХОД ----------

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
