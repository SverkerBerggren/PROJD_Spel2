using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/GraverobberSpells")]
public class GraverobberSpells : Spells
{
    public int amountOfCardsToReturn;
    public bool graveGrief = false;
    public bool digging = false;

    private Graveyard graveyard;
    public GraverobberSpells()
    {
        championCard = true;
        championCardType = ChampionCardType.Graverobber;
    }

    public override void PlaySpell()
    {
        graveyard = Graveyard.Instance;
        if (graveGrief)
        {
            GraveGrief();
        }
        else if (digging)
        {
            Digging();
        }
    }

    private void GraveGrief()
    {
        ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;
        Graveyard graveyard = Graveyard.Instance;
        GameState gameState = GameState.Instance;
        int discardedCards = 0;
        for (int i = 0; i < 2; i++)
        {
            if (actionOfPlayer.handOpponent.DiscardRandomCardInHand() != null)
                discardedCards++;
        }

        if (gameState.isOnline)
        {
            //Discard for Opponent
            RequestOpponentDiscardCard requesten = new RequestOpponentDiscardCard();
            requesten.whichPlayer = ClientConnection.Instance.playerId;
            requesten.amountOfCardsToDiscard = discardedCards;
            requesten.isRandom = false;
            requesten.discardCardToOpponentGraveyard = true;
            ClientConnection.Instance.AddRequest(requesten, gameState.RequestEmpty);
        }
    }

    private void Digging()
    {
		Deck deck = Deck.Instance;
		List<TargetInfo> tIList = new List<TargetInfo>();

        graveyard.FindAndRemoveCardInGraveyard(this);

        for (int i = 0; i < amountOfCardsToReturn; i++)
        {
            Tuple<Card,int> cardToDraw = graveyard.RandomizeCardFromGraveyard();
            if (cardToDraw == null) break;

            TargetInfo tI = new TargetInfo();
            tI.whichList.myGraveyard = true;
            tI.index = cardToDraw.Item2;
            tIList.Add(tI);

            GameState.Instance.DrawCard(1, cardToDraw.Item1);
        }

		graveyard.AddCardToGraveyard(this);

		if (GameState.Instance.isOnline)
        {
            RequestRemoveCardsGraveyard requesten = new RequestRemoveCardsGraveyard();
            requesten.whichPlayer = ClientConnection.Instance.playerId;
            requesten.cardsToRemoveGraveyard = tIList;
            ClientConnection.Instance.AddRequest(requesten, GameState.Instance.RequestEmpty);
        }
        
    }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nAmountOfCardsToReturn: " + amountOfCardsToReturn;
        return lineToWriteOut;
    }
}
