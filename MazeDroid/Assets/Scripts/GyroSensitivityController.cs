using UnityEngine;
using UnityEngine.UI;

public class GyroSensitivityController : MonoBehaviour
{
    private const float MinSensitivity = 0.5f;
    private const float MaxSensitivity = 3f;

    public Slider sensitivitySlider;

    void Start()
    {
        float saved = PlayerPrefs.GetFloat(BoardController.SensitivityPrefsKey, BoardController.DefaultSensitivity);

        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = MinSensitivity;
            sensitivitySlider.maxValue = MaxSensitivity;

            sensitivitySlider.onValueChanged.RemoveListener(SetSensitivity);
            sensitivitySlider.value = saved;
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }
    }

    /// <summary>
    /// Sets the gyroscope sensitivity and persists it via PlayerPrefs.
    /// Range is 0.5 (low) to 3.0 (high).
    /// </summary>
    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat(BoardController.SensitivityPrefsKey, value);
    }
}
