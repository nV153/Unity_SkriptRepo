using System.Collections;
using UnityEngine;

/// <summary>
/// Interface for enemy attack patterns, defining the attack range and execution coroutine.
/// </summary>
public interface IAttackPattern
{
    /// <summary>
    /// The effective attack range of this pattern.
    /// </summary>
    float Range { get; }

    /// <summary>
    /// Executes the attack logic as a coroutine targeting the specified Transform.
    /// </summary>
    /// <param name="target">The target to attack</param>
    /// <returns>Coroutine enumerator</returns>
    IEnumerator ExecuteCoroutine(Transform target);
}
