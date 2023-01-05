using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public  class Effect_Champions : MonoBehaviour
{
    public SkinnedMeshRenderer[] skinneMaterials;
    public MeshRenderer[] meshMaterials;
    public VisualEffect attackVFX;
    private const float DISOLVERATE = 0.0125f;
    private const float REFRESHRATE = 0.025f;
    [SerializeField] private VisualEffect deathVFX;



    public void StartDisolve()
    {
        StartCoroutine(DissolveCo());
    }

    private IEnumerator DissolveCo()
    {
        if (skinneMaterials != null || meshMaterials != null)
        {
            float counter = 0;
            deathVFX.Play();
            while ((skinneMaterials.Length > 0 && skinneMaterials[0].material.GetFloat("_DissolvedAmount") < 1) || (meshMaterials.Length > 0 && meshMaterials[0].material.GetFloat("_DissolvedAmount") < 1))
            {
                //decrease
                counter += DISOLVERATE;
                foreach(SkinnedMeshRenderer sMR in skinneMaterials)
                {
                    foreach (Material ma in sMR.materials)
                    {
                        ma.SetFloat("_DissolvedAmount", counter);
                    }
                }

                foreach (MeshRenderer mr in meshMaterials)
                {
                    foreach (Material ma in mr.materials)
                    {
                        ma.SetFloat("_DissolvedAmount", counter);
                    }
                }
                //can this up to destory object
                if ((skinneMaterials.Length > 0 && skinneMaterials[0].material.GetFloat("_DissolvedAmount") > 0.99f) || (meshMaterials.Length > 0 && meshMaterials[0].material.GetFloat("_DissolvedAmount") > 0.99f))
                {
                    
                    Debug.Log("Time to go");
                    break;
                    //Destory Champ?
                }

                yield return new WaitForSeconds(REFRESHRATE);
            }
           
        }
    }


    //private void OnApplicationQuit()
    //{
    //    foreach (Material m in materials)
    //    {
    //        m.SetFloat("_DissolvedAmount", 0);
    //    }
    //}
}
