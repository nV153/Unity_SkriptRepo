using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Movement behavior for chasing a target.
/// </summary>
public class ChaseMovement : MonoBehaviour, IMovementBehavior
{
    /// <summary>
    /// Updates the movement of the NavMeshAgent towards the target position.
    /// Stops moving if target is null or if the enemy is currently attacking.
    /// </summary>
    /// <param name="agent">NavMeshAgent controlling the movement</param>
    /// <param name="self">Transform of the enemy</param>
    /// <param name="target">Target to chase</param>
    /// <param name="isAttacking">Whether the enemy is attacking</param>
    public void UpdateMovement(NavMeshAgent agent, Transform self, Transform target, bool isAttacking)
    {
        if (target == null || isAttacking)
        {
            agent.ResetPath();
            return;
        }

        // Set destination to the target's position
        agent.SetDestination(target.position);
    }
}
