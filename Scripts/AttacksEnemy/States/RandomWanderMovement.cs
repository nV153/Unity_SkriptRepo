using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Movement behavior for random wandering.
/// The enemy randomly picks a new direction within a radius at fixed intervals.
/// Does not move while attacking.
/// </summary>
public class RandomWanderMovement : MonoBehaviour, IMovementBehavior
{
    public float changeDirectionInterval = 1f; // How often to pick a new direction (seconds)
    public float moveRadius = 5f;               // Max distance for random wandering

    private Vector3 currentDirection;
    private float lastChangeTime;

    /// <summary>
    /// Updates movement for the enemy.
    /// If not attacking, sets random destinations inside the defined radius.
    /// </summary>
    /// <param name="agent">NavMeshAgent used for movement</param>
    /// <param name="self">Transform of the enemy</param>
    /// <param name="target">Transform of the current target (unused)</param>
    /// <param name="isAttacking">Whether the enemy is attacking</param>
    public void UpdateMovement(NavMeshAgent agent, Transform self, Transform target, bool isAttacking)
    {
        // If agent is null or enemy is attacking, reset path and do not move
        if (agent == null || isAttacking)
        {
            agent.ResetPath();
            return;
        }

        // Time to pick a new random destination or if agent has no path
        if (Time.time >= lastChangeTime + changeDirectionInterval || !agent.hasPath)
        {
            // Generate random position inside moveRadius
            Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
            randomDirection.y = 0; // keep movement on horizontal plane

            Vector3 destination = self.position + randomDirection;

            NavMeshHit hit;
            // Find a valid position on the NavMesh close to destination
            if (NavMesh.SamplePosition(destination, out hit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                currentDirection = (hit.position - self.position).normalized;
                lastChangeTime = Time.time;
            }
        }
    }
}
