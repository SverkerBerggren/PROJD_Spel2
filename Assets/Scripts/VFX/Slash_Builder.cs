using System.Collections;

using UnityEngine;
using UnityEngine.VFX;


public class Slash_Builder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ParticleSystem PS_Slash;
    [SerializeField] private SkinnedMeshRenderer mainMesh;
    [SerializeField] private MeshRenderer hammerMesh;
    private Material skinnerMaterial;
    private Material vapenMaterial;
    [SerializeField]private float dissolveRate = 0.0125f;
    [SerializeField]private float refreshRate = 0.2f;
    [SerializeField] private VisualEffect deathPS;
    private void Start()
    {
        if(mainMesh != null)
        {
            skinnerMaterial = mainMesh.material;
            vapenMaterial = hammerMesh.material;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo()
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
        }
    }
    public void PlayPS()
    {
        PS_Slash.Play();
    }


}
