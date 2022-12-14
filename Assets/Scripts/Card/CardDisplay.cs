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
    [SerializeField] private float scaleOnHover = 1.3f; 

    [NonSerialized] public CardDisplayAttributes cardDisplayAttributes;
    [NonSerialized] public SpriteRenderer artworkSpriteRenderer;

    [NonSerialized] public Transform displayTransform;


    public LayoutElement layoutElement;

    [NonSerialized] public bool firstCardDrawn = false;
    [NonSerialized] public bool mouseDown = false;
    [NonSerialized] public bool clickedOnCard = false;

    private CardMovement cardMovement;

    private void Awake()
    {
        if (!loadedSpriteRenderer && opponentCard)
            LoadSpriteRendererOnce();
        if (!loadedDisplayAttributes)
            LoadDisplayAttributesOnce();
        Invoke(nameof(LoadInvoke), 0.01f);       
    }

    public void HideUnusedCard()
    {
        gameObject.SetActive(false);
    }


    private void LoadInvoke()
    {
        originalSize = transform.localScale;
        cardTargeting = GetComponent<CardTargeting>();
        cardMovement = GetComponent<CardMovement>();
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
        cardDisplayAttributes = transform.GetChild(0).GetComponent<CardDisplayAttributes>();
        displayTransform = cardDisplayAttributes.transform;
    }

    public void UpdateTextOnCard()
    {
        if (!loadedDisplayAttributes)
            LoadDisplayAttributesOnce();

        cardDisplayAttributes.UpdateTextOnCard(this);
    }

    public void ResetSize()
    {
        displayTransform.localScale = new Vector3(1,1,1);
    }

    public void MouseEnter()
    {
        if (opponentCard) return;

        if (!alreadyBig && !clickedOnCard)
        {
            displayTransform.position += new Vector3(0, 7.5f, -1);
            displayTransform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            alreadyBig = true;
        }
    }

    public void MouseExit()
    {
        if (opponentCard) return;
        if (!mouseDown)
        {
            alreadyBig = false;
            displayTransform.position += new Vector3(0, -7.5f, 1);
            ResetSize();
        }
    }

    public void EndStep()
    {
        firstCardDrawn = false;
    }
}
