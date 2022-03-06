using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreDisplay;

    public int Score { get; private set; }

    public void AddScore()
    {
        Score++;
        scoreDisplay.text = Score.ToString();
    }

    public void Reset()
    {
        Score = 0;
        scoreDisplay.text = Score.ToString();
    }
}