using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CultistAttackSelfCheck : MonoBehaviour
{

    void Start()
    {
        // alternativ check alive patticle count, when it drops to zero, destroy or recycle
        //the delay of destroy should be greater than VFX playtime
        Destroy(gameObject, 3f);
    }
}
