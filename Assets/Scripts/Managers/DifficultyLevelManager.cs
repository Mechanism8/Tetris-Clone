using UnityEngine;
using TMPro;

public class DifficultyLevelManager : MonoBehaviour
{
    public TextMeshProUGUI difficultyLevelCounter;
    private int _maxLevel = 9;
    private bool _activated;
    [SerializeField] private StepDelayValueScriptableObject _stepDelayValue;
    [SerializeField] private DifficultyLevelScriptableObject _difficultyLevelScriptableObject;


    private void Start()
    {
        _difficultyLevelScriptableObject.difficultyLevel = 1;
        SetDifficulty();
    }

    private void OnEnable()
    {
        if (_activated) return;
        ScoreManager.ScoreUpdated += IncreaseLevel;
        GameManager.DifficultyChanged += SetDifficulty;
        _activated = true;
    }

    private void OnDisable()
    {
        ScoreManager.ScoreUpdated -= IncreaseLevel;
        GameManager.DifficultyChanged -= SetDifficulty;
        _activated = false;
    }

    private void IncreaseLevel()
    {
        if (_difficultyLevelScriptableObject.difficultyLevel != _maxLevel)
        {
            _difficultyLevelScriptableObject.difficultyLevel++;
            SetDifficulty();
        }        
    }

    private void SetDifficulty()
    {
        SetStepDelay();
        difficultyLevelCounter.SetText(_difficultyLevelScriptableObject.difficultyLevel.ToString());
    }

    public void SetStepDelay()
    {
        _stepDelayValue.stepDelay = 1 - (_difficultyLevelScriptableObject.difficultyLevel * .1f);
    }



}
