using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class Slash_GraveRobber : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private VisualEffect slashEffect;

    public void PlayEffect()
    {
        slashEffect.Play();
    }
}
