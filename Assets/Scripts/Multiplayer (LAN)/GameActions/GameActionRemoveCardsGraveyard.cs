using System.Collections.Generic;

public class GameActionRemoveCardsGraveyard : GameAction
{
    public List<TargetInfo> cardsToRemoveGraveyard = new List<TargetInfo>();

    public GameActionRemoveCardsGraveyard()
    {
        Type = 9; 
    }
    public GameActionRemoveCardsGraveyard(List<TargetInfo> cardsToRemoveGraveyard)
    {
        this.cardsToRemoveGraveyard = cardsToRemoveGraveyard;
        Type = 9;
    }
}
