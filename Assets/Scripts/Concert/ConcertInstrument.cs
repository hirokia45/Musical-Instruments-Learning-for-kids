using System;
using UnityEngine;

public class ConcertInstrument : MonoBehaviour
{
    public AudioSource InstrumentAudio;

    private InstrumentSO _instrument;

    private void Start()
    {
        InstrumentAudio = GetComponent<AudioSource>();
    }

    public void SetInstrument(InstrumentSO instrument)
    {
        _instrument = instrument;
    }
}