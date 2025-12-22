using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup canvasGroup;

    [Header("Buttons")]
    public Button retryButton;
    public Button mainMenuButton;

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        Hide();
    }

    void Start()
    {
        // жёсткая привязка кнопок
        if (retryButton != null)
            retryButton.onClick.AddListener(Retry);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(MainMenu);
    }

    public void Show()
    {
        Time.timeScale = 0f;

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
