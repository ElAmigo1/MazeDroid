using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the Well Done overlay shown when the ball reaches the level end zone.
/// The Canvas GameObject starts inactive in the scene. Show() activates it.
/// </summary>
public class WellDoneScreen : MonoBehaviour
{
    [Header("Buttons")]
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Scene Names")]
    public string playAgainScene = "SelectMode";
    public string mainMenuScene = "Mainscreen";

    private bool listenersAdded = false;

    /// <summary>Activates the Well Done screen and freezes time.</summary>
    public void Show()
    {
        if (!listenersAdded)
        {
            listenersAdded = true;
            if (restartButton != null)
                restartButton.onClick.AddListener(OnPlayAgain);
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(OnMainMenu);
        }

        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnPlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(playAgainScene);
    }

    private void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
