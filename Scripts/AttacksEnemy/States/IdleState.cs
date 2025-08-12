using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Represents the Idle state of an enemy.
/// In this state, the enemy stops moving and waits, but checks for targets.
/// </summary>
public class IdleState : IEnemyBehaviorState
{
    /// <summary>
    /// Called when entering the Idle state.
    /// Stops the NavMeshAgent movement.
    /// </summary>
    /// <param name="context">The enemy context</param>
    public void Enter(EnemyContext context)
    {
        // Stop movement when entering Idle
        context.Agent.ResetPath();
    }

    /// <summary>
    /// Called when exiting the Idle state.
    /// Can be used to clean up or reset variables.
    /// </summary>
    /// <param name="context">The enemy context</param>
    public void Exit(EnemyContext context)
    {
        // Optional: Logic when leaving the state
    }

    /// <summary>
    /// Called every frame while in Idle state.
    /// Checks for targets and can trigger a state change.
    /// </summary>
    /// <param name="context">The enemy context</param>
    public void Update(EnemyContext context)
    {
        // Detect a target using the detection method
        Transform target = context.DetectionMethod?.DetectTarget(context.transform);

        if (target != null)
        {
            // Set the detected target
            context.Target = target;

            // Example: Switch to a chase state (commented out here)
            // context.SwitchState(new ChaseState());
        }
    }
}
