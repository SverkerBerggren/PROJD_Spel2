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
    private bool loadedSpriteRenderer = false;
    private bool loadedDisplayAttributes = false;

    [NonSerialized] public CardDisplayAtributes cardDisplayAtributes;
    [NonSerialized] public SpriteRenderer artworkSpriteRenderer;
    

    [NonSerialized] public bool firstCardDrawn = false;
    [NonSerialized] public bool mouseDown = false;

    private void Awake()
    {
        
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
        if (!loadedSpriteRenderer)
            LoadSpriteRendererOnce();
        if (!loadedDisplayAttributes)
            LoadDisplayAttributesOnce();
    }


    public void SetBackfaceOnOpponentCards(Sprite backfaceCard)
    {
        if (!loadedSpriteRenderer)
            LoadSpriteRendererOnce();
        opponentCard = true;
        artworkSpriteRenderer.sprite = backfaceCard;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void LoadSpriteRendererOnce()
    {
        loadedSpriteRenderer = true;
        artworkSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private void LoadDisplayAttributesOnce()
    {
        loadedDisplayAttributes = true;
        
    }

    public void UpdateTextOnCard()
    {
/*        if (!loadedDisplayAttributes)
            LoadDisplayAttributesOnce();*/
        cardDisplayAtributes = transform.GetChild(0).GetComponent<CardDisplayAtributes>();
        print(cardDisplayAtributes == null);
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
