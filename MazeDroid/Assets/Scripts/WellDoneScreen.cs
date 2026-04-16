using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the Well Done overlay shown when the ball reaches the level end zone.
/// The Canvas GameObject starts inactive in the scene. Show() activates it,
/// freezes time so the ball stops, and wires up the navigation buttons.
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

    /// <summary>Activates the Well Done screen, stops the ball and wires button listeners.</summary>
    public void Show()
    {
        // Wire buttons once — persistent calls are intentionally empty so only this listener fires.
        if (!listenersAdded)
        {
            listenersAdded = true;
            if (restartButton != null)
                restartButton.onClick.AddListener(OnPlayAgain);
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(OnMainMenu);
        }

        // Disable board input so the ball cannot move while the overlay is visible.
        BoardController board = FindObjectOfType<BoardController>();
        if (board != null)
            board.enabled = false;

        gameObject.SetActive(true);
    }

    private void OnPlayAgain()
    {
        SceneManager.LoadScene(playAgainScene);
    }

    private void OnMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    private void OnDestroy()
    {
        // Re-enable input if this screen is destroyed without navigating away (e.g. editor).
        BoardController board = FindObjectOfType<BoardController>();
        if (board != null)
            board.enabled = true;
    }
}
