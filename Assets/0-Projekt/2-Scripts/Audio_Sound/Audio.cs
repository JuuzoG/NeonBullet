using Unity.VisualScripting;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio instance;
    [SerializeField] private AudioSource soundObject;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void SoundEffect(AudioClip audioClip, Transform transform, float volume)
    {
        AudioSource audioSource = Instantiate(soundObject,transform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float cliplength = audioSource.clip.length;

        Destroy(audioSource.gameObject,cliplength);
    }
}
