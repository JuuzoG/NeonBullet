using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string soundName;
    public AudioClip clip;
}

public class SoundLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffect[] sounds;

    private Dictionary<string, AudioClip> soundLookup;

    private void Awake()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        soundLookup = new Dictionary<string, AudioClip>();

        foreach (var sound in sounds)
        {
            if (string.IsNullOrEmpty(sound.soundName))
            {
                Debug.LogWarning("SoundLibrary: Ein Sound hat keinen Namen und wird übersprungen.", this);
                continue;
            }

            if (sound.clip == null)
            {
                Debug.LogWarning($"SoundLibrary: Sound '{sound.soundName}' hat keinen AudioClip zugewiesen.", this);
                continue;
            }

            if (!soundLookup.TryAdd(sound.soundName, sound.clip))
            {
                Debug.LogWarning($"SoundLibrary: Doppelter Sound-Name '{sound.soundName}' gefunden. Erster Eintrag wird verwendet.", this);
            }
        }
    }

    public AudioClip GetClipFromName(string soundName)
    {
        if (soundLookup == null)
        {
            BuildLookup();
        }

        if (soundLookup.TryGetValue(soundName, out AudioClip clip))
        {
            return clip;
        }

        Debug.LogWarning($"SoundLibrary: Kein Sound mit dem Namen '{soundName}' gefunden.", this);
        return null;
    }

    public bool TryGetClip(string soundName, out AudioClip clip)
    {
        if (soundLookup == null)
        {
            BuildLookup();
        }

        return soundLookup.TryGetValue(soundName, out clip);
    }
}