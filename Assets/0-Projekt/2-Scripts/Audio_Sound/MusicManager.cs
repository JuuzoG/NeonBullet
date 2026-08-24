
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Library")]
    [SerializeField] private MusicLibrary musicLibrary;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sourceA;
    [SerializeField] private AudioSource sourceB;

    [Header("Mixer")]
    [SerializeField] private AudioMixerGroup musicMixerGroup;

    [Header("Fade Settings")]
    [SerializeField] private float defaultFadeDuration = 0.75f;

    private AudioSource activeSource;
    private AudioSource inactiveSource;
    private Coroutine fadeRoutine;
    private AudioClip currentClip;
    private string currentTrackName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Beide Sources vorbereiten
        sourceA.outputAudioMixerGroup = musicMixerGroup;
        sourceB.outputAudioMixerGroup = musicMixerGroup;

        sourceA.loop = true;
        sourceB.loop = true;

        sourceA.playOnAwake = false;
        sourceB.playOnAwake = false;

        activeSource = sourceA;
        inactiveSource = sourceB;
    }

    // --- Name-basiertes Abspielen über die MusicLibrary ---
    public void PlayMusic(string trackName, float fadeDuration = -1f)
    {
        if (string.IsNullOrEmpty(trackName)) return;
        
        if (trackName == currentTrackName) return;

        if (!musicLibrary.TryGetClip(trackName, out AudioClip clip))
        {
            // MusicLibrary loggt bereits eine Warnung bei fehlendem Track
            return;
        }

        currentTrackName = trackName;
        PlayClip(clip, fadeDuration);
    }

    private void PlayClip(AudioClip clip, float fadeDuration)
    {
        if (clip == null || clip == currentClip) return;

        if (fadeDuration < 0f) fadeDuration = defaultFadeDuration;

        currentClip = clip;

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(CrossFade(clip, fadeDuration));
    }

    public void StopMusic(float fadeDuration = -1f)
    {
        if (fadeDuration < 0f) fadeDuration = defaultFadeDuration;

        currentClip = null;
        currentTrackName = null;

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(FadeOut(activeSource, fadeDuration));
    }

    public bool IsPlaying(string trackName)
    {
        return trackName == currentTrackName;
    }

    private IEnumerator CrossFade(AudioClip newClip, float duration)
    {
        // Neuen Clip auf der inaktiven Source starten
        inactiveSource.clip = newClip;
        inactiveSource.volume = 0f;
        inactiveSource.Play();

        float timer = 0f;
        float startVolumeActive = activeSource.volume;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            inactiveSource.volume = Mathf.Lerp(0f, 1f, t);
            activeSource.volume = Mathf.Lerp(startVolumeActive, 0f, t);

            yield return null;
        }

        inactiveSource.volume = 1f;
        activeSource.volume = 0f;
        activeSource.Stop();

        // Rollen tauschen
        AudioSource temp = activeSource;
        activeSource = inactiveSource;
        inactiveSource = temp;

        fadeRoutine = null;
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        source.volume = 0f;
        source.Stop();

        fadeRoutine = null;
    }
}