using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    public static Audio instance;

    [Header("Library")]
    [SerializeField] private SoundLibrary soundLibrary;

    [Header("Sound Source")]
    [SerializeField] private AudioSource soundObject;

    [Header("Mixer")]
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [Header("Pitch Variance")]
    [SerializeField] private bool usePitchVariance = true;
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (soundLibrary == null)
        {
            Debug.LogWarning("Audio: Keine SoundLibrary zugewiesen. SoundEffect(string) wird nicht funktionieren.", this);
        }
    }

    // --- Name-basiertes Abspielen über die SoundLibrary ---
    public void SoundEffect(string soundName, Transform transform, float volume)
    {
        if (string.IsNullOrEmpty(soundName)) return;

        if (soundLibrary == null)
        {
            Debug.LogWarning("Audio: Keine SoundLibrary zugewiesen, kann Sound nicht per Name abspielen.", this);
            return;
        }

        if (!soundLibrary.TryGetClip(soundName, out AudioClip clip))
        {
            // SoundLibrary loggt bereits eine Warnung bei fehlendem Sound
            return;
        }

        SoundEffect(clip, transform, volume);
    }

    // --- Direktes Abspielen per AudioClip ---
    public void SoundEffect(AudioClip audioClip, Transform transform, float volume)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("Audio: audioClip ist null, Sound wird nicht abgespielt.", this);
            return;
        }

        if (soundObject == null)
        {
            Debug.LogWarning("Audio: soundObject (AudioSource Prefab) ist nicht zugewiesen.", this);
            return;
        }

        AudioSource audioSource = Instantiate(soundObject, transform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = sfxMixerGroup;

        // Leichte zufällige Tonhöhen-Variation, damit wiederholte Sounds nicht roboterhaft klingen
        if (usePitchVariance)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        }

        audioSource.Play();

        float cliplength = audioClip.length;

        Destroy(audioSource.gameObject, cliplength);
    }
}