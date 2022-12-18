using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHolderTest : MonoBehaviour
{

    public List<AudioClip> listOfAudio = new List<AudioClip>();
    // Start is called before the first frame update
    void Start()
    {
        // PlaySoundEvent soundToPlay = new PlaySoundEvent("hej", listOfAudio[0]); 

       // SoundSystem.instance.PlaySound(new PlaySoundEvent("hej", listOfAudio[0]));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlaySound(listOfAudio[0]);
        }
    }
}
