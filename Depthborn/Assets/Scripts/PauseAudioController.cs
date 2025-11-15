using UnityEngine;
using UnityEngine.UI;

public class PauseAudioController : MonoBehaviour
{
    public Button playPauseButton;
    public Button skipButton;

    public Sprite playIcon;
    public Sprite pauseIcon;
    public Image playPauseImage;

    void Start()
    {
        playPauseButton.onClick.AddListener(TogglePlayPause);
        skipButton.onClick.AddListener(SkipTrack);

        UpdateIcon();
    }

    void TogglePlayPause()
    {
        AudioManager.Instance.ToggleMusicPause();
        UpdateIcon();
    }

    void SkipTrack()
    {
        AudioManager.Instance.PlayRandomTrack();
        UpdateIcon();
    }

    void UpdateIcon()
    {
        playPauseImage.sprite =
            AudioManager.Instance.IsPlaying() ? pauseIcon : playIcon;
    }
}
