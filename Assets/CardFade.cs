using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFade : MonoBehaviour
{
    private float alphaColor;
    private float alphaCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        alphaColor = GetComponentInParent<Button>().targetGraphic.canvasRenderer.GetColor().a;
        this.GetComponent<CanvasGroup>().alpha = alphaColor;

    }
}
