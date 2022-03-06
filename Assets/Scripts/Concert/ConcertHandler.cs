using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConcertHandler : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private Canvas songSelectCanvas;
    [SerializeField] private Canvas instrumentSelectCanvas;
    [SerializeField] private Canvas instrumentListCanvas;
    [SerializeField] private Canvas playingCanvas;
    [SerializeField] private GameObject listContainer;

    [Header("Config")]
    [SerializeField] private float canvasChangeInterval = 1f;
    private ButtonClickAudio _buttonClickAudio;

    public InstrumentSelectType CurrentSelectType { get; private set; }
    private InstrumentSO[] _selectedInstruments = new InstrumentSO[4];
    private AudioSource[] _audioSources;

    private void Start()
    {
        _buttonClickAudio = FindObjectOfType<ButtonClickAudio>();
        _audioSources = GetComponentsInChildren<AudioSource>();

        instrumentSelectCanvas.gameObject.SetActive(false);
        instrumentListCanvas.gameObject.SetActive(false);
        playingCanvas.gameObject.SetActive(false);
    }

    public void LoadSongSelect()
    {
        _buttonClickAudio.PlayCancelSfx();

        instrumentSelectCanvas.gameObject.SetActive(false);
        songSelectCanvas.gameObject.SetActive(true);
    }
    
    public void LoadInstrumentSelect()
    {
        _buttonClickAudio.PlayCancelSfx();

        songSelectCanvas.gameObject.SetActive(false);
        instrumentListCanvas.gameObject.SetActive(false);
        instrumentSelectCanvas.gameObject.SetActive(true);
    }

    public void LoadInstrumentList(int selectType)
    {
        _buttonClickAudio.PlayCancelSfx();

        InstrumentSelectType type = selectType switch
        {
            0 => InstrumentSelectType.Melody,
            1 => InstrumentSelectType.Harmony,
            2 => InstrumentSelectType.Bass,
            3 => InstrumentSelectType.Percussion,
            _ => InstrumentSelectType.Melody
        };

        CurrentSelectType = type;

        instrumentSelectCanvas.gameObject.SetActive(false);
        instrumentListCanvas.gameObject.SetActive(true);
    }

    public void SelectInstrument(InstrumentSelectType selectType, InstrumentSO instrument)
    {
        int index = GetPartIndex(selectType);

        _selectedInstruments[index] = instrument;
        
        LoadInstrumentSelect();
        UpdateSelectedUI(selectType);
    }

    public void StartConcert()
    {
        _buttonClickAudio.PlayCancelSfx();
        
        instrumentSelectCanvas.gameObject.SetActive(false);
        playingCanvas.gameObject.SetActive(true);

        StartCoroutine(PlayEnsemble());
    }
    
    public void StopConcert()
    {
        _buttonClickAudio.PlayCancelSfx();

        foreach (var audioSource in _audioSources)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        
        playingCanvas.gameObject.SetActive(false);
        instrumentSelectCanvas.gameObject.SetActive(true);

    }

    private IEnumerator PlayEnsemble()
    {
        yield return new WaitForSeconds(canvasChangeInterval);

        for (int i = 0; i < _audioSources.Length; i++)
        {
            if (_selectedInstruments[i] == null) continue;

            AudioClip clipType = i switch
            {
                0 => _selectedInstruments[i].MelodyClip,
                1 => _selectedInstruments[i].HarmonyClip,
                2 => _selectedInstruments[i].BassClip,
                3 => _selectedInstruments[i].PercussionClip,
                _ => throw new ArgumentOutOfRangeException()
            };
            _audioSources[i].clip = clipType;
            _audioSources[i].Play();
        }
    }

    private void UpdateSelectedUI(InstrumentSelectType selectType)
    {
        int index = GetPartIndex(selectType);

        InstrumentSelect[] children = listContainer.GetComponentsInChildren<InstrumentSelect>(true);

        Image[] images = children[index].GetComponentsInChildren<Image>();
        images[1].sprite = _selectedInstruments[index].InstrumentImage;
    }

    private int GetPartIndex(InstrumentSelectType selectType)
    {
        return selectType switch
        {
            InstrumentSelectType.Melody => 0,
            InstrumentSelectType.Harmony => 1,
            InstrumentSelectType.Bass => 2,
            InstrumentSelectType.Percussion => 3,
            _ => 0
        };
    }

}