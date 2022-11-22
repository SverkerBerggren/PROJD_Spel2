using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public enum CardType
{
    Spell,
    Landmark,
    Attack
};

public enum ChampionCardType
{
    None,
    Duelist,
    Builder,
    Cultist,
    TheOneWhoDraws,
    Shanker,
    Graverobber,
    All
};

public abstract class Card : ScriptableObject
{
    public string cardName;
    public CardType typeOfCard;
    public string description;

    public Sprite artwork;
    public int maxManaCost;

    public string tag;

    private Champion target;
    private LandmarkDisplay landmarkTarget;

    [Header("Atributes")]
    public int damage = 0;
    public int amountToHeal = 0;
    public int amountToShield = 0;
    public int amountOfCardsToDraw = 0;
    public int amountOfCardsToDiscard = 0;

    public bool discardCardsYourself = true;
    public bool targetable = false;

    public bool championCard = false;
    public ChampionCardType championCardType = ChampionCardType.None;

  
    public Champion Target { get { return target; } set { target = value; } }
    public LandmarkDisplay LandmarkTarget { get { return landmarkTarget; } set { landmarkTarget = value; } }

    protected Card() { }

    public virtual void PlayCard()
    {
        CardAndPlacement cardPlacement = new CardAndPlacement();
        cardPlacement.cardName = cardName;
        GameState gameState = GameState.Instance;
        
        TargetInfo placement = new TargetInfo();
        placement.whichList = new ListEnum();

        if (typeOfCard != CardType.Landmark)
        {
            ListEnum tempEnum = new ListEnum();
            tempEnum.myGraveyard = true;
            placement.whichList = tempEnum;
            placement.index = 100;
        }

        if (gameState.isOnline)
        {
            RequestPlayCard playCardRequest = new RequestPlayCard(cardPlacement);
            playCardRequest.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(playCardRequest, gameState.RequestEmpty);
        }
        
        if (amountOfCardsToDraw != 0)
        {
            gameState.DrawCard(amountOfCardsToDraw, null);
            gameState.Refresh();
        }
        if (amountOfCardsToDiscard != 0)
        {
            gameState.DiscardCard(amountOfCardsToDiscard, discardCardsYourself);
            gameState.Refresh();
        }
        gameState.playerChampion.champion.AmountOfCardsPlayed(this);

    }
   
    public virtual string WriteOutCardInfo()
    {
        string lineToWriteOut = null;
        lineToWriteOut = "Cardname: " +cardName + "\nDescription:  " + description + "\nTypeOfCard: " + typeOfCard + "\nMaxMana: " + maxManaCost + 
            "\nTag: " + tag + "\nAmountOfDamage: " + damage + "\nAmountOfHealing: " + amountToHeal + "\nAmountToShield: " + amountToShield + 
            "\nAmountOfCardsToDraw: " + amountOfCardsToDraw + "\nAmountOfCardsToDiscard: " + amountOfCardsToDiscard + "\nDiscardCardsYourself: " + discardCardsYourself + 
            "\nTargetable: " + targetable + "\nChampionCard: " + championCard + "\nChampionCardType: " + championCardType;
        return lineToWriteOut; 
    }


}
