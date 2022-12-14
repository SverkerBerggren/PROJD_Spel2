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

	protected override void PlaySpell()
    {
        graveyard = Graveyard.Instance;
        if (GraveGrief)
            GraveGriefActivate();
        else if (Digging)
            DiggingActivate();
    }

    private void GraveGriefActivate()
    {
        ActionOfPlayer actionOfPlayer = ActionOfPlayer.Instance;
        Graveyard graveyard = Graveyard.Instance;
        GameState gameState = GameState.Instance;
        int discardedCards = 2;


        if (gameState.IsOnline)
        {
            //Discard for Opponent
            RequestOpponentDiscardCard requesten = new RequestOpponentDiscardCard();
            requesten.whichPlayer = ClientConnection.Instance.playerId;
            requesten.amountOfCardsToDiscard = discardedCards;
            requesten.isRandom = true;
            requesten.discardCardToOpponentGraveyard = true;
            ClientConnection.Instance.AddRequest(requesten, gameState.RequestEmpty);
        }
        else
        {
			List<Card> cards = actionOfPlayer.HandOpponent.DiscardMultipleRandomCards(discardedCards, false);
			foreach (Card card in cards)
			{
				graveyard.AddCardToGraveyard(card);
			}
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

		if (GameState.Instance.IsOnline)
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
