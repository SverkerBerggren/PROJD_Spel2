using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash_Builder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ParticleSystem PS_Slash;

    public void PlayPS()
    {
        PS_Slash.Play();
    }
}
