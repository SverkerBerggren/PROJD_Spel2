using System.Collections;
using System.Collections.Generic;

public class GameActionAddSpecificCardToHand : GameAction
{
    public string cardToAdd = "";

    public GameActionAddSpecificCardToHand()
    {
        Type = 11; 
    }
    public GameActionAddSpecificCardToHand(string cardToAdd)
    {
        Type = 11;
        this.cardToAdd = cardToAdd;
    }
}
