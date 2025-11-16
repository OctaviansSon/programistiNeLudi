using UnityEngine; // ОБЯЗАТЕЛЬНО
using System.Collections;

public enum PathChoice { None, Spirit, Human }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PathChoice pathChoice = PathChoice.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void SetPathChoice(PathChoice choice)
    {
        pathChoice = choice;
        if (choice == PathChoice.Spirit)
        {
            Debug.Log("Игрок лишен оружия братьев до конца игры");
            // Здесь можно выключить Isaac-бафы
        }
        else if (choice == PathChoice.Human)
        {
            Debug.Log("Игрок теряет связь с духами навсегда");
            // Здесь можно отключить духов
        }
    }
}
