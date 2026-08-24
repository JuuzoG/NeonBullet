using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip clip;
}

public class MusicLibrary : MonoBehaviour
{
    [SerializeField] private MusicTrack[] tracks;

    private Dictionary<string, AudioClip> trackLookup;

    private void Awake()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        trackLookup = new Dictionary<string, AudioClip>();

        foreach (var track in tracks)
        {
            if (string.IsNullOrEmpty(track.trackName))
            {
                Debug.LogWarning("MusicLibrary: Ein Track hat keinen Namen und wird übersprungen.", this);
                continue;
            }

            if (track.clip == null)
            {
                Debug.LogWarning($"MusicLibrary: Track '{track.trackName}' hat keinen AudioClip zugewiesen.", this);
                continue;
            }

            if (!trackLookup.TryAdd(track.trackName, track.clip))
            {
                Debug.LogWarning($"MusicLibrary: Doppelter Track-Name '{track.trackName}' gefunden. Erster Eintrag wird verwendet.", this);
            }
        }
    }

    public AudioClip GetClipFromName(string trackName)
    {
        if (trackLookup == null)
        {
            BuildLookup();
        }

        if (trackLookup.TryGetValue(trackName, out AudioClip clip))
        {
            return clip;
        }

        Debug.LogWarning($"MusicLibrary: Kein Track mit dem Namen '{trackName}' gefunden.", this);
        return null;
    }

    public bool TryGetClip(string trackName, out AudioClip clip)
    {
        if (trackLookup == null)
        {
            BuildLookup();
        }

        return trackLookup.TryGetValue(trackName, out clip);
    }
}