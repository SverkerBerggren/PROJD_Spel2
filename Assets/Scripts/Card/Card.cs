using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public enum CardType
{
    Spell,
    Landmark
};

public abstract class Card : ScriptableObject
{
    public string cardName;
    public CardType typeOfCard;
    public string description;

    public Sprite artwork;
    public int manaCost;

    public int maxManaCost;
    public string tag;   

    private Champion target;
    private LandmarkDisplay landmarkTarget;

    public int amountOfCardsToDraw = 0;
    public int amountOfCardsToDiscard = 0;
    public bool discardCardsYourself = true;


  
    public Champion Target { get { return target; } set { target = value; } }
    public LandmarkDisplay LandmarkTarget { get { return landmarkTarget; } set { landmarkTarget = value; } }

    protected Card() { maxManaCost = manaCost; }

    public virtual void PlayCard()
    {
        maxManaCost = manaCost;

        CardAndPlacement cardPlacement = new CardAndPlacement();
        cardPlacement.cardName = cardName;
    
        
        TargetInfo placement = new TargetInfo();
        placement.whichList = new ListEnum();
        placement.index = 100;

        if (typeOfCard != CardType.Landmark)
        {
            ListEnum tempEnum = new ListEnum();
            tempEnum.myGraveyard = true;
            placement.whichList = tempEnum;
            placement.index = 100;
        }

        if (GameState.Instance.isOnline)
        {
            RequestPlayCard playCardRequest = new RequestPlayCard(cardPlacement);
            playCardRequest.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(playCardRequest, GameState.Instance.RequestPlayCard);
        }


        
        if (amountOfCardsToDraw != 0)
        {
            GameState.Instance.DrawCard(amountOfCardsToDraw, null);
        }
        if (amountOfCardsToDiscard != 0)
        {
            GameState.Instance.DiscardCard(amountOfCardsToDiscard, discardCardsYourself);
        }
        GameState.Instance.playerChampion.champion.AmountOfCardsPlayed(this);
    }
}
