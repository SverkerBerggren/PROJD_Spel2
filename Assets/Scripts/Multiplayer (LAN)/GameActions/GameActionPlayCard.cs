using System.Collections;
using System.Collections.Generic;
using System;

public class GameActionPlayCard : GameAction
{
    public CardAndPlacement cardAndPlacement = new CardAndPlacement();

    public GameActionPlayCard()
    {
        Type = 10; 
    }
    public GameActionPlayCard(CardAndPlacement cardToPlay)
    {
        Type = 10;

        this.cardAndPlacement = cardToPlay;
    }
}
