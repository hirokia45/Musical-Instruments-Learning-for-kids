using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    [SerializeField] private AudioClip navClickClip;
    [SerializeField] private float navigationWaitTime = 1f;
    
    private AudioSource _audioSource;
    
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator Navigate(int sceneIndex)
    {
        _audioSource.PlayOneShot(navClickClip);
        yield return new WaitForSeconds(navigationWaitTime);
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadLearnScreen()
    {
        StartCoroutine(Navigate(1));
    }

    public void LoadMainScreen()
    {
        StartCoroutine(Navigate(0));
    }

    public void LoadPlayScreen()
    {
        StartCoroutine(Navigate(2));
    }

    public void LoadQuizScreen()
    {
        StartCoroutine(Navigate(3));
    }

    public void LoadConcertScreen()
    {
        StartCoroutine(Navigate(4));
    }

    public void MoveToPage(int screenIndex)
    {
        StartCoroutine(Navigate(screenIndex));
    }
}
