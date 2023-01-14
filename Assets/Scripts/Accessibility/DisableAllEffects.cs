using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAllEffects : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    private GameObject[] champs;
    void Start()
    {
        champs = GameObject.FindGameObjectsWithTag("Champion");
    }


    public void DisableEffects()
    {

        foreach (GameObject c in champs)
        {
            //close all animator
            if (c.GetComponentInChildren<AvailableChampion>().Animator != null)
                c.GetComponentInChildren<AvailableChampion>().Animator.enabled = false;

            //disable effect kontroller
            GameObject.Find("EffectController").GetComponent<EffectController>().DissableEffects(true);
        }
    }

    public void enableEffects()
    {
        foreach (GameObject c in champs)
        {
            //close all animator
            if (c.GetComponentInChildren<AvailableChampion>().Animator != null)
                c.GetComponentInChildren<AvailableChampion>().Animator.enabled = true;

            //disable effect kontroller
            GameObject.Find("EffectController").GetComponent<EffectController>().DissableEffects(false);
        }
    }

}
