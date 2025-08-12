using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Interface for movement behaviors used by enemies.
/// </summary>
public interface IMovementBehavior
{
    /// <summary>
    /// Updates the movement logic for an enemy.
    /// </summary>
    /// <param name="agent">NavMeshAgent component controlling movement</param>
    /// <param name="self">Transform of the enemy</param>
    /// <param name="target">Target to move towards (can be null)</param>
    /// <param name="isAttacking">Whether the enemy is currently attacking</param>
    void UpdateMovement(NavMeshAgent agent, Transform self, Transform target, bool isAttacking);
}
