using System.Collections;
using System.Collections.Generic;
using System;

public class GameAction : MBJson.JSONDeserializeable, MBJson.JSONTypeConverter
{

   public int Type = 0;
   
   public int cardId = 0;
   
   public bool cardPlayed = false;

    public string errorMessage = "";

    public GameAction() { }// denna far inte tas bort, kravs for parsingen 
    public Type GetType(int IntegerToConvert)
    {   
        if(Type == 0)
        {
            return typeof(GameAction);
        }
        if(Type == 1)
        {
            return typeof(GameActionEndTurn); 
        }
        if (Type == 2)
        {
            return typeof(GameActionDrawCard);
        }
        if (Type == 3)
        {
            return typeof(GameActionHeal);
        }
        if (Type == 4)
        {
            return typeof(GameActionDiscardCard);
        }
        if (Type == 5)
        {
            return typeof(GameActionDamage);
        }       
        if (Type == 6)
        {
            return typeof(GameActionShield);
        }       
        if (Type == 7)
        {
            return typeof(GameActionSwitchActiveChamp);
        }     
        if (Type == 8)
        {
            return typeof(GameActionDestroyLandmark);
        }     
        if (Type == 9)
        {
            return typeof(GameActionRemoveCardsGraveyard);
        }    
        if (Type == 10)
        {
            return typeof(GameActionPlayCard);
        }   
        if (Type == 11)
        {
            return typeof(GameActionAddSpecificCardToHand);
        }
        return (typeof(GameAction));
    }
    public object Deserialize(MBJson.JSONObject ObjectToParse)
    {
        object ReturnValue = new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToParse);
        return (ReturnValue);
    }


}
