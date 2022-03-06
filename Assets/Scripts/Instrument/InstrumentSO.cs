using UnityEngine;

[CreateAssetMenu(menuName = "Instrument Detail", fileName = "New Instrument")]
public class InstrumentSO : ScriptableObject
{
    [Header("Core Data")]
    [SerializeField] private Instrument instrumentType;
    [SerializeField] private Family instrumentFamily;
    [SerializeField] private string instrumentName;
    [TextArea(2, 6)] [SerializeField] private string instrumentDescription;
    [SerializeField] private Sprite instrumentImage;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] instrumentClips = new AudioClip[3];
    [SerializeField] private AudioClip melodyClip;
    [SerializeField] private AudioClip harmonyClip;
    [SerializeField] private AudioClip bassClip;
    [SerializeField] private AudioClip percussionClip;

    [Header("Filter Data")]
    [SerializeField] private bool pichted;
    [SerializeField] private bool bass;

    public Instrument InstrumentType => instrumentType;
    public Family InstrumentFamily => instrumentFamily;
    public string InstrumentName => instrumentName;
    public string InstrumentDescription => instrumentDescription;
    public Sprite InstrumentImage => instrumentImage;
    public AudioClip[] InstrumentClips => instrumentClips;
    public AudioClip MelodyClip => melodyClip;
    public AudioClip HarmonyClip => harmonyClip;
    public AudioClip BassClip => bassClip;
    public AudioClip PercussionClip => percussionClip;
    public bool Pitched => pichted;
    public bool Bass => bass;
}