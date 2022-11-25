using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "Graverobber", menuName = "Champion/Graverobber", order = 1)]
public class Graverobber : Champion
{
	public Graverobber(Graverobber c) : base(c.championName, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect) { }

	public override void EndStep()
	{
		base.EndStep();
		if (Graveyard.Instance.graveyardPlayer.Count == 0) return;
		Tuple<Card, int> info = Graveyard.Instance.RandomizeCardFromGraveyard();
		ActionOfPlayer.Instance.DrawCardPlayer(1, info.Item1, true);
		if (GameState.Instance.isOnline)
		{
			TargetInfo targetInfo = new TargetInfo();
			targetInfo.whichList.myGraveyard = true;
			targetInfo.index = info.Item2;
			RequestRemoveCardsGraveyard request = new RequestRemoveCardsGraveyard();
			request.whichPlayer = ClientConnection.Instance.playerId;
			List<TargetInfo> list = new List<TargetInfo>();
			list.Add(targetInfo);
			request.cardsToRemoveGraveyard = list;
			ClientConnection.Instance.AddRequest(request, GameState.Instance.RequestEmpty);


			RequestAddSpecificCardToHand requestAddSpecificCardToHand = new RequestAddSpecificCardToHand();
			requestAddSpecificCardToHand.whichPlayer = ClientConnection.Instance.playerId;

			requestAddSpecificCardToHand.cardToAdd = info.Item1.cardName;

			ClientConnection.Instance.AddRequest(requestAddSpecificCardToHand, GameState.Instance.RequestEmpty);

		}

	}
}
