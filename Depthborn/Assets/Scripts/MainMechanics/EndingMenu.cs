using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingMenu : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup canvas;

    [Header("Buttons")]
    public Button continueButton;
    public Button endGameButton;

    void Awake()
    {
        if (canvas == null)
            canvas = GetComponent<CanvasGroup>();

        Hide();

        // 🔗 ЯВНО ПРИВЯЗЫВАЕМ КНОПКИ
        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueRun);

        if (endGameButton != null)
            endGameButton.onClick.AddListener(EndGame);
    }

    public void Show()
    {
        Time.timeScale = 0f;
        canvas.alpha = 1f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }

    // ====== BUTTON ACTIONS ======

    public void ContinueRun()
    {
        Time.timeScale = 1f;

        // 1) сохраняем статы игрока (если есть)
        var ph = FindFirstObjectByType<PlayerHealth>();
        var ps = FindFirstObjectByType<PlayerStats>();
        if (RunManager.Instance != null && (ph != null || ps != null))
        {
            RunManager.Instance.SavePlayerStats(ph, ps);
        }

        // 2) апдейт прогресса
        RunManager.Instance.NextFloor();

        // 3) перезагружаем сцену (PlayerSpawner восстановит статы)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
