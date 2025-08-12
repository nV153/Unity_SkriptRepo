using UnityEngine;

/// <summary>
/// Interface for movement behavior patterns that can be assigned to NPCs or characters.
/// </summary>
public interface IMovePattern
{
    /// <summary>
    /// Moves the character according to the pattern.
/// </summary>
/// <param name="characterController">CharacterController to move.</param>
    void Move(CharacterController characterController);
}
