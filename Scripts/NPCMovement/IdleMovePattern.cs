using UnityEngine;

/// <summary>
/// Move pattern that does not move the character (idle).
/// </summary>
public class IdleMovePattern : IMovePattern
{
    /// <summary>
    /// Does nothing, representing an idle state.
    /// </summary>
    /// <param name="characterController">CharacterController to move (not used).</param>
    public void Move(CharacterController characterController)
    {
        // Intentionally empty: no movement for idle pattern
    }
}
