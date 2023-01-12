using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTOSpeechButton : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip clipToPlay;


    public void OnClick()
    {
        if (clipToPlay == null)
            return;
        audioSource.clip = clipToPlay;
        audioSource.PlayOneShot(clipToPlay);
    }
}
