using UnityEngine;

/// <summary>
/// ScriptableObject config for the WanderState.
/// Creates an instance of WanderState when requested.
/// </summary>
[CreateAssetMenu(menuName = "Enemy/States/WanderStateConfig")]
public class WanderStateConfig : BaseStartStateConfig
{
    /// <summary>
    /// Factory method to create a new WanderState instance.
    /// </summary>
    /// <returns>New WanderState object implementing IEnemyBehaviorState.</returns>
    public override IEnemyBehaviorState CreateState()
    {
        return new WanderState();
    }
}
