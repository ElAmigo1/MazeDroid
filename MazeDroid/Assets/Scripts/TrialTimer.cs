using TMPro;
using UnityEngine;

/// <summary>
/// Counts down from totalSeconds. When it hits zero, shows the GameOverScreen.
/// Attach to any persistent GameObject in the TrialMode scene.
/// </summary>
public class TrialTimer : MonoBehaviour
{
    public const float DefaultTime = 120f;

    [Header("Timer Settings")]
    public float totalSeconds = DefaultTime;

    [Header("Display")]
    public TextMeshProUGUI timerText;

    private float remaining;
    private bool finished = false;

    void Start()
    {
        remaining = totalSeconds;
        UpdateDisplay();
    }

    void Update()
    {
        if (finished) return;

        remaining -= Time.deltaTime;

        if (remaining <= 0f)
        {
            remaining = 0f;
            finished = true;
            UpdateDisplay();
            TriggerGameOver();
            return;
        }

        UpdateDisplay();
    }

    /// <summary>Call this when the ball reaches the end zone to stop the timer.</summary>
    public void StopTimer()
    {
        finished = true;
    }

    private void UpdateDisplay()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        timerText.text = $"{minutes:0}:{seconds:00}";

        // Turn red in the last 30 seconds.
        timerText.color = remaining <= 30f
            ? new Color(1f, 0.25f, 0.15f, 1f)
            : new Color(1f, 0.85f, 0.2f, 1f);
    }

    private void TriggerGameOver()
    {
        GameOverScreen go = FindObjectOfType<GameOverScreen>(true);
        if (go != null)
            go.Show();
        else
            Debug.LogError("[TrialTimer] GameOverScreen not found in scene!");
    }
}
