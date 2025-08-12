using UnityEngine;

/// <summary>
/// Enemy behavior state where the enemy attacks randomly at set intervals.
/// Chooses a random attack pattern from available ones and executes it.
/// </summary>
public class RndAttackState : IEnemyBehaviorState
{
    private float attackInterval;
    private float lastAttackTime;

    /// <summary>
    /// Constructor to set the attack interval.
    /// </summary>
    /// <param name="interval">Time in seconds between attacks</param>
    public RndAttackState(float interval = 3f)
    {
        attackInterval = interval;
        lastAttackTime = -interval; // Allow immediate attack on enter
    }

    /// <summary>
    /// Called when entering this state.
    /// Resets the last attack time to allow immediate attack.
    /// </summary>
    /// <param name="ctx">Enemy context</param>
    public void Enter(EnemyContext ctx)
    {
        lastAttackTime = Time.time - attackInterval;
    }

    /// <summary>
    /// Called every frame.
    /// Checks if it is time to attack and triggers a random attack pattern.
    /// </summary>
    /// <param name="ctx">Enemy context</param>
    public void Update(EnemyContext ctx)
    {
        if (ctx.Target == null || ctx.AttackPatterns.Count == 0)
            return;

        if (Time.time >= lastAttackTime + attackInterval)
        {
            int index = Random.Range(0, ctx.AttackPatterns.Count);
            ctx.StartCoroutine(ctx.AttackPatterns[index].ExecuteCoroutine(ctx.Target));
            lastAttackTime = Time.time;
        }
    }

    /// <summary>
    /// Called when exiting this state.
    /// Cleanup can be done here if necessary.
    /// </summary>
    /// <param name="ctx">Enemy context</param>
    public void Exit(EnemyContext ctx)
    {
        // Optional cleanup code here
    }
}
