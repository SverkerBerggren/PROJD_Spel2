


public class GameActionOpponentDiscardCard : GameAction
{
    public int amountOfCardsToDiscard = 0;

    public bool isRandom = false;
    public bool discardCardToOpponentGraveyard = false;

    public GameActionOpponentDiscardCard()
    {
        Type = 12;
    }
    
    public GameActionOpponentDiscardCard(int amountOfCardsToDiscard, bool discardCardToOpponentGraveyard)
    {
        Type = 12;
        this.amountOfCardsToDiscard = amountOfCardsToDiscard;
        this.discardCardToOpponentGraveyard = discardCardToOpponentGraveyard;
    }

}
