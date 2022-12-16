using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Assertions.Must;

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

    private Champion target;
    private LandmarkDisplay landmarkTarget;

    [Header("Atributes")]
    public int damage = 0;
    public int amountToHeal = 0;
    public int amountToShield = 0;
    public int amountOfCardsToDraw = 0;
    public int amountOfCardsToDiscard = 0;

    [Header("Effect")]
    public Effects effect;

    public bool discardCardsYourself = true;
    public bool targetable = false;

    public bool championCard = false;
    public ChampionCardType championCardType = ChampionCardType.None;

    [NonSerialized] public bool purchasedFormShop = false;

  
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

        if (purchasedFormShop)
            placement.whichList.opponentGraveyard = false;
        else
            placement.whichList.opponentGraveyard = true;
        
        cardPlacement.placement = placement;

        gameState.ShowPlayedCard(this, false, -1);
        if (gameState.isOnline)        
            gameState.PlayCardRequest(cardPlacement);
        
        if (amountOfCardsToDraw != 0)       
            gameState.DrawCard(amountOfCardsToDraw, null);
        
        if (amountOfCardsToDiscard != 0)     
            gameState.DiscardCard(amountOfCardsToDiscard, discardCardsYourself);
        
        if (effect != null)
            gameState.AddEffect(effect);

        gameState.Refresh();
        gameState.AddCardToPlayedCardsThisTurn(this);
        gameState.playerChampion.champion.AmountOfCardsPlayed(this);
    }
   
    public virtual string WriteOutCardInfo()
    {
        string lineToWriteOut = null;
        lineToWriteOut = "Cardname: " +cardName + "\nDescription:  " + description + "\nTypeOfCard: " + typeOfCard + "\nMaxMana: " + maxManaCost + 
            "\nAmountOfDamage: " + damage + "\nAmountOfHealing: " + amountToHeal + "\nAmountToShield: " + amountToShield + 
            "\nAmountOfCardsToDraw: " + amountOfCardsToDraw + "\nAmountOfCardsToDiscard: " + amountOfCardsToDiscard + "\nDiscardCardsYourself: " + discardCardsYourself + 
            "\nTargetable: " + targetable + "\nChampionCard: " + championCard + "\nChampionCardType: " + championCardType;
        return lineToWriteOut; 
    }


}
