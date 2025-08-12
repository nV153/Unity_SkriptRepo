using UnityEngine;

/// <summary>
/// ScriptableObject to configure and create a random attack state for enemies.
/// Contains parameters like attack interval.
/// </summary>
[CreateAssetMenu(menuName = "Enemy/States/RndAttackStateConfig")]
public class RndAttackStateConfig : BaseStartStateConfig
{
    [Tooltip("Time interval between attacks in seconds.")]
    public float attackInterval = 3f;

    /// <summary>
    /// Creates a new instance of the RndAttackState with the configured attack interval.
    /// </summary>
    /// <returns>Instance of IEnemyBehaviorState</returns>
    public override IEnemyBehaviorState CreateState()
    {
        return new RndAttackState(attackInterval);
    }
}
