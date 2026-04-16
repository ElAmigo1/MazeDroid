using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the in-game pause popup. Opens on Escape or via the gear button.
/// Contains a volume slider, a gyro sensitivity slider, a Continue button and an End Game button.
/// Does NOT use Time.timeScale so the UI EventSystem keeps processing input while paused.
/// The ball Rigidbody is frozen instead to stop gameplay.
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    private const string VolumePrefsKey = "MasterVolume";
    private const string MixerParameter = "MasterVolume";
    private const float DefaultVolume = 1f;
    private const float MinDb = -80f;
    private const string MainMenuScene = "Mainscreen";

    [Header("References")]
    public AudioMixer audioMixer;
    public GameObject popupPanel;
    [Tooltip("The ball Rigidbody to freeze while paused (drag the Ball GameObject here).")]
    public Rigidbody ballRigidbody;

    [Header("Sliders")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    private bool isOpen;
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    void Start()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);

        InitVolumeSlider();
        InitSensitivitySlider();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Toggle();
    }

    /// <summary>Toggles the pause popup open or closed. Freezes the ball Rigidbody instead of Time.timeScale so UI stays responsive.</summary>
    public void Toggle()
    {
        if (popupPanel == null) return;

        isOpen = !isOpen;
        popupPanel.SetActive(isOpen);
        SetBallFrozen(isOpen);

        if (isOpen)
        {
            InitVolumeSlider();
            InitSensitivitySlider();
        }
    }

    /// <summary>Opens the popup.</summary>
    public void Open() { if (!isOpen) Toggle(); }

    /// <summary>Closes the popup and resumes the game.</summary>
    public void Close() { if (isOpen) Toggle(); }

    /// <summary>Alias for Close — wired to the Continue button.</summary>
    public void Continue() => Close();

    /// <summary>Loads the main menu — wired to the End Game button.</summary>
    public void EndGame()
    {
        SetBallFrozen(false);
        SceneManager.LoadScene(MainMenuScene);
    }

    private void SetBallFrozen(bool freeze)
    {
        if (ballRigidbody == null) return;

        if (freeze)
        {
            savedVelocity = ballRigidbody.velocity;
            savedAngularVelocity = ballRigidbody.angularVelocity;
            ballRigidbody.isKinematic = true;
        }
        else
        {
            ballRigidbody.isKinematic = false;
            ballRigidbody.velocity = savedVelocity;
            ballRigidbody.angularVelocity = savedAngularVelocity;
        }
    }

    /// <summary>Sets and persists master volume from the slider (0–1 range).</summary>
    public void SetVolume(float value)
    {
        PlayerPrefs.SetFloat(VolumePrefsKey, value);
        if (audioMixer == null) return;
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
