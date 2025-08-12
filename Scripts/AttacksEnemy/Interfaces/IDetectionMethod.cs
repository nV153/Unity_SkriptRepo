using UnityEngine;

/// <summary>
/// Interface for detection methods used by enemies to find targets.
/// </summary>
public interface IDetectionMethod
{
    /// <summary>
    /// Detects and returns a target Transform from the given transform context.
    /// </summary>
    /// <param name="self">The transform of the enemy</param>
    /// <returns>Transform of the detected target, or null if none found</returns>
    Transform DetectTarget(Transform self);
}
