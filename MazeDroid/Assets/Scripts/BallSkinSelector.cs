using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Drives the Skin Select screen. Previews each ball skin on a 3D preview sphere
/// rendered via a RenderTexture, and saves the selection via BallSkinManager.
/// </summary>
public class BallSkinSelector : MonoBehaviour
{
    [Header("Preview")]
    public Renderer previewRenderer;
    public RawImage previewImage;

    [Header("Skin Labels")]
    public TMPro.TextMeshProUGUI skinNameLabel;

    [Header("Materials (set in Inspector)")]
    public Material[] skinMaterials;

    [Header("Skin Names")]
    public string[] skinNames = { "Default", "Basketball", "Football" };

    private int currentIndex;

    private static readonly string[] SkinDescriptions =
    {
        "Classic red ball",
        "Slam dunk ready!",
        "Touch down!"
    };

    void Start()
    {
        currentIndex = BallSkinManager.GetSkinIndex();
        RefreshPreview();
    }

    /// <summary>Cycles to the next skin.</summary>
    public void Next()
    {
        currentIndex = (currentIndex + 1) % skinMaterials.Length;
        RefreshPreview();
    }

    /// <summary>Cycles to the previous skin.</summary>
    public void Previous()
    {
        currentIndex = (currentIndex - 1 + skinMaterials.Length) % skinMaterials.Length;
        RefreshPreview();
    }

    /// <summary>Saves the current skin selection and loads the target game scene stored by SelectMode.</summary>
    public void ConfirmAndPlay()
    {
        BallSkinManager.SetSkinIndex(currentIndex);
        string targetScene = PlayerPrefs.GetString("TargetGameScene", "SampleScene");
        UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
    }

    private void RefreshPreview()
    {
        if (previewRenderer != null && skinMaterials != null && currentIndex < skinMaterials.Length)
            previewRenderer.sharedMaterial = skinMaterials[currentIndex];

        if (skinNameLabel != null)
        {
            string name = currentIndex < skinNames.Length ? skinNames[currentIndex] : $"Skin {currentIndex}";
            string desc = currentIndex < SkinDescriptions.Length ? SkinDescriptions[currentIndex] : "";
            skinNameLabel.text = $"{name}\n<size=70%><color=#aaaaaa>{desc}</color></size>";
        }
    }
}
