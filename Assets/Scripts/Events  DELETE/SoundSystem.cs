using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// When this script is attached to an object, an AudioSource is automatically also added, and cannot be removed.
[RequireComponent(typeof(AudioSource))]
public class SoundSystem : MonoBehaviour
{
    public static SoundSystem instance;
    [SerializeField] private int maxBufferSize = 2;

    public AudioSource audioSource;
    private List<AudioSource> soundBuffer = new List<AudioSource>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        EventHandler.RegisterListener<PlaySoundEvent>(PlaySound);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("EffectsVolume");

        InvokeRepeating(nameof(ClearBuffer), 0, 0.5f);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("EffectsVolume", volume);
        audioSource.volume = volume;
    }

    public void PlaySound(PlaySoundEvent eventInfo)
    {
        AudioClip clip = eventInfo.sound;
        if (soundBuffer.Count < maxBufferSize)
        {
            audioSource.PlayOneShot(clip);
            soundBuffer.Add(audioSource);
        }
    }

    public void PlaySoundGhetto(AudioClip clip)
    {
        audioSource.Play();
        audioSource.PlayOneShot(clip);
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    void ClearBuffer()
    {
        soundBuffer.Clear();
    }
}
