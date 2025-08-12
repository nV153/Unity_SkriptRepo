using UnityEngine;

/// <summary>
/// Simple script that detects when another collider enters the trigger zone
/// and logs the name of the object to the console.
/// </summary>
public class SimpleTriggerCheck : MonoBehaviour
{
    /// <summary>
    /// Called automatically by Unity when another collider enters this object's trigger collider.
    /// </summary>
    /// <param name="other">The collider of the object that entered the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Log the name of the object that entered the trigger area
        Debug.Log("Object entered the zone: " + other.name);
    }
}