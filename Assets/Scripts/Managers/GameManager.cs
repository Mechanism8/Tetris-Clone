using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _managers;
    [SerializeField] private GameObject _sceneGroupPrefab;
    private GameObject _sceneGroup;
    private bool _gameStarted;

    public static event Action NewGameStarted;
    public static event Action GamePaused;
    public static event Action GameResumed;
    public static event Action DifficultyChanged;
    public static event Action GameOver;


    private void Start()
    {
        ActivateAllManagers();
    }

    public void StartNewGame()
    {
        if (_gameStarted)
        {
            Destroy(_sceneGroup);
            Time.timeScale = 1f;
        }
        NewGameStarted?.Invoke();
        _sceneGroup = Instantiate(_sceneGroupPrefab, new Vector3(-1, -1, 0), Quaternion.identity);
        _gameStarted = true;
    }

    private void ActivateAllManagers()
    {
        foreach (var manager in _managers)
        {
            manager.SetActive(true);
        }
    }

    private void DeactivateAllManagers()
    {
        foreach (var manager in _managers)
        {
            manager.SetActive(false);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        GamePaused?.Invoke();
    }

    public void ResumeGame()
    {
        GameResumed?.Invoke();
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void EndGame()
    {
        Destroy(_sceneGroup);
        GameOver?.Invoke();
    }

    public void ChangeDifficulty()
    {
        DifficultyChanged?.Invoke();
    }


}
