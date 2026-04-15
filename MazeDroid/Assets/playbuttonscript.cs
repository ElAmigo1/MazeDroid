using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        sceneName = "SelectMode";
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneNormalMode(string sceneName)
    {
        PlayerPrefs.SetString("TargetGameScene", "SampleScene");
        SceneManager.LoadScene("SkinSelect");
    }

    public void LoadSceneHardMode(string sceneName)
    {
        PlayerPrefs.SetString("TargetGameScene", "Level 2");
        SceneManager.LoadScene("SkinSelect");
    }

    public void LoadTrialMode(string sceneName)
    {
        PlayerPrefs.SetString("TargetGameScene", "TrialMode");
        SceneManager.LoadScene("SkinSelect");
    }

    public void Home(string sceneName)
    {
        sceneName = "Mainscreen";
        SceneManager.LoadScene(sceneName);
    }

    public void SettingsScene(string sceneName)
    {
        sceneName = "Settings";
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>Loads SelectMode — used by WellDoneScreen "Play Again".</summary>
    public void LoadSelectMode()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SelectMode");
    }

    /// <summary>Loads Mainscreen — used by WellDoneScreen "Main Menu".</summary>
    public void LoadMainscreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Mainscreen");
    }
}

