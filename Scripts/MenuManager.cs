using UnityEngine;

/// <summary>
/// Manages the visibility of main menu and options menu panels.
/// Allows switching between the main menu and options menu UI.
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the main menu panel GameObject.
    /// </summary>
    public GameObject mainMenuPanel;

    /// <summary>
    /// Reference to the options menu panel GameObject.
    /// </summary>
    public GameObject optionsPanel;

    /// <summary>
    /// Shows the options menu and hides the main menu.
    /// </summary>
    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    /// <summary>
    /// Shows the main menu and hides the options menu.
    /// </summary>
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
}
