using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private const string MixerParameter = "MasterVolume";
    private const string VolumePrefsKey = "MasterVolume";
    private const float DefaultVolume = 1f;
    private const float MinDb = -80f;

    public static AudioManager instance;

    public AudioMixer audioMixer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ApplySavedVolume();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Applies the saved volume from PlayerPrefs to the AudioMixer.
    /// Called on startup so all scenes inherit the correct volume.
    /// </summary>
    private void ApplySavedVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefsKey, DefaultVolume);
        float db = savedVolume <= 0.001f ? MinDb : Mathf.Log10(savedVolume) * 20f;
        audioMixer.SetFloat(MixerParameter, db);
    }
}