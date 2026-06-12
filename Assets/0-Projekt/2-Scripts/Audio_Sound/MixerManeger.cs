using UnityEngine;
using UnityEngine.Audio;

public class MixerManeger : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume",volume);
    }
}
