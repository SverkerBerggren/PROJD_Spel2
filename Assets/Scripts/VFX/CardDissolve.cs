using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardDissolve : MonoBehaviour
{
    //the VFX gameobject should set to disactive at start
    [SerializeField] private GameObject cardDissolve_VFX;
    private MeshRenderer meshMaterial;
    [SerializeField] private GameObject glow;
    [SerializeField] private GameObject textCanvas;
    [SerializeField] private string alpha = "_AlphaClipThreshold";

    private ActionOfPlayer actionOfPlayer;
    private CardDisplay display;
    private float dissolveRate = 0.0225f;
    private float refreshRate = 0.035f;

    // Start is called before the first frame update
    void Start()
    {
        actionOfPlayer = ActionOfPlayer.Instance;
        display = GetComponentInParent<CardDisplay>();
       meshMaterial = GetComponent<MeshRenderer>();

    }

    public void StartDisolve()
    {
        
        StartCoroutine(DissolveCoro());
    }
    // Update is called once per frame


    private IEnumerator DissolveCoro()
    {
        //alphaclip starts with 0. 0 means no clip and 1 is all clip
        //make a smooth transition from 0 to 1
       

        glow.GetComponent<MeshRenderer>().enabled = false;
        textCanvas.SetActive(false);

        float counter = meshMaterial.material.GetFloat(alpha);

        cardDissolve_VFX.SetActive(true);
        while ( meshMaterial.material.GetFloat(alpha) < 1f)
        {
           // Debug.Log(counter);
            counter += dissolveRate;
            meshMaterial.material.SetFloat(alpha, counter);
            if (meshMaterial.material.GetFloat(alpha) >= 0.99f)
            {
                Debug.Log("Down");
                cardDissolve_VFX.SetActive(false);
                meshMaterial.material.SetFloat(alpha, 0);
                glow.GetComponent<MeshRenderer>().enabled = true;
                textCanvas.SetActive(true);
                actionOfPlayer.ChangeCardOrder(true, display);
                break;

            }
            yield return new WaitForSeconds(refreshRate);
        }

        
    }
       

   
    private void PlayDissVFX()
    {

        cardDissolve_VFX.SetActive(true);
    }



 
}
