using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private static AudioManager instance;

    public AudioClip shankerAttack;
    public AudioClip shieldSound;
    public AudioClip healSound;
    public AudioClip clickSound;
    public AudioClip builderAttack;
    public AudioClip graveRobberAttack;
    public AudioClip manaSound;
    public AudioClip landmarkSound;
    public AudioClip wrongUI;
    public List<AudioClip> damageSounds;
    private float originalVolume;

    

    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    public static AudioManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource.volume = 0.3f;
        originalVolume = 0.3f;
    }

    public void PlaySound(AudioClip soundToPlay)
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.PlayOneShot(soundToPlay);
    }
    public void PlayShankerAttack()
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.pitch = Random.Range(minPitch, maxPitch);   

        audioSource.volume = originalVolume;
        audioSource.PlayOneShot(shankerAttack);
    }
    public void PlayShieldSound()
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = originalVolume;
        audioSource.PlayOneShot(shieldSound);
    }
    public void PlayHealSound()
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = 0.17f;

        audioSource.PlayOneShot(healSound);
    }
    public void PlayClickSound()
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.pitch = 1f;
        audioSource.volume = originalVolume;

        audioSource.PlayOneShot(clickSound);
    }
    public void PlayBuilderAttackSound()
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.pitch = Random.Range(minPitch, maxPitch) ;
        audioSource.volume = originalVolume;

        audioSource.PlayOneShot(builderAttack);
    }
    public void PlayGraveDiggerAttackSound()
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.pitch = Random.Range(minPitch, maxPitch) ;
        audioSource.volume = originalVolume;

        audioSource.PlayOneShot(graveRobberAttack);
    }
    public void PlayManaSound()
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.pitch = Random.Range(minPitch, maxPitch) ;
        audioSource.volume = originalVolume;

        audioSource.PlayOneShot(manaSound);
    }
    
    public void PlayLandmarkSound()
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.pitch = Random.Range(minPitch, maxPitch) ;
        audioSource.volume = originalVolume;

        audioSource.PlayOneShot(landmarkSound);
    }
    
    public void PlayWrongUI()
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.pitch = 1f ;
        audioSource.volume = originalVolume;

        audioSource.PlayOneShot(wrongUI);
    }
    public void PlayDamageSound()
    {
        //    audioSource.clip = soundToPlay;
        //    audioSource.pitch = Random.Range(0.95f, 1.05f);
        //    audioSource.Play();
        audioSource.pitch = 1f ;
        audioSource.volume = originalVolume;

        audioSource.PlayOneShot(damageSounds[Random.Range(0,damageSounds.Count -1)]);
    }

    public void SetVolume(Slider slider)
    {   
        

        audioSource.volume = slider.value;
    }
}
