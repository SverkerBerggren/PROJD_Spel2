using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CardDisplay : Displays
{
    private bool alreadyBig = false;
    private Vector3 originalSize;

    [NonSerialized] public SpriteRenderer artworkSpriteRenderer;
    

    [NonSerialized] public bool firstCardDrawn = false;
    [NonSerialized] public bool mouseDown = false;

    private void Awake()
    {
        cardDisplayAtributes = transform.GetChild(0).GetComponent<CardDisplayAtributes>();
        Invoke(nameof(LoadInvoke), 0.01f);       
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (card == null)
            gameObject.SetActive(false);
    }

    private void LoadInvoke()
    {
        originalSize = transform.localScale;
        cardTargeting = GetComponent<CardTargeting>();
        
    }

    public void SetBackfaceOnOpponentCards(Sprite backfaceCard)
    {
        opponentCard = true;
        artworkSpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        artworkSpriteRenderer.sprite = backfaceCard;
        transform.Find("Landmark_Prefab").gameObject.SetActive(false);
    }

    public void UpdateTextOnCard()
    {       
        cardDisplayAtributes.UpdateTextOnCard(this);
    }

    public override void UpdateVariables()
    {
        Calculations.Instance.CalculateHandManaCost(this);
        base.UpdateVariables();
    }



    public void ResetSize()
    {
        transform.localScale = originalSize;
    }

    private void OnMouseEnter()
    {
        if (opponentCard) return;
       
        if (!alreadyBig)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 7, transform.position.z - 1);
            transform.localScale = new Vector3(transform.localScale.x + 0.5f, transform.localScale.x + 0.5f, transform.localScale.x + 0.5f);
            alreadyBig = true;
        }
    }
    private void OnMouseExit()
    {
        if (opponentCard) return;
        if (!mouseDown)
        {
            alreadyBig = false;
            transform.position = new Vector3(transform.position.x, transform.position.y - 7, transform.position.z + 1);
            ResetSize();
        }

    }

    public void EndStep()
    {
        firstCardDrawn = false;
    }
}
