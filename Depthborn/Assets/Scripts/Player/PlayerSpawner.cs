using UnityEngine;
using System;

public class PlayerSpawner : MonoBehaviour
{
    public static event Action<PlayerHealth> OnPlayerSpawned;
    public GameObject playerPrefab;

    static GameObject existingPlayer;

    void Start()
    {
        if (existingPlayer != null)
        {
            // игрок уже есть → просто сообщаем системам
            PlayerHealth ph = existingPlayer.GetComponent<PlayerHealth>();
            OnPlayerSpawned?.Invoke(ph);
            return;
        }

        GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        player.tag = "Player";
        DontDestroyOnLoad(player);

        existingPlayer = player;

        PlayerHealth phNew = player.GetComponent<PlayerHealth>();
        OnPlayerSpawned?.Invoke(phNew);
    }
}
