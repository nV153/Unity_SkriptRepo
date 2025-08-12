using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Idle movement behavior: the enemy does not move.
/// </summary>
public class IdleMovement : MonoBehaviour, IMovementBehavior
{
    /// <summary>
    /// Called every frame to update movement.
    /// In idle state, this stops the NavMeshAgent from moving.
/// </summary>
/// <param name="agent">The NavMeshAgent controlling movement</param>
/// <param name="self">Transform of the enemy</param>
/// <param name="target">Current target transform (can be null)</param>
/// <param name="isAttacking">Whether the enemy is attacking</param>
    public void UpdateMovement(NavMeshAgent agent, Transform self, Transform target, bool isAttacking)
    {
        // Stop any current movement/pathfinding
        agent.ResetPath();
    }
}
