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
    private CardMovement cardMovement;

    [NonSerialized] public CardDisplayAttributes cardDisplayAttributes;
    [NonSerialized] public SpriteRenderer artworkSpriteRenderer;
    [NonSerialized] public Transform displayTransform;
    [NonSerialized] public bool firstCardDrawn = false;
    [NonSerialized] public bool mouseDown = false;
    [NonSerialized] public bool clickedOnCard = false;

    [SerializeField] private float scaleOnHover = 1.3f; 

    public LayoutElement LayoutElement;
    public CardDissolve CardDissolve;

    public SelectIndicater selectIndicater;

    private void Awake()
    {
        if (!loadedSpriteRenderer && OpponentCard)
            LoadSpriteRendererOnce();
        if (!loadedDisplayAttributes)
            LoadDisplayAttributesOnce();
        Invoke(nameof(LoadInvoke), 0.01f);

        selectIndicater = SelectIndicater.Instance; 
    }

    private void Start()
    {
        displayTransform = transform.GetChild(0).transform;
        CardDissolve = GetComponentInChildren<CardDissolve>();
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
        artworkSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private void LoadDisplayAttributesOnce()
    {
        loadedDisplayAttributes = true;
        cardDisplayAttributes = transform.GetChild(0).GetComponent<CardDisplayAttributes>();
        displayTransform = cardDisplayAttributes.transform;
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
        if (OpponentCard) return;

        if (!alreadyBig && !clickedOnCard)
        {
            displayTransform.position += new Vector3(0, 7.5f, -1);
            displayTransform.localScale = new Vector3(scaleOnHover, scaleOnHover, scaleOnHover);
            alreadyBig = true;
        }
        //set up Select Indicater, should only call this metod when it is a attack card
     
        selectIndicater.UppdateIndicater();
    }

    public void MouseExit()
    {
        if (OpponentCard) return;
        if (!mouseDown)
        {
            alreadyBig = false;
            displayTransform.position += new Vector3(0, -7.5f, 1);
            ResetSize();
        }
        //Avaktivera  Indicater
        selectIndicater.DisableIndicater();
    }

    public void EndStep()
    {
        firstCardDrawn = false;
    }
}
