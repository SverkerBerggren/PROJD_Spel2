using System.Collections;
using System.Collections.Generic;
using System;

public class GameAction : MBJson.JSONDeserializeable, MBJson.JSONTypeConverter
{
    public int Type = 0;
    public string errorMessage = "";
    public int gameId = 0; 
    public GameAction() { }// denna far inte tas bort, kravs for parsingen 
    public Type GetType(int IntegerToConvert)
    {   
        if(IntegerToConvert == 0)
        {
            return typeof(GameAction);
        }
        if(IntegerToConvert == 1)
        {
            return typeof(GameActionEndTurn); 
        }
        if (IntegerToConvert == 2)
        {
            return typeof(GameActionDrawCard);
        }
        if (IntegerToConvert == 3)
        {
            return typeof(GameActionHeal);
        }
        if (IntegerToConvert == 4)
        {
            return typeof(GameActionDiscardCard);
        }
        if (IntegerToConvert == 5)
        {
            return typeof(GameActionDamage);
        }       
        if (IntegerToConvert == 6)
        {
            return typeof(GameActionShield);
        }       
        if (IntegerToConvert == 7)
        {
            return typeof(GameActionSwitchActiveChamp);
        }     
        if (IntegerToConvert == 8)
        {
            return typeof(GameActionDestroyLandmark);
        }     
        if (IntegerToConvert == 9)
        {
            return typeof(GameActionRemoveCardsGraveyard);
        }    
        if (IntegerToConvert == 10)
        {
            return typeof(GameActionPlayCard);
        }   
        if (IntegerToConvert == 11)
        {
            return typeof(GameActionAddSpecificCardToHand);
        }   
        if (IntegerToConvert == 12)
        {
            return typeof(GameActionOpponentDiscardCard);
        }  
        if (IntegerToConvert == 13)
        {
            return typeof(GameActionPlayLandmark);
        }  
        if (IntegerToConvert == 14)
        {
            return typeof(GameActionGameSetup);
        }  
        if (IntegerToConvert == 15)
        {
            return typeof(GameActionPassPriority);
        }  
        if (IntegerToConvert == 16)
        {
            return typeof(GameActionStopSwapping);
        }    
        return (typeof(GameAction));
    }
    public object Deserialize(MBJson.JSONObject ObjectToParse)
    {
        object ReturnValue = new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToParse);
        return (ReturnValue);
    }


}
