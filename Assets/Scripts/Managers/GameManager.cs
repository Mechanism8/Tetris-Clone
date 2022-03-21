using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _managers;
    [SerializeField] private GameObject _sceneGroupPrefab;
    private GameObject _sceneGroup;
    private bool _gameStarted;
    private bool _paused;

    public static GameManager Instance { get; private set; }
    public event Action NewGameStarted;
    public event Action GamePaused;
    public event Action GameResumed;
    public event Action DifficultyChanged;
    public event Action GameOver;
    public event Action PieceLanded;
    public event Action<int> ScoreIncreased;


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

    public void TogglePause()
    {
        if (!_paused)
        {           
            PauseGame();
        }
        else
        {           
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        _paused = true;
        Time.timeScale = 0f;
        GamePaused?.Invoke();
    }

    public void ResumeGame()
    {
        _paused = false;
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

    public void IncreaseScore(int score)
    {
        ScoreIncreased?.Invoke(score);
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
