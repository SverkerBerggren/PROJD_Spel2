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

    public Type GetType(int IntegerToConvert)
    {
        return (typeof(ClientRequest));
    }
    public object Deserialize(MBJson.JSONObject ObjectToParse)
    {
        object ReturnValue = new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToParse);
        return (ReturnValue);
    }
}
