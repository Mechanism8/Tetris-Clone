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
    [SerializeField] private GameObject _gameOverLabel;
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
        StartCoroutine("GameOver");        
        
    }

    private IEnumerator GameOver()
    {
        _gameOverLabel.SetActive(true);
        yield return new WaitForSeconds(3f);
        _gameOverLabel.SetActive(false);
        ActivateMenu();
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
