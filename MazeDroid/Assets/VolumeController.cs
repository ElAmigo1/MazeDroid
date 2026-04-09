using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private const string MixerParameter = "MasterVolume";
    private const string VolumePrefsKey = "MasterVolume";
    private const float DefaultVolume = 1f;
    private const float MinDb = -80f;

    public AudioMixer audioMixer;
    public Slider volumeSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefsKey, DefaultVolume);

        if (volumeSlider != null)
        {
            // Suppress the OnValueChanged callback during initialization
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        ApplyVolume(savedVolume);
    }

    /// <summary>
    /// Sets the master volume from a normalized slider value (0 to 1).
    /// Saves the value to PlayerPrefs so it persists between sessions.
    /// </summary>
    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat(VolumePrefsKey, volume);
        ApplyVolume(volume);
    }

    private void ApplyVolume(float volume)
    {
        float db = volume <= 0.001f ? MinDb : Mathf.Log10(volume) * 20f;
        audioMixer.SetFloat(MixerParameter, db);
    }
}