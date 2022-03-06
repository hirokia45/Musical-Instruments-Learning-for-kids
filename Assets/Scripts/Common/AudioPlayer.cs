using System;
using UnityEngine;


public class AudioPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    static AudioPlayer Instance;
    
    void Awake()
    {
        ManageSingleton();
        _audioSource = GetComponent<AudioSource>();
    }

    private void ManageSingleton()
    {
        if (Instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMainBgm()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void Stop()
    {
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
    }

    public void Play(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void Play(AudioClip clip, bool isSingle)
    {
        if (isSingle && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        Play(clip);
    }
}