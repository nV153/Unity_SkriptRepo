using UnityEngine;

/// <summary>
/// Provides a simple method to quit the application.
/// Typically called from a UI button in the main menu or pause menu.
/// </summary>
public class QuitGame : MonoBehaviour
{
    /// <summary>
    /// Quits the game application.
    /// Note: This will only work in a built application,
    /// not in the Unity Editor.
    /// </summary>
    public void ExitGame()
    {
        // Exit the game (has no effect in the Unity Editor)
        Application.Quit();
    }
}
