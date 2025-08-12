using UnityEngine;

/// <summary>
/// Abstract base class for enemy state handlers in a state machine.
/// Each specific enemy state will inherit from this and implement its behavior.
/// </summary>
public abstract class EnemyStateHandler : MonoBehaviour
{
    /// <summary>
    /// Reference to the EnemyContext which holds shared enemy data and state machine control.
    /// </summary>
    protected EnemyContext context;

    /// <summary>
    /// Initializes the state handler with the given context.
    /// </summary>
    /// <param name="ctx">The EnemyContext containing shared data.</param>
    public virtual void Init(EnemyContext ctx)
    {
        context = ctx;
    }

    /// <summary>
    /// Called every frame by the EnemyContext to execute the state's logic.
    /// Must be implemented by subclasses.
    /// </summary>
    public abstract void UpdateHandler();
}
