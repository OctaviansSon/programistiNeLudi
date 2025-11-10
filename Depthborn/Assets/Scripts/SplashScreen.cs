using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 2f;
    public float displayTime = 2f;

    private void Start()
    {
        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        // Fade In
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        // Включаем интерактивность, если нужно
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Задержка отображения
        yield return new WaitForSeconds(displayTime);

        // Fade Out
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        // Переход к следующей сцене
        SceneManager.LoadScene("MainMenu");
    }
}
