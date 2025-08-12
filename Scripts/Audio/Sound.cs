using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Represents a sound with its audio clip and playback settings.
/// </summary>
[System.Serializable]
public class Sound
{
    public string name;             // Name identifier for the sound

    public AudioClip clip;          // Audio clip to play

    [Range(0f, 1f)]
    public float volume = 1f;       // Volume level (0 to 1)

    [Range(0.1f, 3f)]
    public float pitch = 1f;        // Playback pitch (0.1 to 3)

    public bool loop = false;       // Should the sound loop

    [HideInInspector]
    public AudioSource source;      // Internal AudioSource component for playback, hidden in inspector
}
