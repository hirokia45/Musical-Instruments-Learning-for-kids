using System.Collections.Generic;
using UnityEngine;

public class InstrumentData : MonoBehaviour
{
    [Header("Instrument Data")]
    [SerializeField] private List<InstrumentSO> instrumentData = new List<InstrumentSO>();

    public List<InstrumentSO> List => instrumentData;
    static InstrumentData Instance;
    
    void Awake()
    {
        ManageSingleton();
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
}