using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    public int floor = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void NextFloor()
    {
        floor++;
    }

    public int EnemyHPBonus() => floor * 2;
}
