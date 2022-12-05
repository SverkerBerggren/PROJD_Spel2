using System.Collections;
using System.Collections.Generic;


public class RequestOpponentDiscardCard : ClientRequest
{
    public  int amountOfCardsToDiscard = 0;

    public bool isRandom = false;
    public bool discardCardToOpponentGraveyard = false;

    public RequestOpponentDiscardCard()
    {
        Type = 13;
    }

    public RequestOpponentDiscardCard(int amountOfCardsToDiscard, bool discardCardToOpponentGraveyard)
    {
        Type = 13;
        this.amountOfCardsToDiscard = amountOfCardsToDiscard;
        this.discardCardToOpponentGraveyard = discardCardToOpponentGraveyard;
    }
}
