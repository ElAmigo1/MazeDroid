using UnityEngine;

/// <summary>
/// Place on a trigger collider at the end zone.
/// When the Ball tag enters, shows the Well Done screen and stops the trial timer if present.
/// </summary>
public class LevelEnd : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Ball")) return;

        triggered = true;

        // Stop the trial timer if this is Trial Mode.
        TrialTimer timer = FindObjectOfType<TrialTimer>();
        if (timer != null)
            timer.StopTimer();

        WellDoneScreen screen = FindObjectOfType<WellDoneScreen>(true);
        if (screen != null)
            screen.Show();
        else
            Debug.LogError("[LevelEnd] WellDoneScreen not found in scene!");
    }
}
