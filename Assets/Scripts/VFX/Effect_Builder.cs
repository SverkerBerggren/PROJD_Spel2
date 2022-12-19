using System.Collections;

using UnityEngine;
using UnityEngine.VFX;


public class Effect_Builder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ParticleSystem PS_Slash;
    [SerializeField] private SkinnedMeshRenderer mainMesh;
    [SerializeField] private MeshRenderer hammerMesh;
    private Material skinnerMaterial;
    private Material vapenMaterial;
    [SerializeField]private float dissolveRate = 0.0125f;
    [SerializeField]private float refreshRate = 0.025f;
    [SerializeField] private VisualEffect deathPS;

    private bool goDissolve;
    private void Start()
    {
        if( mainMesh != null) skinnerMaterial = mainMesh.material;
        if (hammerMesh != null) vapenMaterial = hammerMesh.material;
    }

    public void StartDisolve()
    {
        StartCoroutine(DissolveCo());
    }

    private IEnumerator DissolveCo()
    {
        if(skinnerMaterial != null && vapenMaterial != null)
        {
            float counter = 0;
            deathPS.Play();
            while (skinnerMaterial.GetFloat("_DissolvedAmount") < 1 && vapenMaterial.GetFloat("_DissolvedAmount")<1)
            {
                //decrease
                counter += dissolveRate;
             
                    skinnerMaterial.SetFloat("_DissolvedAmount", counter);
                    vapenMaterial.SetFloat("_DissolvedAmount", counter);
               
                yield return new WaitForSeconds(refreshRate);
            }
            //can this up to destory object
            if (skinnerMaterial.GetFloat("_DissolvedAmount") > 0.99f && vapenMaterial.GetFloat("_DissolvedAmount") > 0.99f)
            {
                Debug.Log("Time to go");
                //Destory Champ?
            }
        }
    }
    //Call this up when attack
    public void PlayPS()
    {
        PS_Slash.Play();
    }

}
