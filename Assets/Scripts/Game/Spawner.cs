using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] groups;
    private bool _activated;
    private int _nextPieceID;
    private Texture2D _nextPiecePreview;
    public static event System.Action<Texture2D> NextPieceUpdated;


    private void Awake()
    {
        RuntimePreviewGenerator.BackgroundColor = Color.clear;
        RuntimePreviewGenerator.PreviewDirection = Vector3.forward;
        RuntimePreviewGenerator.Padding = .5f;
    }

    private void OnEnable()
    {
        if (_activated) return;
        Group.BottomReached += SpawnNext;
        _activated = true;
    }

    private void OnDisable()
    {
        Group.BottomReached -= SpawnNext;
        _activated = false;
    }

    public void SpawnNext()
    {
        var instance = Instantiate(groups[_nextPieceID], transform.position, Quaternion.identity);
        instance.transform.SetParent(this.transform.parent);
        _nextPieceID = Random.Range(0, groups.Length);
        SetNextPiecePreview(_nextPieceID);
    }

    void Start()
    {
        _nextPieceID = Random.Range(0, groups.Length);
        SetNextPiecePreview(_nextPieceID);
        SpawnNext();
    }

    private void SetNextPiecePreview(int id)
    {
        _nextPiecePreview = RuntimePreviewGenerator.GenerateModelPreview(groups[id].transform, 512, 512);
        NextPieceUpdated?.Invoke(_nextPiecePreview);
    }

}
