using UnityEngine;
using System;
using TMPro;

public class LineManager : MonoBehaviour
{
    public int lines;
    public int maxlines;
    public TextMeshProUGUI linesLabel;
    public static event Action LineLimitReached;
    private bool _activated;

    private void OnEnable()
    {
        if (_activated) return;
        Playfield.RowCleared += UpdateLines;
        GameManager.Instance.NewGameStarted += ResetLines;
        _activated = true;

    }

    private void OnDisable()
    {
        Playfield.RowCleared -= UpdateLines;
        GameManager.Instance.NewGameStarted -= ResetLines;
        _activated = false;
    }

    void Start()
    {
        ResetLines();
    }

    private void UpdateLines()
    {
        lines++;
        CheckScoreAmount();
        linesLabel.SetText(lines.ToString());
    }

    private void CheckScoreAmount()
    {
        if (lines % maxlines == 0)
        {
            LineLimitReached?.Invoke();
        }
    }

    private void ResetLines()
    {
        lines = 0;
        maxlines = 20;
        linesLabel.SetText(lines.ToString());
    }
    
}
