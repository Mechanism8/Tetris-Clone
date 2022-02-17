using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    private AudioSource _audioSource;
    private bool _activated;

    private void OnEnable()
    {
        if (_activated) return;
        GameManager.Instance.GameOver += OnGameOver;
        GameManager.Instance.PieceLanded += OnLanded;
        Playfield.RowCleared += OnErase;
        _activated = true;
    }

    private void OnDisable()
    {
        GameManager.Instance.GameOver -= OnGameOver;
        GameManager.Instance.PieceLanded -= OnLanded;
        Playfield.RowCleared -= OnErase;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnLanded()
    {
        _audioSource.clip = _audioClips[1];
        _audioSource.Play();
    }

    private void OnErase()
    {
        _audioSource.clip = _audioClips[2];
        _audioSource.Play();
    }

    public void OnSelect()
    {
        _audioSource.clip = _audioClips[0];
        _audioSource.Play();
    }

    private void OnGameOver()
    {
        _audioSource.clip = _audioClips[3];
        _audioSource.Play();
    }


}
