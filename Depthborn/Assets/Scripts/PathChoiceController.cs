using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PathChoiceController : MonoBehaviour
{
    [Header("Options")]
    public TMP_Text spiritOption;
    public TMP_Text refugeeOption;
    public Image selector; // сюда можно кинуть кастомный 2D спрайт
    public TMP_Text descriptionText;

    [Header("Settings")]
    public float fadeDuration = 1.0f;

    private int selectedIndex = 0;
    private bool choiceMade = false;

    private string spiritDesc = "Вы пойдёте к духам. Лут братьев недоступен, но боссы будут Hades-стиль.";
    private string humanDesc = "Вы пойдёте с беженцами. Связь с духами навсегда потеряна, только Hades-способности.";

    void Start()
    {
        UpdateSelector();
        UpdateDescription();
    }

    void Update()
    {
        if (choiceMade) return;

        // Навигация через клавиши
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex = 0;
            UpdateSelector();
            UpdateDescription();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex = 1;
            UpdateSelector();
            UpdateDescription();
        }

        // Подтверждение выбора
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            choiceMade = true;
            if (selectedIndex == 0)
            {
                GameManager.Instance.SetPathChoice(PathChoice.Spirit);
                StartCoroutine(LoadNextScene("SpiritPathScene"));
            }
            else
            {
                GameManager.Instance.SetPathChoice(PathChoice.Human);
                StartCoroutine(LoadNextScene("HumanPathScene"));
            }
        }
    }

    void UpdateSelector()
    {
        // Меняем позицию селектора к выбранному варианту
        selector.transform.position = selectedIndex == 0 ? spiritOption.transform.position + Vector3.left * 30f
                                                          : refugeeOption.transform.position + Vector3.left * 30f;
    }

    void UpdateDescription()
    {
        descriptionText.text = selectedIndex == 0 ? spiritDesc : humanDesc;
    }

    IEnumerator LoadNextScene(string sceneName)
    {
        // fade-out панели выбора
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg != null)
            yield return StartCoroutine(FadeCanvas(cg, 1, 0, fadeDuration));

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeCanvas(CanvasGroup group, float from, float to, float duration)
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
