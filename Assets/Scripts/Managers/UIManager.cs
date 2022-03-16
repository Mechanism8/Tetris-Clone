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
    }

    private void OnDisable()
    {
        GameManager.Instance.NewGameStarted -= OnNewGame;
        GameManager.Instance.GamePaused -= OnGamePaused;
        GameManager.Instance.GameResumed -= OnGameResumed;
        GameManager.Instance.GameOver -= OnGameOver;
        GameManager.Instance.DifficultyChanged -= OnChangedDifficulty;
    }

    private void Start()
    {
        ActivateMenu();
        _resumeButton.interactable = false;
    }

    private void ActivateMenu()
    {
        _menuPanel.SetActive(true);
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
        _gameOverLabel.SetActive(false);
        _resumeButton.interactable = false;
        ActivateMenu();
    }

    private void OnGameResumed()
    {
        DeactivateMenu();
        _pauseButton.interactable = true;
    }

    private void DeactivateMenu()
    {
        _menuPanel.SetActive(false);
    }

    private void OnGamePaused()
    {
        ActivateMenu();
        _pauseButton.interactable = false;
    }

    private void OnNewGame()
    {
        DeactivateMenu();
        _infoPanel.SetActive(true);
        _helpPanel.SetActive(true);
        _resumeButton.interactable = true;
        _pauseButton.interactable = true;
    }


}
