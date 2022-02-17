using UnityEngine;
using UnityEngine.UI;

public class NextPreview : MonoBehaviour
{
    private bool _activated;
    private RawImage _previewImage;

    private void Awake()
    {
        _previewImage = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        if (_activated) return;
        Spawner.NextPieceUpdated += SetPreviewImage;
        _activated = true;
    }

    private void OnDisable()
    {
        Spawner.NextPieceUpdated -= SetPreviewImage;
        _activated = false;
    }

    private void SetPreviewImage(Texture2D preview)
    {
        _previewImage.texture = preview;
    }
}
