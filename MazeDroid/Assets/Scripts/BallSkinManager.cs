using UnityEngine;

/// <summary>
/// Static manager that persists the selected ball skin index via PlayerPrefs.
/// Index 0 = Default, 1 = Basketball, 2 = Football.
/// </summary>
public static class BallSkinManager
{
    public const string SkinPrefsKey = "BallSkinIndex";
    public const int DefaultSkin     = 0;
    public const int BasketballSkin  = 1;
    public const int FootballSkin    = 2;

    public static readonly string[] MaterialPaths = new[]
    {
        "BallColor",       // Resources path for Default
        "BallBasketball",  // Resources path for Basketball
        "BallFootball",    // Resources path for Football
    };

    /// <summary>Returns the currently saved skin index.</summary>
    public static int GetSkinIndex()
    {
        return PlayerPrefs.GetInt(SkinPrefsKey, DefaultSkin);
    }

    /// <summary>Saves the selected skin index.</summary>
    public static void SetSkinIndex(int index)
    {
        PlayerPrefs.SetInt(SkinPrefsKey, index);
    }

    /// <summary>
    /// Loads the material for the given skin index from Resources and applies it to the renderer.
    /// </summary>
    public static void ApplySkin(Renderer renderer, int index)
    {
        if (renderer == null) return;

        int clamped = Mathf.Clamp(index, 0, MaterialPaths.Length - 1);
        Material mat = Resources.Load<Material>(MaterialPaths[clamped]);

        if (mat != null)
            renderer.sharedMaterial = mat;
        else
            Debug.LogWarning($"BallSkinManager: Material not found at Resources/{MaterialPaths[clamped]}");
    }
}
