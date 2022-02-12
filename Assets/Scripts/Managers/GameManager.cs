using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _managers;
    [SerializeField] private GameObject _sceneGroupPrefab;
    private GameObject _sceneGroup;
    private bool _gameStarted;

    public static GameManager Instance { get; private set; }
    public event Action NewGameStarted;
    public event Action GamePaused;
    public event Action GameResumed;
    public event Action DifficultyChanged;
    public event Action GameOver;
    public event Action PieceLanded;


    private void Awake()
    {
        if (Instance != null) 
        {
            Destroy(Instance);
        }

        Instance = this;
    }

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

    public void SpawnNextPiece()
    {
        PieceLanded?.Invoke();
    }

    public void EndGame()
    {
        Destroy(_sceneGroup);
        GameOver?.Invoke();
    }

    public void ChangeDifficulty()
    {
        DifficultyChanged?.Invoke();
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }


}
