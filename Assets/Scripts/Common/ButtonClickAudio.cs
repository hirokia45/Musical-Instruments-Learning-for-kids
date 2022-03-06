using UnityEngine;

public class ButtonClickAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip cancelClip;
    
    private AudioPlayer _audioPlayer;
    
    private void Start()
    {
        _audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    public void PlaySelectSfx()
    {
        _audioPlayer.Play(clickClip);
    }

    public void PlayCancelSfx()
    {
        _audioPlayer.Play(cancelClip);
    }
}