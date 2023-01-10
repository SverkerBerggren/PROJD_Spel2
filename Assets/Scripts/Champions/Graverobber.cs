using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Graverobber", menuName = "Champion/Graverobber", order = 1)]
public class Graverobber : Champion
{
    public override void EndStep()
	{
		base.EndStep();
		if (Graveyard.Instance.GraveyardPlayer.Count == 0) return;

		// Draws the remove card to the players hand
		Tuple<Card, int> info = Graveyard.Instance.RandomizeCardFromGraveyard();
		ActionOfPlayer.Instance.DrawCardPlayer(1, info.Item1, true);

		if (GameState.Instance.IsOnline)
		{
			TargetInfo targetInfo = new TargetInfo();
			targetInfo.whichList.myGraveyard = true;
			targetInfo.index = info.Item2;

			// Removes a card from your graveyard
			RequestRemoveCardsGraveyard request = new RequestRemoveCardsGraveyard();
			request.whichPlayer = ClientConnection.Instance.playerId;
			List<TargetInfo> list = new List<TargetInfo>() { targetInfo };
			request.cardsToRemoveGraveyard = list;
			ClientConnection.Instance.AddRequest(request, GameState.Instance.RequestEmpty);


			// Shows the opponent that you removed a card from your graveyard
			RequestAddSpecificCardToHand requestAddSpecificCardToHand = new RequestAddSpecificCardToHand();
			requestAddSpecificCardToHand.whichPlayer = ClientConnection.Instance.playerId;
			requestAddSpecificCardToHand.cardToAdd = info.Item1.CardName;
			ClientConnection.Instance.AddRequest(requestAddSpecificCardToHand, GameState.Instance.RequestEmpty);

		}

	}
}
