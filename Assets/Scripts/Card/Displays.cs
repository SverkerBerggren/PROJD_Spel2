using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Displays : MonoBehaviour
{
    public Card card;
    public int manaCost;
    
    protected Calculations calculations;

    [System.NonSerialized] public CardTargeting cardTargeting;
    //[System.NonSerialized] public SpriteRenderer artworkSpriteRenderer;
    //[System.NonSerialized] public CardDisplayAtributes cardDisplayAtributes;

    [System.NonSerialized] public bool opponentCard;

    [System.NonSerialized] public bool previewCard = false;

    [System.NonSerialized] public int damageShow = 0;
    [System.NonSerialized] public int amountToHealShow = 0;
    [System.NonSerialized] public int amountToShieldShow = 0;
    [System.NonSerialized] public int amountOfCardsToDrawShow = 0;
    [System.NonSerialized] public int amountOfCardsToDiscardShow = 0;

    public virtual void UpdateVariables()
    {
        calculations = Calculations.Instance;

        if (card.damage != 0)
            damageShow = calculations.CalculateDamage(card.damage, previewCard);
        if (card.amountToHeal != 0)
            amountToHealShow = calculations.CalculateHealing(card.amountToHeal, previewCard);
        if (card.amountToShield != 0)
            amountToShieldShow = calculations.CalculateShield(card.amountToShield, previewCard);
        if (card.amountOfCardsToDraw != 0)
            amountOfCardsToDrawShow = card.amountOfCardsToDraw;
        if (card.amountOfCardsToDiscard != 0)
            amountOfCardsToDiscardShow = card.amountOfCardsToDiscard;        
    }
}
