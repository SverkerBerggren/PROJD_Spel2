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

        for (int i = 0; i < 2; i++)
        {
            Card cardToAdd = actionOfPlayer.handOpponent.DiscardRandomCardInHand();
            graveyard.AddCardToGraveyard(cardToAdd);
        }

        if (gameState.isOnline)
        {
            //Discard for Opponent
            RequestOpponentDiscardCard requesten = new RequestOpponentDiscardCard();
            requesten.whichPlayer = ClientConnection.Instance.playerId;
            requesten.amountOfCardsToDiscard = 2;
            requesten.isRandom = false;
            requesten.discardCardToOpponentGraveyard = true;
            ClientConnection.Instance.AddRequest(requesten, gameState.RequestEmpty);
        }
    }

    private void Digging()
    {
        for (int i = 0; i < amountOfCardsToReturn; i++)
        {
            Tuple<Card,int> cardToDraw = graveyard.RandomizeCardFromGraveyard();
            GameState.Instance.DrawCard(1, cardToDraw.Item1);
        }
    }

    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nAmountOfCardsToReturn: " + amountOfCardsToReturn;
        return lineToWriteOut;
    }
}
