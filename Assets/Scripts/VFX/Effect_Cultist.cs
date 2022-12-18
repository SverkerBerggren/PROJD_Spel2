 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Effect_Cultist : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer mainMesh;
    [SerializeField] private VisualEffect deathEffect;
    private Material mainMaterial;

    private const float DISOLVERATE = 0.0125f;
    private const float REFRESHRATE = 0.025f;

    private bool goDissolve;
    // Start is called before the first frame update
    void Start()
    {
        if (mainMesh != null) mainMaterial = mainMesh.material;
    }

    // Update is called once per frame
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
        deathEffect.Play();
        float counter = 0;

        while(mainMaterial.GetFloat("_DissolvedAmount") < 1)
        {
            counter += DISOLVERATE;
            mainMaterial.SetFloat("_DissolvedAmount", counter);
            yield return new WaitForSeconds(REFRESHRATE);
        }
        if(mainMaterial.GetFloat("_DissolvedAmount") > 0.99f)
            {
            Debug.Log("Time to go");
            }

    }

    
    public void SetDissolve(bool bo)
    {
        goDissolve = bo;
    }
}
