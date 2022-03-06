using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentDetail : MonoBehaviour
{
    [Header("Canvas Refs")]
    [SerializeField] private GameObject instrumentDetailCanvas;
    [SerializeField] private GameObject instrumentListCanvas;

    [Header("Detail UI")]
    [SerializeField] private TextMeshProUGUI instrumentTitle;
    [SerializeField] private TextMeshProUGUI instrumentDetail;
    [SerializeField] private Image instrumentImage;
    
    private InstrumentSO _currentInstrument;
    private AudioPlayer _audioPlayer;
    private InstrumentData _instrumentData;
    private ButtonClickAudio _buttonClickAudio;
     
    private void Start()
    {
        instrumentDetailCanvas.SetActive(false);
        _audioPlayer = FindObjectOfType<AudioPlayer>();
        _instrumentData = FindObjectOfType<InstrumentData>();
        _buttonClickAudio = FindObjectOfType<ButtonClickAudio>();
    }
    
    public void SelectInstrument(string value)
    {
        _buttonClickAudio.PlaySelectSfx();
        
        Instrument instrument = (Instrument)Enum.Parse(typeof(Instrument), value);
        _currentInstrument = _instrumentData.List.Find(item => item.InstrumentType == instrument);

        ShowInstrumentDetail();
    }

    public void CloseDetail()
    {
        _audioPlayer.Stop();
        _buttonClickAudio.PlayCancelSfx();
        
        _currentInstrument = null;
        instrumentListCanvas.SetActive(true);
        instrumentDetailCanvas.SetActive(false);
    }

    private void ShowInstrumentDetail()
    {
        if (_currentInstrument == null) return;
        
        instrumentListCanvas.SetActive(false);
        instrumentDetailCanvas.SetActive(true);
        
        instrumentTitle.text = _currentInstrument.InstrumentName;
        instrumentDetail.text = _currentInstrument.InstrumentDescription;
        instrumentImage.sprite = _currentInstrument.InstrumentImage;
    }

    public void PlaySampleAudio(int index)
    {
        _audioPlayer.Play(_currentInstrument.InstrumentClips[index], true);
    }
}