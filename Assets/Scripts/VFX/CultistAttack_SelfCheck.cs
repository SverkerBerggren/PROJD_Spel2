using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CultistAttack_SelfCheck : MonoBehaviour
{
    // Start is called before the first frame update
    private VisualEffect effect;
    void Start()
    {
        effect = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        // alternativ check alive patticle count, when it drops to zero, destroy or recycle
       // Debug.Log(vfx.aliveParticleCount);
       //the delay of destroy should be greater than VFX playtime

        Destroy(gameObject,3f);
        
    }
}
