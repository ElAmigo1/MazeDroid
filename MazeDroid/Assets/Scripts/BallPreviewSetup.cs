using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attaches to the PreviewCamera GameObject. Creates a RenderTexture at runtime,
/// targets the camera to it, and displays it on the provided RawImage in the UI.
/// </summary>
[RequireComponent(typeof(Camera))]
public class BallPreviewSetup : MonoBehaviour
{
    private const int RenderTextureSize = 512;

    public RawImage previewImage;

    private RenderTexture renderTexture;

    void Awake()
    {
        renderTexture = new RenderTexture(RenderTextureSize, RenderTextureSize, 16, RenderTextureFormat.ARGB32)
        {
            antiAliasing = 4
        };
        renderTexture.Create();

        Camera cam = GetComponent<Camera>();
        cam.targetTexture = renderTexture;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.14f, 0.15f, 0.2f, 1f);

        if (previewImage != null)
            previewImage.texture = renderTexture;
    }

    void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
        }
    }
}
