using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentList : MonoBehaviour
{
    [SerializeField] private GameObject familySelection;
    [SerializeField] private GameObject instrumentList;

    [SerializeField] private GameObject gridContainer;

    private Family? _currentFamily;
    private InstrumentData _instrumentData;
    private InstrumentDetail _instrumentDetail;
    private ButtonClickAudio _buttonClickAudio;

    private void Start()
    {
        instrumentList.SetActive(false);
        _instrumentData = FindObjectOfType<InstrumentData>();
        _instrumentDetail = FindObjectOfType<InstrumentDetail>();
        _buttonClickAudio = FindObjectOfType<ButtonClickAudio>();
    }

    public void SelectFamily(int familyIndex)
    {
        _buttonClickAudio.PlaySelectSfx();
        _currentFamily = familyIndex switch
        {
            0 => Family.Wind,
            1 => Family.Strings,
            2 => Family.Percussion,
            _ => Family.Wind
        };

        ShowInstrumentList();
    }

    public void CloseList()
    {
        _buttonClickAudio.PlayCancelSfx();
        _currentFamily = null;
        familySelection.SetActive(true);
        instrumentList.SetActive(false);
    }

    private void ShowInstrumentList()
    {
        familySelection.SetActive(false);
        instrumentList.SetActive(true);

        SetChildrenButtons();
    }

    private void SetChildrenButtons()
    {
        List<InstrumentSO> filteredInstruments =
            _instrumentData.List.Where(item => item.InstrumentFamily == _currentFamily).ToList();

        InstrumentSelect[] children = gridContainer.GetComponentsInChildren<InstrumentSelect>();

        foreach (var child in children.Select((value, index) => new {value, index}))
        {
            InstrumentSO instrument = filteredInstruments[child.index];

            TextMeshProUGUI title = child.value.GetComponentInChildren<TextMeshProUGUI>();
            title.text = instrument.InstrumentName;

            Image[] images = child.value.GetComponentsInChildren<Image>();
            images[1].sprite = instrument.InstrumentImage;

            Button button = child.value.GetComponentInChildren<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate
            {
                _instrumentDetail.SelectInstrument(instrument.InstrumentType.ToString());
            });
        }
    }
}