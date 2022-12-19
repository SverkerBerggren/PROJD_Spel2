using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public  class Effect_Champions : MonoBehaviour
{
    public Material[] materials;
    private const float DISOLVERATE = 0.0125f;
    private const float REFRESHRATE = 0.025f;
    [SerializeField] private VisualEffect deathPS;


    public void StartDisolve()
    {
        StartCoroutine(DissolveCo());
    }

    private IEnumerator DissolveCo()
    {
        if (materials != null )
        {
            float counter = 0;
            deathPS.Play();
            while (materials[0].GetFloat("_DissolvedAmount") < 1)
            {
                //decrease
                counter += DISOLVERATE;
                foreach(Material m in materials)
                {
                    m.SetFloat("_DissolvedAmount", counter);
                }
   

                yield return new WaitForSeconds(REFRESHRATE);
            }
            //can this up to destory object
            if (materials[0].GetFloat("_DissolvedAmount") > 0.99f)
            {
                Debug.Log("Time to go");
                //Destory Champ?
            }
        }
    }


    private void OnApplicationQuit()
    {
        foreach (Material m in materials)
        {
            m.SetFloat("_DissolvedAmount", 0);
        }
    }
}
