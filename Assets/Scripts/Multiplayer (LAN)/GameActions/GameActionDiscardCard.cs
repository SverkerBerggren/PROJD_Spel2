using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionDiscardCard : GameAction
{
    public List<string> listOfCardsDiscarded = new List<string>();
    public bool discardCardToOpponentGraveyard = false;
    public GameActionDiscardCard()
    {
        Type = 4; 
    }
    public GameActionDiscardCard(List<string> listOfCardsDiscarded, bool discardCardToOpponentGraveyard)
    {
        Type = 4;

        this.listOfCardsDiscarded = new List<string>(listOfCardsDiscarded);
        this.discardCardToOpponentGraveyard = discardCardToOpponentGraveyard;
    }
}
