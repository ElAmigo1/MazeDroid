using UnityEngine;

/// <summary>
/// Attach this to the Ball prefab. On Start it reads the saved skin from
/// BallSkinManager and applies the correct material to this ball's Renderer.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class BallSkinApplier : MonoBehaviour
{
    private Renderer ballRenderer;

    void Awake()
    {
        ballRenderer = GetComponent<Renderer>();
        int skinIndex = BallSkinManager.GetSkinIndex();
        ApplySkinInstance(skinIndex);
    }

    private void ApplySkinInstance(int index)
    {
        int clamped = Mathf.Clamp(index, 0, BallSkinManager.MaterialPaths.Length - 1);
        Material mat = Resources.Load<Material>(BallSkinManager.MaterialPaths[clamped]);

        if (mat != null)
            // Use material (not sharedMaterial) to create a runtime instance
            // so the original asset is never mutated
            ballRenderer.material = mat;
        else
            Debug.LogWarning($"BallSkinApplier: Material not found at Resources/{BallSkinManager.MaterialPaths[clamped]}");
    }
}
