using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class CampSceneController : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup lorePanel;
    public TMP_Text loreText;
    public CanvasGroup choicePanel;
    public TMP_Text choiceText;

    [Header("Audio")]
    public AudioSource loreAudio;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    private void Start()
    {
        // Изначально ChoicePanel скрыта
        choicePanel.alpha = 0;
        choicePanel.interactable = false;
        choicePanel.blocksRaycasts = false;

        StartCoroutine(ShowLore());
    }

    private IEnumerator ShowLore()
    {
        // Fade-in лора
        yield return StartCoroutine(FadeCanvas(lorePanel, 0, 1, fadeDuration));

        // Проигрываем аудио
        loreAudio?.Play();

        // Ждем окончания аудио
        yield return new WaitForSeconds(loreAudio.clip.length);

        // Fade-out лора
        yield return StartCoroutine(FadeCanvas(lorePanel, 1, 0, fadeDuration));

        // Показываем панель выбора пути
        choicePanel.interactable = true;
        choicePanel.blocksRaycasts = true;
        choiceText.text = "Куда пойдёшь?";
        yield return StartCoroutine(FadeCanvas(choicePanel, 0, 1, fadeDuration));
    }

    private IEnumerator FadeCanvas(CanvasGroup group, float from, float to, float duration)
    {
        float t = 0;
        group.alpha = from;
        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        group.alpha = to;
    }
}
