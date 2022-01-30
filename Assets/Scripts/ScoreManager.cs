using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public int maxscore;
    public TextMeshProUGUI scoreAmountLabel;
    public static event Action ScoreUpdated;
    private bool _activated;

    private void OnEnable()
    {
        if (_activated) return;
        Playfield.RowCleared += UpdateScore;
        _activated = true;

    }

    private void OnDisable()
    {
        Playfield.RowCleared -= UpdateScore;
        _activated = false;
    }

    void Start()
    {
        score = 0;
        maxscore = 1;
        scoreAmountLabel.SetText(score.ToString());
    }

    private void UpdateScore()
    {
        score++;
        CheckScoreAmount();
        scoreAmountLabel.SetText(score.ToString());
    }

    private void CheckScoreAmount()
    {
        if (score % maxscore == 0)
        {
            ScoreUpdated?.Invoke();
        }
    }
    
}
