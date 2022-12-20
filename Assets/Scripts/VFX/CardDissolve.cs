using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardDissolve : MonoBehaviour
{
    //the VFX gameobject should set to disactive at start
    [SerializeField] private GameObject cardDissolve_VFX;
    private Material meshMaterial;
    [SerializeField] private GameObject ram;
    [SerializeField] private GameObject textCanvas;
    [SerializeField] private string alpha = "_AlphaClipThreshold";

    private ActionOfPlayer actionOfPlayer;
    private CardDisplay display;
    private float dissolveRate = 0.0225f;
    private float refreshRate = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        actionOfPlayer = ActionOfPlayer.Instance;
        display = GetComponentInParent<CardDisplay>();
       meshMaterial = GetComponent<MeshRenderer>().material;

    }

    public void StartDisolve()
    {
        ram.SetActive(false);
        textCanvas.SetActive(false);
        StartCoroutine(DissolveCoro());
    }
    // Update is called once per frame


    private IEnumerator DissolveCoro()
    {
        //alphaclip starts with 0. 0 means no clip and 1 is all clip
        //make a smooth transition from 0 to 1
        float counter = meshMaterial.GetFloat(alpha);
        PlayDissVFX();
        while ( meshMaterial.GetFloat(alpha) < 1f)
        {
            counter += dissolveRate;
            meshMaterial.SetFloat(alpha, counter);
            yield return new WaitForSeconds(refreshRate);
        }

        if ( meshMaterial.GetFloat(alpha) >= 0.99f)
        {
            cardDissolve_VFX.SetActive(false);
            meshMaterial.SetFloat(alpha, 0);
            ram.SetActive(true);
            textCanvas.SetActive(true);
            actionOfPlayer.ChangeCardOrder(true, display);
      
        }
    }
       

   
    private void PlayDissVFX()
    {

        cardDissolve_VFX.SetActive(true);
    }



 
}
