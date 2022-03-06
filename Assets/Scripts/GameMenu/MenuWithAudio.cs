using System;
using UnityEngine;

public class MenuWithAudio : MonoBehaviour
{
    private AudioPlayer _audioPlayer;

    private void Start()
    {
        _audioPlayer = FindObjectOfType<AudioPlayer>();
        _audioPlayer.PlayMainBgm();
    }
}