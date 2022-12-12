using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Displays : MonoBehaviour
{
    public Card card;
    public int manaCost;

    [System.NonSerialized] public CardTargeting cardTargeting;
    [System.NonSerialized] public bool opponentCard;
}
