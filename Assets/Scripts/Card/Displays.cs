using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Displays : MonoBehaviour
{
    public Card card;
    public int manaCost;
    [NonSerialized] public CardDisplayAtributes cardDisplayAtributes;
    protected Calculations calculations;

    [System.NonSerialized] public CardTargeting cardTargeting;
    //[System.NonSerialized] public SpriteRenderer artworkSpriteRenderer;
    //[System.NonSerialized] public CardDisplayAtributes cardDisplayAtributes;

    public bool opponentCard;

    [System.NonSerialized] public int damageShow = 0;
    [System.NonSerialized] public int amountToHealShow = 0;
    [System.NonSerialized] public int amountToShieldShow = 0;
    [System.NonSerialized] public int amountOfCardsToDrawShow = 0;
    [System.NonSerialized] public int amountOfCardsToDiscardShow = 0;


}
