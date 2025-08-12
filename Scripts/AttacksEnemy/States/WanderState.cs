/// <summary>
/// Wander state implementation for an enemy behavior.
/// Controls walking animation and updates movement when not attacking.
/// </summary>
public class WanderState : IEnemyBehaviorState
{
    /// <summary>
    /// Called once when entering the wander state.
    /// Enables walking animation if available.
    /// </summary>
    /// <param name="ctx">Enemy context containing components and state info.</param>
    public void Enter(EnemyContext ctx)
    {
        if (ctx.Animator != null)
        {
            ctx.Animator.SetBool("IsWalking", true);
        }
    }

    /// <summary>
    /// Called each frame to update the enemy behavior.
    /// Calls movement update if movement behavior is defined and enemy is not attacking.
    /// </summary>
    /// <param name="ctx">Enemy context.</param>
    public void Update(EnemyContext ctx)
    {
        if (ctx.MovementBehavior != null && !ctx.IsAttacking)
        {
            ctx.MovementBehavior.UpdateMovement(ctx.Agent, ctx.Self, ctx.Target, ctx.IsAttacking);
        }

        // Optional: add state transition logic here (e.g. if player detected, switch state)
    }

    /// <summary>
    /// Called once when exiting the wander state.
    /// Stops the agent and disables walking animation.
    /// </summary>
    /// <param name="ctx">Enemy context.</param>
    public void Exit(EnemyContext ctx)
    {
        ctx.Agent.ResetPath();

        if (ctx.Animator != null)
        {
            ctx.Animator.SetBool("IsWalking", false);
        }
    }
}
