using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Game Over overlay for Trial Mode. Shown when the timer runs out.
/// The Canvas root starts inactive. Show() activates it and freezes time.
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    [Header("Buttons")]
    public Button retryButton;
    public Button mainMenuButton;

    [Header("Scene Names")]
    public string retryScene = "TrialMode";
    public string mainMenuScene = "Mainscreen";

    private bool listenersAdded = false;

    /// <summary>Activates the Game Over screen and freezes time.</summary>
    public void Show()
    {
        if (!listenersAdded)
        {
            listenersAdded = true;
            if (retryButton != null)
                retryButton.onClick.AddListener(OnRetry);
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(OnMainMenu);
        }

        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(retryScene);
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
