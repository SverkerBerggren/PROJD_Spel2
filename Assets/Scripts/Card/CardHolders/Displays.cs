using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Displays : MonoBehaviour
{
    [System.NonSerialized] public CardTargeting CardTargeting;
    [System.NonSerialized] public bool OpponentCard;

    public Card Card;
    public int ManaCost;

}
