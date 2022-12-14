using System.Collections;
using System.Collections.Generic;
using System;

public class ServerResponse : MBJson.JSONDeserializeable,MBJson.JSONTypeConverter
{
    public  int Type = 0;
    public  int whichPlayer = 100;
    public List<GameAction> OpponentActions = new List<GameAction>();
    public string message = "";
    public int gameId = 0;

    public bool SQLIsConnected = false;
    public Type GetType(int IntegerToConvert)
    {   if(IntegerToConvert == 0)
        {
            return (typeof(ServerResponse));
        }
        if (IntegerToConvert == 1)
        {
            return (typeof(ResponseEndTurn));
        }
        if (IntegerToConvert == 2)
        {
            return (typeof(ResponseDrawCard));
        }
        if (IntegerToConvert == 3)
        {
            return (typeof(ResponseDiscardCard));
        }
        if (IntegerToConvert == 4)
        {
            return (typeof(ResponseHeal));
        }
        if (IntegerToConvert == 5)
        {
            return (typeof(ResponseDamage));
        }
        if (IntegerToConvert == 6)
        {
            return (typeof(ResponseShield));
        }
        if (IntegerToConvert == 7)
        {
            return (typeof(ResponseSwitchActiveChamp));
        }
        if (IntegerToConvert == 8)
        {
            return (typeof(ResponseDestroyLandmark));
        }
        if (IntegerToConvert == 9)
        {
            return (typeof(ResponseRemoveCardsGraveyard));
        }
        if (IntegerToConvert == 10)
        {
            return (typeof(ResponsePlayCard));
        }
        if (IntegerToConvert == 11)
        {
            return (typeof(ResponseAddSpecificCardToHand));
        }
        if (IntegerToConvert == 12)
        {
            return (typeof(ResponseOpponentDiscardCard));
        }
        if (IntegerToConvert == 13)
        {
            return (typeof(ResponsePlayLandmark));
        }
        if (IntegerToConvert == 14)
        {
            return (typeof(ResponsePassPriority));
        }
        if (IntegerToConvert == 15)
        {
            return (typeof(ResponseAvailableLobbies));
        }
        if (IntegerToConvert == 16)
        {
            return (typeof(ResponseHostLobby));
        }
        if (IntegerToConvert == 17)
        {
            return (typeof(ResponseJoinLobby));
        }
        if (IntegerToConvert == 18)
        {
            return (typeof(ResponseUniqueInteger));
        }
        if(IntegerToConvert == 19)
        {
            return (typeof(ResponseStopSwapping));
        }
        return (typeof(ServerResponse));
    }
    public ServerResponse() 
    {
        Type = 0;
    } //Denna ska inte tas bort, behovs for parsingen 

    public object Deserialize(MBJson.JSONObject ObjectToParse)
    {
        object ReturnValue = new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToParse);
        return (ReturnValue);
    }
}
