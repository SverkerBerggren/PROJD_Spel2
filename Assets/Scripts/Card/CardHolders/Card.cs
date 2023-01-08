using System;
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
    private LandmarkDisplay landmarkTarget;
    private Champion target;

    public string CardName;
    public CardType TypeOfCard;
    public string Description;
    public int MaxManaCost;


    [Header("Atributes")]
    public int Damage = 0;
    public int AmountToHeal = 0;
    public int AmountToShield = 0;
    public int AmountOfCardsToDraw = 0;
    public int AmountOfCardsToDiscard = 0;

    [Header("Effect")]
    public Effects Effect;

    public bool DiscardCardsYourself = true;
    public bool Targetable = false;

    public bool ChampionCard = false;
    public ChampionCardType ChampionCardType = ChampionCardType.None;

    [NonSerialized] public bool PurchasedFormShop = false;

  
    public Champion Target { get { return target; } set { target = value; } }
    public LandmarkDisplay LandmarkTarget { get { return landmarkTarget; } set { landmarkTarget = value; } }

    protected Card() { }

    public virtual void PlayCard()
    {
        CardAndPlacement cardPlacement = new CardAndPlacement();
        cardPlacement.CardName = CardName;
        GameState gameState = GameState.Instance;
        
        TargetInfo placement = new TargetInfo();
        placement.whichList = new ListEnum();

        if (PurchasedFormShop)
            placement.whichList.opponentGraveyard = false;
        else
            placement.whichList.opponentGraveyard = true;
        
        cardPlacement.Placement = placement;

        gameState.ShowPlayedCard(this, false, -1);
        if (gameState.IsOnline)        
            gameState.PlayCardRequest(cardPlacement);
        
        if (AmountOfCardsToDraw != 0)       
            gameState.DrawCard(AmountOfCardsToDraw, null);
        
        if (AmountOfCardsToDiscard != 0)     
            gameState.DiscardCard(AmountOfCardsToDiscard, DiscardCardsYourself);
        
        if (Effect != null)
            gameState.AddEffect(Effect);

        gameState.Refresh();
        gameState.AddCardToPlayedCardsThisTurn(this);
        gameState.PlayerChampion.Champion.AmountOfCardsPlayed(this);
    }
   
    public virtual string WriteOutCardInfo()
    {
        string lineToWriteOut = null;
        lineToWriteOut = "Cardname: " +CardName + "\nDescription:  " + Description + "\nTypeOfCard: " + TypeOfCard + "\nMaxMana: " + MaxManaCost + 
            "\nAmountOfDamage: " + Damage + "\nAmountOfHealing: " + AmountToHeal + "\nAmountToShield: " + AmountToShield + 
            "\nAmountOfCardsToDraw: " + AmountOfCardsToDraw + "\nAmountOfCardsToDiscard: " + AmountOfCardsToDiscard + "\nDiscardCardsYourself: " + DiscardCardsYourself + 
            "\nTargetable: " + Targetable + "\nChampionCard: " + ChampionCard + "\nChampionCardType: " + ChampionCardType;
        return lineToWriteOut; 
    }


}
