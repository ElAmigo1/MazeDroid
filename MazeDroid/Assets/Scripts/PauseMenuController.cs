using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Manages the in-game pause popup. Opens on Escape or via the home button.
/// Contains a volume slider and a gyro sensitivity slider, mirroring the Settings screen.
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    private const string VolumePrefsKey = "MasterVolume";
    private const string MixerParameter = "MasterVolume";
    private const float DefaultVolume = 1f;
    private const float MinDb = -80f;

    [Header("References")]
    public AudioMixer audioMixer;
    public GameObject popupPanel;

    [Header("Sliders")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    private bool isOpen;

    void Start()
    {
        popupPanel.SetActive(false);
        InitVolumeSlider();
        InitSensitivitySlider();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Toggle();
    }

    /// <summary>Toggles the pause popup open or closed.</summary>
    public void Toggle()
    {
        isOpen = !isOpen;
        popupPanel.SetActive(isOpen);

        if (isOpen)
        {
            // Refresh sliders in case values changed in Settings screen
            InitVolumeSlider();
            InitSensitivitySlider();
        }
    }

    /// <summary>Opens the popup.</summary>
    public void Open() { if (!isOpen) Toggle(); }

    /// <summary>Closes the popup.</summary>
    public void Close() { if (isOpen) Toggle(); }

    /// <summary>Sets and persists master volume from the slider (0-1 range).</summary>
    public void SetVolume(float value)
    {
        PlayerPrefs.SetFloat(VolumePrefsKey, value);
        float db = value <= 0.001f ? MinDb : Mathf.Log10(value) * 20f;
        audioMixer.SetFloat(MixerParameter, db);
    }

    /// <summary>Sets and persists gyro sensitivity from the slider.</summary>
    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat(BoardController.SensitivityPrefsKey, value);
    }

    private void InitVolumeSlider()
    {
        if (volumeSlider == null) return;
        float saved = PlayerPrefs.GetFloat(VolumePrefsKey, DefaultVolume);
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.value = saved;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void InitSensitivitySlider()
    {
        if (sensitivitySlider == null) return;
        float saved = PlayerPrefs.GetFloat(BoardController.SensitivityPrefsKey, BoardController.DefaultSensitivity);
        sensitivitySlider.onValueChanged.RemoveListener(SetSensitivity);
        sensitivitySlider.minValue = 0.5f;
        sensitivitySlider.maxValue = 3f;
        sensitivitySlider.value = saved;
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }
}
