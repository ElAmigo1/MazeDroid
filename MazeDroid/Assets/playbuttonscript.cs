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
        // Store the target game scene, then go to skin select
        PlayerPrefs.SetString("TargetGameScene", "SampleScene");
        SceneManager.LoadScene("SkinSelect");
    }

    public void LoadSceneHardMode(string sceneName)
    {
        // Store the target game scene, then go to skin select
        PlayerPrefs.SetString("TargetGameScene", "Level 2");
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
}
