using UnityEngine;

/// <summary>
/// Place on a trigger collider at the end zone.
/// When the Ball tag enters, it shows the Well Done screen.
/// </summary>
public class LevelEnd : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Ball")) return;

        triggered = true;

        WellDoneScreen screen = FindObjectOfType<WellDoneScreen>(true);
        if (screen != null)
            screen.Show();
        else
            Debug.LogError("[LevelEnd] WellDoneScreen not found in scene!");
    }
}
