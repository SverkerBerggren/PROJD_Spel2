using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class CardDisplay : Displays
{
    private bool alreadyBig = false;
    private Vector3 originalSize;
    private Vector3 originalPosition;
    private Vector3 hoverPosition;
    private bool loadedSpriteRenderer = false;
    private bool loadedDisplayAttributes = false;
    private CardMovement cardMovement;
    private SelectIndicater selectIndicater;

    [NonSerialized] public CardDisplayAttributes cardDisplayAttributes;
    [NonSerialized] public SpriteRenderer artworkSpriteRenderer;
    [NonSerialized] public Transform displayTransform;
    [NonSerialized] public bool firstCardDrawn = false;
    [NonSerialized] public bool mouseDown = false;
    [NonSerialized] public bool clickedOnCard = false;

    [SerializeField] private float scaleOnHover = 1.3f; 

    public CardDissolve CardDissolve;


    private void Awake()
    {
        if (!loadedSpriteRenderer && OpponentCard)
            LoadSpriteRendererOnce();
        if (!loadedDisplayAttributes && !OpponentCard)
            LoadDisplayAttributesOnce();
        Invoke(nameof(LoadInvoke), 0.01f);

    }

    private void Start()
    {
        displayTransform = transform.GetChild(0).transform;
        CardDissolve = GetComponentInChildren<CardDissolve>();
        selectIndicater = SelectIndicater.Instance;
    }
    private void LoadInvoke()
    {
        originalSize = transform.localScale;
        CardTargeting = GetComponent<CardTargeting>();
        cardMovement = GetComponent<CardMovement>();
    }
    private void LoadSpriteRendererOnce()
    {
        loadedSpriteRenderer = true;
        artworkSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void LoadDisplayAttributesOnce()
    {
        loadedDisplayAttributes = true;
        cardDisplayAttributes = GetComponentInChildren<CardDisplayAttributes>();
        displayTransform = cardDisplayAttributes.transform;
        originalPosition = new Vector3(0, 0, 0);
        hoverPosition = new Vector3(originalPosition.x, originalPosition.y + 0.1f, originalPosition.z - 15);
    }

    public void HideUnusedCard()
    {
        gameObject.SetActive(false);
    }

    public void SetBackfaceOnOpponentCards(Sprite backfaceCard)
    {
        if (!loadedSpriteRenderer)
            LoadSpriteRendererOnce();
        OpponentCard = true;
        artworkSpriteRenderer.sprite = backfaceCard;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void UpdateTextOnCard(bool showCard)
    {
        if (!loadedDisplayAttributes)
            LoadDisplayAttributesOnce();
        if (!showCard)
            MouseExit();
        cardDisplayAttributes.UpdateTextOnCard(this);
    }

    public void ResetSize()
    {
        displayTransform.localScale = new Vector3(1,1,1);
    }

    public void MouseEnter()
    {
        if (OpponentCard) return;

        if (!alreadyBig && !clickedOnCard)
        {
            displayTransform.localPosition = hoverPosition;
            displayTransform.localScale = new Vector3(scaleOnHover, scaleOnHover, scaleOnHover);
            alreadyBig = true;
        }
        //set up Select Indicater, should only call this metod when it is a attack card
        
        if (selectIndicater != null)
            selectIndicater.UppdateIndicater(Card);
    }

    public void MouseExit()
    {
        if (OpponentCard) return;
        if (!mouseDown)
        {
            alreadyBig = false;
            displayTransform.localPosition = originalPosition;
            ResetSize();
        }
        //Avaktivera  Indicater
        if (selectIndicater != null)
            selectIndicater.DisableIndicater();
    }

    public void EndStep()
    {
        firstCardDrawn = false;
    }
}
