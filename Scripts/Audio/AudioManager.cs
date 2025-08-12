using UnityEngine.Audio;
using UnityEngine;
using System;

/// <summary>
/// Manages the playback of sounds defined in the "sounds" array.
/// Implements singleton pattern to persist between scenes.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;                   // Array of sound objects to manage

    public static AudioManager instance;    // Singleton instance

    void Awake()
    {
        // Singleton pattern: ensure only one AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize AudioSource for each sound
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    /// <summary>
    /// Play a sound by its name.
    /// </summary>
    /// <param name="name">Name of the sound to play.</param>
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }

        s.source.Play();
    }

    /// <summary>
    /// Example method to play a specific sound called "BOOM".
    /// </summary>
    public void PlayBoomSound()
    {
        Play("BOOM");
    }
}
