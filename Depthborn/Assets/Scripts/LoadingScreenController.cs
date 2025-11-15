using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreenController : MonoBehaviour
{
    public TMP_Text loadingText;
    public TMP_Text tipsText;
    public Image loadingIcon;

    public float iconRotateSpeed = 180f;

    string[] tips = {
        "Совет: уворачивайтесь от атак, как в Hades!",
        "Совет: Исследуй каждую комнату — лут может спасти тебе жизнь.",
        "Помни: духовный путь закрывает доступ к оружию людей.",
        "Беженцы не дают связи с духами, но дают стабильность.",
        "Не спеши — каждый выбор меняет игру."
    };

    void Start()
    {
        StartCoroutine(AnimateLoading());
    }

    void Update()
    {
        // вращаем иконку
        loadingIcon.transform.Rotate(0, 0, iconRotateSpeed * Time.deltaTime);
    }

    IEnumerator AnimateLoading()
    {
        tipsText.text = tips[Random.Range(0, tips.Length)];

        string targetScene = PlayerPrefs.GetString("NextScene");
        if (string.IsNullOrEmpty(targetScene)) targetScene = "MainMenu";

        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
        op.allowSceneActivation = false;

        // анимация текста «Загрузка…»
        while (op.progress < 0.9f)
        {
            loadingText.text = "Загрузка" + new string('.', Mathf.FloorToInt(Time.time % 3) + 1);
            yield return null;
        }

        // задержка для эффекта
        yield return new WaitForSeconds(0.5f);

        op.allowSceneActivation = true;
    }
}
