using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class Effect_GraveRobber : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private VisualEffect slashEffect;
    [SerializeField] private SkinnedMeshRenderer mainMesh;
    [SerializeField] private MeshRenderer shieldMesh;
    [SerializeField] private MeshRenderer vapenMesh;
    [SerializeField] private VisualEffect deathPS;
    private Material mainMaterial;
     private Material shieldMaterial;   
     private Material[] vapenMaterial;

    [SerializeField] private float dissolveRate = 0.0125f;
    [SerializeField] private float refreshRate = 0.025f;

    private bool goDissolve;

    void Start()
    {
        if (mainMesh != null) mainMaterial = mainMesh.material;
        if(shieldMesh != null) shieldMaterial = shieldMesh.material;
        if(vapenMesh != null) vapenMaterial = vapenMesh.materials;
    }

    void Update()
    {
        if (goDissolve)
        {
            StartCoroutine(DissolveCo());
        }
    }
    
    IEnumerator DissolveCo()
    {
          
            goDissolve = false;
            float counter = 0;
            deathPS.Play();
            while (mainMaterial.GetFloat("_DissolvedAmount") < 1 )
            {
                //decrease
                counter += dissolveRate;

                mainMaterial.SetFloat("_DissolvedAmount", counter);
                shieldMaterial.SetFloat("_DissolvedAmount", counter);
            for(int i =0; i< vapenMaterial.Length; i++)
            {
                vapenMaterial[i].SetFloat("_DissolvedAmount", counter);
            }

                yield return new WaitForSeconds(refreshRate);
            }
            //can this up to destory object
            if (mainMaterial.GetFloat("_DissolvedAmount") > 0.99f )
            {
                Debug.Log("Time to go");
                //Destory Champ?
            }
        
    }
    public void PlayEffect()
    {
        slashEffect.Play();
    }
    public void SetDissolve(bool bo)
    {
        goDissolve=bo;
    }
}
