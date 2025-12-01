using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        if (panel != null) panel.SetActive(false);
    }

    public void Show()
    {
        if (panel != null) panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
