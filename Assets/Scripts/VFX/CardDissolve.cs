using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardDissolve : MonoBehaviour
{
    //the VFX gameobject should set to disactive at start
    [SerializeField] private GameObject cardDissolve_VFX;
    [SerializeField] private Renderer meshMaterial;
    [SerializeField] private GameObject ram;
    [SerializeField] private GameObject textCanvas;
    [SerializeField] private string alpha = "_AlphaClipThreshold";

    private ActionOfPlayer actionOfPlayer;
    private CardDisplay display;
    private bool goDissolve;
    private float currentDiss;
    private float targetDiss;
    private int startAlph;
    private int fullAlph;

    // Start is called before the first frame update
    void Start()
    {
        actionOfPlayer = ActionOfPlayer.Instance;
        currentDiss = targetDiss = 0;
        fullAlph = 100;
        startAlph = 0;
        display = GetComponentInParent<CardDisplay>();

    }

    // Update is called once per frame
    void Update()
    {
        if (goDissolve)
        {


            //alphaclip starts with 0. 0 means no clip and 1 is all clip
            //make a smooth transition from 0 to 1
            ram.SetActive(false);
            textCanvas.SetActive(false);
            increaseAlpha(100);
            StartCoroutine(PlayVFX());
            currentDiss = Mathf.Lerp(currentDiss, targetDiss, Time.deltaTime);
            meshMaterial.material.SetFloat(alpha, currentDiss);
            //m_PropetyBlock.SetFloat("_AlphaClipThreshold", currentDiss);
            //meshMaterial.SetPropertyBlock(m_PropetyBlock);
            if(startAlph == 100 && meshMaterial.material.GetFloat(alpha) >= 0.99f)
            {
                //Time to go
               //TO DO
                cardDissolve_VFX.SetActive(false);
                meshMaterial.material.SetFloat(alpha, 0);
                ram.SetActive(true);
                textCanvas.SetActive(true);
                actionOfPlayer.ChangeCardOrder(true, display);
                goDissolve = false;
                //Destroy(gameObject);
            }
        }
    }

    IEnumerator PlayVFX()
    {
        yield return new WaitForSeconds(1f);
        cardDissolve_VFX.SetActive(true);
    }

    private void increaseAlpha(int i)
    {
        SetDissolveValue(Mathf.Min(startAlph+i,100));
    }

    private void SetDissolveValue(int j)
    {
        startAlph = j;
        targetDiss = (float)startAlph / fullAlph;
    }

    public void SetDissolveState(bool bo)
    {
        goDissolve = bo;
    }
}
