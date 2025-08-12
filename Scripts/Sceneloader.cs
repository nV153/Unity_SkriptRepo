using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides a simple way to load another scene by name.
/// Can be called from UI buttons or other scripts.
/// </summary>
public class SceneChanger : MonoBehaviour
{
    /// <summary>
    /// The name of the scene to load.
    /// This must match exactly the scene name in the Unity Build Settings.
    /// </summary>
    public string sceneName;

    /// <summary>
    /// Loads the specified scene by name.
    /// </summary>
    public void LoadScene()
    {
        // Load the scene with the given name
        SceneManager.LoadScene(sceneName);
    }
}
