using UnityEngine;

/// <summary>
/// Interface representing a behavior state for an enemy.
/// States must implement entering, updating, and exiting logic.
/// </summary>
public interface IEnemyBehaviorState
{
    /// <summary>
    /// Called once when entering this state.
    /// </summary>
    /// <param name="ctx">Enemy context</param>
    void Enter(EnemyContext ctx);

    /// <summary>
    /// Called every frame while in this state.
    /// </summary>
    /// <param name="ctx">Enemy context</param>
    void Update(EnemyContext ctx);

    /// <summary>
    /// Called once when exiting this state.
    /// </summary>
    /// <param name="ctx">Enemy context</param>
    void Exit(EnemyContext ctx);
}
