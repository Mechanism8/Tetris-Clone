using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreLabel;
    [SerializeField] private TextMeshProUGUI _hiScoreLabel;
    [SerializeField] private HiScoreScriptableObject _hi;
    private int _score;
    private bool _activated;

    private void OnEnable()
    {
        if (_activated) return;
        GameManager.Instance.ScoreIncreased += UpdateScore;
        GameManager.Instance.NewGameStarted += ResetScore;
        _activated = true;
    }

    private void OnDisable()
    {
        GameManager.Instance.ScoreIncreased -= UpdateScore;
        GameManager.Instance.NewGameStarted -= ResetScore;
    }

    private void Start()
    {
        ResetScore();
    }

    private void ResetScore()
    {
        _score = 0;
        _scoreLabel.SetText(_score.ToString());
        _hiScoreLabel.SetText(_hi.hiScore.ToString());
    }

    private void UpdateScore(int amount)
    {
        _score += amount;
        _scoreLabel.SetText(_score.ToString());
        if (_score > _hi.hiScore) UpdateHiScore();
    }

    private void UpdateHiScore()
    {
        _hi.hiScore = _score;
        _hiScoreLabel.SetText(_hi.hiScore.ToString());
    }
}
