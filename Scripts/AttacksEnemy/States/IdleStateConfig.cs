using UnityEngine;

/// <summary>
/// ScriptableObject configuration for the IdleState.
/// Creates a new instance of IdleState when requested.
/// </summary>
[CreateAssetMenu(menuName = "Enemy/States/IdleStateConfig")]
public class IdleStateConfig : BaseStartStateConfig
{
    /// <summary>
    /// Creates and returns a new IdleState instance.
    /// </summary>
    /// <returns>A new IdleState implementing IEnemyBehaviorState</returns>
    public override IEnemyBehaviorState CreateState()
    {
        return new IdleState();
    }
}
