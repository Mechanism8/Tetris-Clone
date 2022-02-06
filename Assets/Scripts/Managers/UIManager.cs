using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _difficultySlider;
    [SerializeField] private TextMeshProUGUI _difficultyLabel;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gameOverLabel;
    [SerializeField] private DifficultyLevelScriptableObject _difficultyLevelScriptableObject;


    private bool _activated;

    private void OnEnable()
    {
        if (_activated) return;
        GameManager.NewGameStarted += OnNewGame;
        GameManager.GamePaused += OnGamePaused;
        GameManager.GameResumed += OnGameResumed;
        GameManager.GameOver += OnGameOver;
        GameManager.DifficultyChanged += OnChangedDifficulty;
    }

    private void OnDisable()
    {
        GameManager.NewGameStarted -= OnNewGame;
        GameManager.GamePaused -= OnGamePaused;
        GameManager.GameResumed -= OnGameResumed;
        GameManager.GameOver -= OnGameOver;
        GameManager.DifficultyChanged -= OnChangedDifficulty;
    }

    private void Start()
    {
        ActivateMenu();
    }

    private void ActivateMenu()
    {
        _menuPanel.SetActive(true);
    }

    private void OnChangedDifficulty()
    {
        _difficultyLabel.text = _difficultySlider.value.ToString();
        _difficultyLevelScriptableObject.difficultyLevel = (int) _difficultySlider.value;
        Debug.Log(_difficultyLevelScriptableObject.difficultyLevel);
        Debug.Log(_difficultySlider.value);
    }

    private void OnGameOver()
    {
        _gameOverLabel.SetActive(true);
        _infoPanel.SetActive(false);
    }

    private void OnGameResumed()
    {
        DeactivateMenu();
    }

    private void DeactivateMenu()
    {
        _menuPanel.SetActive(false);
    }

    private void OnGamePaused()
    {
        ActivateMenu();
    }

    private void OnNewGame()
    {
        DeactivateMenu();
        ActivateInfoPanel();
    }

    private void ActivateInfoPanel()
    {
        _infoPanel.SetActive(true);
    }
}
