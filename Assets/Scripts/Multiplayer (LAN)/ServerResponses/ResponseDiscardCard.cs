using System.Collections;
using System.Collections.Generic;
public class ResponseDiscardCard : ServerResponse
{
    public List<string> listOfCardsDiscarded = new List<string>();
    public bool discardCardToOpponentGraveyard = false;
    public ListEnum listEnum = new ListEnum();

    public ResponseDiscardCard()
    {
        Type = 3; 
    }
    public ResponseDiscardCard(List<string> listOfCardsDiscarded, bool discardCardToOpponentGraveyard)
    {
        Type = 3;

        this.listOfCardsDiscarded = new List<string>(listOfCardsDiscarded);
        this.discardCardToOpponentGraveyard = discardCardToOpponentGraveyard;
    }
}
