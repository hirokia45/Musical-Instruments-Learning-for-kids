using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizHandler : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private TextMeshProUGUI quizTitle;
    [SerializeField] private Canvas quizStartCanvas;
    [SerializeField] private Canvas quizPlayCanvas;
    [SerializeField] private Canvas audioPlayingCanvas;
    [SerializeField] private GameObject answerButtons;

    [Header("Answer Canvas Game Objects")]
    [SerializeField] private Canvas answerCanvas;
    [SerializeField] private TextMeshProUGUI answerTitle;
    [SerializeField] private Image answerImage;

    [Header("Quiz Result Canvas Game Objects")]
    [SerializeField] private Canvas quizResultCanvas;
    [SerializeField] private TextMeshProUGUI resultMessage;

    [Header("Config")]
    [SerializeField] private float displayWaitTime = 5f;
    [SerializeField] private float nextQuestionLoadingInterval = 1.5f;
    [SerializeField] private float displayCorrectAnswerInterval = 3f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip correctAnswerClip;
    [SerializeField] private AudioClip wrongAnswerClip;
    [SerializeField] private AudioClip resultClip;

    private AudioPlayer _audioPlayer;
    private QuestionFactory _questionFactory;
    private ScoreKeeper _scoreKeeper;

    private int _currentQuestionIndex = 0;
    private Question _currentQuestion;
    private bool _willUpdate;
    private bool _loadNextQuestion;

    private void Start()
    {
        quizPlayCanvas.gameObject.SetActive(false);
        answerCanvas.gameObject.SetActive(false);
        quizResultCanvas.gameObject.SetActive(false);
        _audioPlayer = FindObjectOfType<AudioPlayer>();
        _questionFactory = GetComponent<QuestionFactory>();
        _scoreKeeper = GetComponent<ScoreKeeper>();
    }

    private void Update()
    {
        if (_willUpdate)
        {
            UpdateQuestion();
        }

        if (_loadNextQuestion)
        {
            LoadQuestion();
        }
    }

    public void ProcessQuizCommand(int command)
    {
        _audioPlayer.Play(clickClip);

        Func<IEnumerator> func = command switch
        {
            0 => StartQuiz,
            1 => QuitQuiz,
            _ => StartQuiz
        };

        StartCoroutine(func());
    }
    
    public void PlayAudioAgain()
    {
        _audioPlayer.Play(_currentQuestion.CorrectInstrument.InstrumentClips[0]);
    }

    private IEnumerator StartQuiz()
    {
        yield return new WaitForSeconds(1f);
        ResetQuizData();
        _questionFactory.GenerateQuestions();

        quizResultCanvas.gameObject.SetActive(false);
        quizStartCanvas.gameObject.SetActive(false);
        quizPlayCanvas.gameObject.SetActive(true);

        LoadQuestion();
    }

    private IEnumerator QuitQuiz()
    {
        yield return new WaitForSeconds(1f);
        ResetQuizData();
        
        quizPlayCanvas.gameObject.SetActive(false);
        quizStartCanvas.gameObject.SetActive(true);
    }

    public void SelectAnswer(int playerInput)
    {
        CheckAnswer(playerInput);
    }

    private void LoadQuestion()
    {
        if (_loadNextQuestion) _loadNextQuestion = false;

        _currentQuestionIndex++;

        if (_currentQuestionIndex < _questionFactory.QuestionAmount + 1)
        {
            quizTitle.text = $"{_currentQuestionIndex} もんめ";

            _currentQuestion = _questionFactory.Questions[_currentQuestionIndex - 1];

            answerCanvas.gameObject.SetActive(false);
            audioPlayingCanvas.gameObject.SetActive(true);
            quizPlayCanvas.gameObject.SetActive(false);

            _audioPlayer.Play(_currentQuestion.CorrectInstrument.InstrumentClips[0]);

            Invoke(nameof(DisplayQuestion), displayWaitTime);
        }
        else
        {   
            DisplayResult();
        }
    }
    
    private void DisplayQuestion()
    {
        _willUpdate = true;
    }

    private void DisplayResult()
    {
        _audioPlayer.Play(resultClip);
        answerCanvas.gameObject.SetActive(false);
        quizResultCanvas.gameObject.SetActive(true);
        quizTitle.text = "クイズのけっか";

        resultMessage.text = $"{_questionFactory.QuestionAmount} もんちゅう　{_scoreKeeper.Score} もんせいかい";
    }

    private void UpdateQuestion()
    {
        _willUpdate = false;
        audioPlayingCanvas.gameObject.SetActive(false);
        quizPlayCanvas.gameObject.SetActive(true);

        InstrumentSelect[] children = answerButtons.GetComponentsInChildren<InstrumentSelect>();

        foreach (var child in children.Select((value, index) => new {value, index}))
        {
            InstrumentSO instrument = _currentQuestion.Options[child.index];

            TextMeshProUGUI title = child.value.GetComponentInChildren<TextMeshProUGUI>();
            title.text = instrument.InstrumentName;

            Image[] images = child.value.GetComponentsInChildren<Image>();
            images[2].sprite = instrument.InstrumentImage;
        }
    }

    private void CheckAnswer(int playerInput)
    {
        float interval;

        quizPlayCanvas.gameObject.SetActive(false);
        answerCanvas.gameObject.SetActive(true);
        answerImage.sprite = _currentQuestion.CorrectInstrument.InstrumentImage;

        if (_currentQuestion.AnswerIndex == playerInput)
        {
            interval = nextQuestionLoadingInterval;
            _audioPlayer.Play(correctAnswerClip);
            _scoreKeeper.AddScore();
            answerTitle.text = "せいかい！！";
        }
        else
        {
            interval = displayCorrectAnswerInterval;
            _audioPlayer.Play(wrongAnswerClip);
            answerTitle.text = "こたえ";
        }

        Invoke(nameof(NextQuestion), interval);
    }

    private void NextQuestion()
    {
        _loadNextQuestion = true;
    }
    
    private void ResetQuizData()
    {
        _questionFactory.ResetQuestions();
        _currentQuestionIndex = 0;
        _currentQuestion = null;
        _scoreKeeper.Reset();
    }
}