using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestAddSpecificCardToHand : ClientRequest
{
    public string cardToAdd = "";

    public RequestAddSpecificCardToHand()
    {
        Type = 12; 
    }
    public RequestAddSpecificCardToHand(string cardToAdd)
    {
        Type = 12;
        this.cardToAdd = cardToAdd; 
    }
}
