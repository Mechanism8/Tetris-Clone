using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _difficultySlider;
    [SerializeField] private TextMeshProUGUI _difficultyLabel;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _helpPanel;
    [SerializeField] private GameObject _gameOverLabel;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private DifficultyLevelScriptableObject _difficultyLevelScriptableObject;

    private bool _activated;

    private void OnEnable()
    {
        if (_activated) return;
        GameManager.Instance.NewGameStarted += OnNewGame;
        GameManager.Instance.GamePaused += OnGamePaused;
        GameManager.Instance.GameResumed += OnGameResumed;
        GameManager.Instance.GameOver += OnGameOver;
        GameManager.Instance.DifficultyChanged += OnChangedDifficulty;
        _activated = true;
    }

    private void OnDisable()
    {
        GameManager.Instance.NewGameStarted -= OnNewGame;
        GameManager.Instance.GamePaused -= OnGamePaused;
        GameManager.Instance.GameResumed -= OnGameResumed;
        GameManager.Instance.GameOver -= OnGameOver;
        GameManager.Instance.DifficultyChanged -= OnChangedDifficulty;
        _activated = false;
    }

    private void Start()
    {
        _menuPanel.SetActive(true);
        _resumeButton.interactable = false;
    }

    private void OnChangedDifficulty()
    {
        _difficultyLabel.text = _difficultySlider.value.ToString();
        _difficultyLevelScriptableObject.difficultyLevel = (int) _difficultySlider.value;
    }

    private void OnGameOver()
    {
        _infoPanel.SetActive(false);
        _helpPanel.SetActive(false);
        StartCoroutine("GameOver");
        
    }

    private IEnumerator GameOver()
    {
        _gameOverLabel.SetActive(true);
        yield return new WaitForSeconds(3f);
        _difficultySlider.value = _difficultyLevelScriptableObject.difficultyLevel;
        _difficultyLabel.text = _difficultyLevelScriptableObject.difficultyLevel.ToString();
        _gameOverLabel.SetActive(false);
        _resumeButton.interactable = false;
        _menuPanel.SetActive(true);
    }

    private void OnGameResumed()
    {
        _menuPanel.SetActive(false);
        _pauseButton.interactable = true;
    }

    private void OnGamePaused()
    {
        _menuPanel.SetActive(true);
        _pauseButton.interactable = false;
        _difficultySlider.value = _difficultyLevelScriptableObject.difficultyLevel;
        _difficultyLabel.text = _difficultyLevelScriptableObject.difficultyLevel.ToString();
    }

    private void OnNewGame()
    {
        _menuPanel.SetActive(false);
        _infoPanel.SetActive(true);
        _helpPanel.SetActive(true);
        _resumeButton.interactable = true;
        _pauseButton.interactable = true;
    }


}
