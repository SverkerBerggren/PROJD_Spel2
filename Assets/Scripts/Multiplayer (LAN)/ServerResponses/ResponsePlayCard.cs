

public class ResponsePlayCard : ServerResponse
{
    public CardAndPlacement cardAndPlacement = new CardAndPlacement();
    public int manaCost = 0;
    public ResponsePlayCard()
    {
        Type = 10;
    }
    public ResponsePlayCard(CardAndPlacement cardToPlay, int manaCost)
    {
        Type = 10;

        this.cardAndPlacement = cardToPlay;
        this.manaCost = manaCost;
    }
}
