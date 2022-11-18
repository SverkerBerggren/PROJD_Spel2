using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseOpponentDiscardCard : ServerResponse
{
    public int amountOfCardsToDiscard = 0;


    public bool isRandom = false;

    public ResponseOpponentDiscardCard()
    {
        Type = 12;
    }
    public ResponseOpponentDiscardCard(int amountOfCardsToDiscard)
    {
        Type = 12;
        this.amountOfCardsToDiscard = amountOfCardsToDiscard; 
    }


}
