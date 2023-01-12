
using System.Collections.Generic;


public class RequestRemoveCardsGraveyard : ClientRequest
{
    public List<TargetInfo> cardsToRemoveGraveyard = new List<TargetInfo>();

    public RequestRemoveCardsGraveyard()
    {
        Type = 10;
    }

    public RequestRemoveCardsGraveyard(List<TargetInfo> cardsToRemoveGraveyard)
    {
        this.cardsToRemoveGraveyard = cardsToRemoveGraveyard;
        Type = 10; 
    }


}
