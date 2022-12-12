using System.Collections;
using System.Collections.Generic;
using System;

public class ResponsePlayCard : ServerResponse
{
    public CardAndPlacement cardAndPlacement = new CardAndPlacement();

    public ResponsePlayCard()
    {
        Type = 10;
    }
    public ResponsePlayCard(CardAndPlacement cardToPlay)
    {
        Type = 10;

        this.cardAndPlacement = cardToPlay; 
    }
}
