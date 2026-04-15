using UnityEngine;

public class BallSkinApplier : MonoBehaviour
{
    [Header("Model Setup")]
    public Transform modelAnchor;
    public GameObject[] skinPrefabs;
    public Vector3[] skinScales;
    public Vector3[] skinRotations;

    private GameObject currentModel;

    void Start()
    {
        ApplySelectedSkin();
    }

    public void ApplySelectedSkin()
    {
        int skinIndex = BallSkinManager.GetSkinIndex();

        if (skinPrefabs == null || skinPrefabs.Length == 0)
        {
            Debug.LogWarning("BallSkinApplier: Keine skinPrefabs gesetzt!");
            return;
        }

        if (modelAnchor == null)
        {
            Debug.LogWarning("BallSkinApplier: modelAnchor fehlt!");
            return;
        }

        if (skinIndex < 0 || skinIndex >= skinPrefabs.Length)
        {
            Debug.LogWarning("BallSkinApplier: Ungültiger Skin-Index, nehme 0.");
            skinIndex = 0;
        }

        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        currentModel = Instantiate(skinPrefabs[skinIndex], modelAnchor);
        currentModel.transform.localPosition = Vector3.zero;

        if (skinRotations != null && skinIndex < skinRotations.Length)
            currentModel.transform.localRotation = Quaternion.Euler(skinRotations[skinIndex]);
        else
            currentModel.transform.localRotation = Quaternion.identity;

        if (skinScales != null && skinIndex < skinScales.Length)
            currentModel.transform.localScale = skinScales[skinIndex];
        else
            currentModel.transform.localScale = Vector3.one;
    }
}