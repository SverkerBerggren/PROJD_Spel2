using System.Collections;
using System.Collections.Generic;

public class GameActionDrawCard : GameAction
{
    public int amountToDraw = 0;
    public int amountToDrawOpponent = 0; 

    public GameActionDrawCard() { }

    public GameActionDrawCard(int amountToDraw)
    {
        this.amountToDraw = amountToDraw;
        Type = 2; 
    }
    public GameActionDrawCard(int amountToDraw, int amountToDrawOpponent)
    {
        this.amountToDraw = amountToDraw;
        this.amountToDrawOpponent = amountToDrawOpponent; 
        Type = 2;
    }
}
