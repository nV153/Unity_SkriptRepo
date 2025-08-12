using UnityEngine;

/// <summary>
/// Ensures that the attached GameObject is not destroyed when loading new scenes.
/// Useful for persistent managers, audio sources, or player objects.
/// </summary>
public class DontDestroy : MonoBehaviour
{
    /// <summary>
    /// Called when the script instance is being loaded.
    /// Marks the GameObject to persist across scene loads.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
