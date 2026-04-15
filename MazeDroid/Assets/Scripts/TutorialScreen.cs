using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the Tutorial overlay on the Mainscreen.
/// Attach to the TutorialCanvas root. The root starts inactive.
/// Call Show() from the tutorial button's onClick persistent call.
/// </summary>
public class TutorialScreen : MonoBehaviour
{
    [Header("Close Button")]
    public Button closeButton;

    private bool listenersAdded = false;

    /// <summary>Shows the tutorial overlay.</summary>
    public void Show()
    {
        if (!listenersAdded)
        {
            listenersAdded = true;
            if (closeButton != null)
                closeButton.onClick.AddListener(Hide);
        }

        gameObject.SetActive(true);
    }

    /// <summary>Hides the tutorial overlay.</summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
