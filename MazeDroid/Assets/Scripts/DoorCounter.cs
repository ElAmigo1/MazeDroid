using TMPro;
using UnityEngine;

/// <summary>
/// Displays how many doors have been opened out of the total.
/// Call DoorCounter.Instance.RegisterDoorOpened() from BallRespawn each time a door is destroyed.
/// </summary>
public class DoorCounter : MonoBehaviour
{
    public static DoorCounter Instance { get; private set; }

    [Header("Display")]
    public TextMeshProUGUI counterText;
    public int totalDoors = 2;

    private int doorsOpened = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        UpdateDisplay();
    }

    /// <summary>Call this whenever a door is destroyed.</summary>
    public void RegisterDoorOpened()
    {
        doorsOpened = Mathf.Min(doorsOpened + 1, totalDoors);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (counterText != null)
            counterText.text = $"Doors\n{doorsOpened} / {totalDoors}";
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
