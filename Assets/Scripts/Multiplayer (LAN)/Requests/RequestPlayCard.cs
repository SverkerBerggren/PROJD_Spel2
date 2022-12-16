using System.Collections;
using System.Collections.Generic;
using System;

public class RequestPlayCard : ClientRequest
{
    public CardAndPlacement cardAndPlacement = new CardAndPlacement();
    public int manaCost;

    public RequestPlayCard()
    {
        Type = 11; 
    }
    public RequestPlayCard(CardAndPlacement cardToPlay, int manaCost)
    {
        Type = 11;
        this.cardAndPlacement = cardToPlay;
        this.manaCost = manaCost;
    }

}
