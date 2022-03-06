using UnityEngine;

public class MuteMenu : MonoBehaviour
{
    private AudioPlayer _audioPlayer;

    private void Start()
    {
        _audioPlayer = FindObjectOfType<AudioPlayer>();

        _audioPlayer.Stop();
    }

    private void OnDestroy()
    {
        _audioPlayer.Stop();
    }
}