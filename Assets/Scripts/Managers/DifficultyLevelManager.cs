using UnityEngine;
using TMPro;

public class DifficultyLevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _difficultyLevelCounter;
    [SerializeField] private StepDelayValueScriptableObject _stepDelayValue;
    [SerializeField] private DifficultyLevelScriptableObject _difficultyLevelScriptableObject;
    private int _maxLevel = 9;
    private bool _activated;
    


    private void Start()
    {
        _difficultyLevelScriptableObject.difficultyLevel = 1;
        SetDifficulty();
    }

    private void OnEnable()
    {
        if (_activated) return;
        LineManager.LineLimitReached += IncreaseLevel;
        GameManager.Instance.NewGameStarted += SetDifficulty;
        GameManager.Instance.GameResumed += SetDifficulty;
        _activated = true;
    }

    private void OnDisable()
    {
        LineManager.LineLimitReached -= IncreaseLevel;
        GameManager.Instance.NewGameStarted -= SetDifficulty;
        GameManager.Instance.GameResumed -= SetDifficulty;
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
        _difficultyLevelCounter.SetText(_difficultyLevelScriptableObject.difficultyLevel.ToString());
    }

    public void SetStepDelay()
    {
        _stepDelayValue.stepDelay = 1 - (_difficultyLevelScriptableObject.difficultyLevel * .1f);
    }



}
