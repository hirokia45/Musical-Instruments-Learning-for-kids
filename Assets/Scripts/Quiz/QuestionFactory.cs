using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestionFactory : MonoBehaviour
{
    [SerializeField] private int questionAmount = 5;

    public List<Question> Questions => _questions;
    public int QuestionAmount => questionAmount;
    
    private List<Question> _questions = new List<Question>();
    private InstrumentData _instrumentData;

    private void Start()
    {
        _instrumentData = FindObjectOfType<InstrumentData>();
    }

    public void GenerateQuestions()
    {
        var unselectedList = Enumerable.Range(1, _instrumentData.List.Count).ToList();

        while (_questions.Count < questionAmount)
        {
            int index = unselectedList[Random.Range(0, unselectedList.Count)];
            unselectedList.Remove(index);

            InstrumentSO selected = _instrumentData.List[index - 1];
            Question newQuestion = new Question(selected);

            InstrumentSO firstOption = GenerateQuestionOption(index);
            newQuestion.AddOption(firstOption);
            InstrumentSO secondOption = GenerateQuestionOption(index, newQuestion.Options[1]);
            newQuestion.AddOption(secondOption);

            newQuestion.Shuffle();

            _questions.Add(newQuestion);
        }
    }

    public void ResetQuestions()
    {
        _questions.Clear();
    }

    private InstrumentSO GenerateQuestionOption(int correctAnswerIndex)
    {
        var numberList = Enumerable.Range(1, _instrumentData.List.Count).ToList();

        numberList.Remove(correctAnswerIndex);

        int index = numberList[Random.Range(0, numberList.Count)];

        return _instrumentData.List[index - 1];
    }

    private InstrumentSO GenerateQuestionOption(int correctAnswerIndex, InstrumentSO option)
    {
        var numberList = Enumerable.Range(1, _instrumentData.List.Count).ToList();

        int optionIndex = _instrumentData.List.FindIndex(item => item.InstrumentType == option.InstrumentType);

        numberList.Remove(correctAnswerIndex);
        numberList.Remove(optionIndex);

        int index = numberList[Random.Range(0, numberList.Count)];

        return _instrumentData.List[index - 1];
    }
}