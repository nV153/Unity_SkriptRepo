using UnityEngine;

/// <summary>
/// Abstract base class for ScriptableObjects that create enemy behavior states.
/// Each derived ScriptableObject implements CreateState to instantiate the corresponding state.
/// </summary>
public abstract class BaseStartStateConfig : ScriptableObject
{
    /// <summary>
    /// Factory method to create an instance of an enemy behavior state.
/// </summary>
/// <returns>Instance implementing IEnemyBehaviorState</returns>
    public abstract IEnemyBehaviorState CreateState();
}
