using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [Header("Enemy Sounds")]
    public AudioClip spawnSound;
    public AudioClip hitSound;
    public AudioClip deathSound;

    void Start()
    {
        Play(spawnSound);
    }

    public void PlayHit()
    {
        Play(hitSound);
    }

    public void PlayDeath()
    {
        Play(deathSound);
    }

    void Play(AudioClip clip)
    {
        if (AudioManager.Instance == null) return;
        AudioManager.Instance.PlaySFX(clip);
    }
}
