using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/GraverobberSpells")]
public class GraverobberSpells : Spells
{
    public int AmountOfCardsToReturn;
    public bool GraveGrief = false;
    public bool Digging = false;

    private Graveyard graveyard;
    public GraverobberSpells()
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Graverobber;
    }

    public override void PlaySpell()
    {
        graveyard = Graveyard.Instance;
        if (GraveGrief)
        {
            GraveGriefActivate();
        }
        else if (Digging)
        {
            DiggingActivate();
        }
    }

    private void GraveGriefActivate()
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

    private void DiggingActivate()
    {
		Deck deck = Deck.Instance;
		List<TargetInfo> tIList = new List<TargetInfo>();

        graveyard.FindAndRemoveCardInGraveyard(this);

        for (int i = 0; i < AmountOfCardsToReturn; i++)
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
        lineToWriteOut += "\nAmountOfCardsToReturn: " + AmountOfCardsToReturn;
        return lineToWriteOut;
    }
}
