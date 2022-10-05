using System.Collections;
using System.Collections.Generic;
using System;

public class ServerResponse : MBJson.JSONDeserializeable,MBJson.JSONTypeConverter
{
    public  int Type;
  
    public  int whichPlayer;

    public bool cardPlayed = false;

    public List<GameAction> OpponentActions;

    public string message = "";

    public int cardId  = 0; 

    public Type GetType(int IntegerToConvert)
    {
        return (typeof(ServerResponse));
    }
    public object Deserialize(MBJson.JSONObject ObjectToParse)
    {
        object ReturnValue = new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToParse);
        return (ReturnValue);
    }
}