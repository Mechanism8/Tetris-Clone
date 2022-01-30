using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyLevelManager : MonoBehaviour
{
    public TextMeshProUGUI difficultyLevelCounter;
    private int _minLevel = 1;
    private int _maxLevel = 9;
    private int _level;
    private bool _activated;
    [SerializeField] private StepDelayValueScriptableObject _stepDelayValue;


    private void OnEnable()
    {
        if (_activated) return;
        ScoreManager.ScoreUpdated += IncreaseLevel;
        _activated = true;
    }

    private void OnDisable()
    {
        ScoreManager.ScoreUpdated -= IncreaseLevel;
        _activated = false;
    }

    private void Start()
    {
        _level = 1;
        SetStepDelay(_level);
        difficultyLevelCounter.SetText(_level.ToString());
    }

    private void IncreaseLevel()
    {
        if (_level != _maxLevel)
        {
            _level++;
            SetStepDelay(_level);
            difficultyLevelCounter.SetText(_level.ToString());
        }        
    }

    private void DecreaseLevel()
    {
        if (_level != _minLevel)
        {
            _level--;
            SetStepDelay(_level);
            difficultyLevelCounter.SetText(_level.ToString());
        }        
    }
    public void SetStepDelay(int lvl)
    {
        _stepDelayValue.stepDelay = 1 - (lvl * .1f);
    }



}
