using System;
using System.Collections;
using System.Collections.Generic;

public class ClientRequest : MBJson.JSONDeserializeable,MBJson.JSONTypeConverter
{
    public  int Type = 0; 
    public  int whichPlayer = 1000;

    public bool createScene = false;

    public bool isPolling = false;

    public bool hasPlayedCard = false;

   


    public int cardId  = 0; 


    public ClientRequest() { } //Denna far inte tas bort kravs for parsingen 

    public Type GetType(int IntegerToConvert)
    {   if(IntegerToConvert == 0)
        {
            return (typeof(ClientRequest));
        }
        if(IntegerToConvert == 1)
        {
            return (typeof(RequestOpponentActions));
        }
        if(IntegerToConvert == 2)
        {
            return (typeof(RequestEndTurn));
        }
        if (IntegerToConvert == 3)
        {
            return (typeof(RequestDrawCard));
        }
        if (IntegerToConvert == 4)
        {
            return (typeof(RequestHealing));
        }
        if (IntegerToConvert == 5)
        {
            return (typeof(RequestDiscardCard));
        }
        if (IntegerToConvert == 6)
        {
            return (typeof(RequestDamage));
        }
        if (IntegerToConvert == 7)
        {
            return (typeof(RequestShield));
        }
        if (IntegerToConvert == 8)
        {
            return (typeof(RequestSwitchActiveChamps));
        }
        if (IntegerToConvert == 9)
        {
            return (typeof(RequestDestroyLandmark));
        }
        if (IntegerToConvert == 10)
        {
            return (typeof(RequestRemoveCardsGraveyard));
        }
        if (IntegerToConvert == 11)
        {
            return (typeof(RequestPlayCard));
        }
        if (IntegerToConvert == 12)
        {
            return (typeof(RequestAddSpecificCardToHand));
        }
        if (IntegerToConvert == 13)
        {
            return (typeof(RequestOpponentDiscardCard));
        }
        if (IntegerToConvert == 14)
        {
            return (typeof(RequestPlayLandmark));
        }
        if (IntegerToConvert == 15)
        {
            return (typeof(ClientRequestGameSetup));
        }
        if (IntegerToConvert == 16)
        {
            return (typeof(RequestPassPriority));
        }

        return (typeof(ClientRequest));
    }
    public object Deserialize(MBJson.JSONObject ObjectToParse)
    {
        object ReturnValue = new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToParse);
        return (ReturnValue);
    }
}
