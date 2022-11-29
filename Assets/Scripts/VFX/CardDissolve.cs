using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CardDissolve : MonoBehaviour
{
    [SerializeField] private VisualEffect cardDissolve_VFX;
    [SerializeField] private Renderer meshMaterial;
    private bool goDissolve;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void SetDissolveState(bool bo)
    {
        goDissolve = bo;
    }
}
