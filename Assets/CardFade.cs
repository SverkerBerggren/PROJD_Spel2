using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFade : MonoBehaviour
{
    private float alphaColor;
    private CanvasGroup canvasGroup;
    
    // Start is called before the first frame update
    void Start()
    {
        alphaColor = GetComponentInParent<Button>().targetGraphic.canvasRenderer.GetColor().a;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        canvasGroup.alpha = alphaColor;
    }
}
