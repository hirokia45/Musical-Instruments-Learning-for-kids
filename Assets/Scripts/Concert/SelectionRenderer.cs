using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionRenderer : MonoBehaviour
{
    [SerializeField] private GameObject gridContainer;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private int itemsPerPage = 8;

    private ConcertHandler _concertHandler;
    private InstrumentData _instrumentData;
    private ButtonClickAudio _buttonClickAudio;

    private List<InstrumentSO> _renderingItems = new List<InstrumentSO>();
    
    private int _currPage;

    private int TotalItems => _renderingItems.Count;
    private int TotalPages => (TotalItems / itemsPerPage);
    private bool IsLastPage => _currPage == TotalPages;

    private void Awake()
    {
        _concertHandler = FindObjectOfType<ConcertHandler>();
        _instrumentData = FindObjectOfType<InstrumentData>();
        _buttonClickAudio = FindObjectOfType<ButtonClickAudio>();
    }

    private void Start()
    {
        ResetNavButton();
    }

    private void OnEnable()
    {
        _currPage = 0;
        InstrumentSelectType selectType = _concertHandler.CurrentSelectType;

        _renderingItems = selectType switch
        {
            InstrumentSelectType.Melody => _instrumentData.List.Where(item => item.Pitched).ToList(),
            InstrumentSelectType.Harmony => _instrumentData.List.Where(item => item.Pitched).ToList(),
            InstrumentSelectType.Bass => _instrumentData.List.Where(item => item.Bass).ToList(),
            InstrumentSelectType.Percussion => _instrumentData.List.Where(item => !item.Pitched).ToList(),
            _ => _renderingItems
        };

        if (HasNextPage()) nextButton.gameObject.SetActive(true);
        if (TotalItems > 0) RenderItems();
    }

    private void OnDisable()
    {
        _renderingItems.Clear();
        ResetNavButton();
    }

    public void PrevPage()
    {
        _buttonClickAudio.PlayCancelSfx();
        _currPage--;
        nextButton.gameObject.SetActive(true);
        RenderButton(prevButton, HasPrevPage());
        RenderItems();
    }

    public void NextPage()
    {
        _buttonClickAudio.PlayCancelSfx();
        _currPage++;
        prevButton.gameObject.SetActive(true);
        RenderButton(nextButton, HasNextPage());
        RenderItems();
    }

    private void RenderItems()
    {
        int numbersToRender;
        int startIndex = _currPage * itemsPerPage;
        
        if (IsLastPage)
        {
            numbersToRender = TotalItems % itemsPerPage;
        }
        else
        {
            numbersToRender = itemsPerPage;
        }

        List<InstrumentSO> pageItems = _renderingItems.GetRange(startIndex, numbersToRender);
        InstrumentSelect[] children = gridContainer.GetComponentsInChildren<InstrumentSelect>(true);

        foreach (var element in children)
        {
            element.gameObject.SetActive(true);
        }
        
        foreach (var element in children.Select((value, index) => new {value, index}))
        {
            if (element.index < numbersToRender)
            {
                InstrumentSO instrument = pageItems[element.index];
                
                Image[] images = element.value.GetComponentsInChildren<Image>();
                images[1].sprite = instrument.InstrumentImage;
                
                TextMeshProUGUI title = element.value.GetComponentInChildren<TextMeshProUGUI>();
                title.text = instrument.InstrumentName;
                
                Button button = element.value.GetComponentInChildren<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate
                {
                    _concertHandler.SelectInstrument(_concertHandler.CurrentSelectType, instrument);
                });
            }
            else
            {
                element.value.gameObject.SetActive(false);
            }
        }
    }
    
    // Nav Button Related
    private void RenderButton(Button button, bool hasElement)
    {
        button.gameObject.SetActive(hasElement);
    }

    private bool HasNextPage() => _currPage < TotalPages;

    private bool HasPrevPage() => _currPage != 0;

    private void ResetNavButton()
    {
        prevButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }
}