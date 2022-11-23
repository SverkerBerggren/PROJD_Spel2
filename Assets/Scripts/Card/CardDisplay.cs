using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public int manaCost;

    private Calculations calculations;

    private bool alreadyBig = false;
    private Vector3 originalSize;

    [System.NonSerialized] public CardTargeting cardTargeting;
    [System.NonSerialized] public SpriteRenderer artworkSpriteRenderer;
    [System.NonSerialized] public CardDisplayAtributes cardDisplayAtributes;

    
    [System.NonSerialized] public bool opponentCard;
    [System.NonSerialized] public bool mouseDown = false;
    [System.NonSerialized] public int damageShow = 0;
    [System.NonSerialized] public int amountToHealShow = 0;
    [System.NonSerialized] public int amountToShieldShow = 0;
    [System.NonSerialized] public int amountOfCardsToDrawShow = 0;
    [System.NonSerialized] public int amountOfCardsToDiscardShow = 0;


    private void Awake()
    {
        Invoke(nameof(LoadInvoke), 0.01f);
        
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
    }

    public void UpdateTextOnCard()
    {
        print("ar card display attribtues null " + cardDisplayAtributes + " ar opponent card " + opponentCard);
        if (cardDisplayAtributes == null && !opponentCard)
            cardDisplayAtributes = transform.GetComponentInChildren<CardDisplayAtributes>();

        if(!opponentCard)
            cardDisplayAtributes.UpdateTextOnCard(this);
    }

    public void UpdateVariables()
    {
        calculations = Calculations.Instance;
        calculations.CalculateHandManaCost(this);

        if (card.damage != 0)
            damageShow = calculations.CalculateDamage(card.damage);
        if (card.amountToHeal != 0)
            amountToHealShow = calculations.CalculateHealing(card.amountToHeal);
        if (card.amountToShield != 0)
            amountToShieldShow = calculations.CalculateShield(card.amountToShield);
        if (card.amountOfCardsToDraw != 0)
            amountOfCardsToDrawShow = card.amountOfCardsToDraw;
        if (card.amountOfCardsToDiscard != 0)
            amountOfCardsToDiscardShow = card.amountOfCardsToDiscard;
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
}
