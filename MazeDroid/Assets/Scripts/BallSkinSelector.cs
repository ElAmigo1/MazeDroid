using UnityEngine;
using UnityEngine.UI;

public class BallSkinSelector : MonoBehaviour
{
    [Header("Preview")]
    public Transform previewParent;
    public RawImage previewImage;

    [Header("Skin Labels")]
    public TMPro.TextMeshProUGUI skinNameLabel;

    [Header("Skin Prefabs")]
    public GameObject[] skinPrefabs;

    [Header("Skin Names")]
    public string[] skinNames = { "Default", "Basketball", "Football" };

    private int currentIndex;
    private GameObject currentPreview;

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

    void Update()
    {
        // 🔄 Optional: langsame Rotation für nicer Preview
        if (currentPreview != null)
        {
            currentPreview.transform.Rotate(Vector3.up * 50f * Time.deltaTime);
        }
    }

    public void Next()
    {
        if (skinPrefabs == null || skinPrefabs.Length == 0) return;

        currentIndex = (currentIndex + 1) % skinPrefabs.Length;
        RefreshPreview();
    }

    public void Previous()
    {
        if (skinPrefabs == null || skinPrefabs.Length == 0) return;

        currentIndex = (currentIndex - 1 + skinPrefabs.Length) % skinPrefabs.Length;
        RefreshPreview();
    }

    public void ConfirmAndPlay()
    {
        BallSkinManager.SetSkinIndex(currentIndex);

        string targetScene = PlayerPrefs.GetString("TargetGameScene", "SampleScene");
        UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
    }

    private void RefreshPreview()
    {
        // alten löschen
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }

        // neuen spawnen
        if (previewParent != null && skinPrefabs != null && currentIndex < skinPrefabs.Length)
        {
            currentPreview = Instantiate(skinPrefabs[currentIndex], previewParent);

            currentPreview.transform.localPosition = Vector3.zero;
            currentPreview.transform.localRotation = Quaternion.identity;
            currentPreview.transform.localScale = Vector3.one;
        }
        else
        {
            Debug.LogWarning("PreviewParent oder SkinPrefabs fehlen!");
        }

        // UI Text
        if (skinNameLabel != null)
        {
            string name = currentIndex < skinNames.Length ? skinNames[currentIndex] : $"Skin {currentIndex}";
            string desc = currentIndex < SkinDescriptions.Length ? SkinDescriptions[currentIndex] : "";

            skinNameLabel.text = $"{name}\n<size=70%><color=#aaaaaa>{desc}</color></size>";
        }
    }
}