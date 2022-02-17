using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreLabel;
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
        _scoreLabel.text = _score.ToString();
    }

    private void UpdateScore(int amount)
    {
        _score += amount;
        _scoreLabel.text = _score.ToString();
    }
}
