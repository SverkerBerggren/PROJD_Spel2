using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/DisableCardLandmark")]
public class DisableCardLandmark : Landmarks
{
    [NonSerialized] public Landmarks DisabledLandmark;
    public DisableCardLandmark(DisableCardLandmark card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield) {}

	public override void PlaceLandmark()
	{
		base.PlaceLandmark();
		ListEnum lE = new ListEnum();
		lE.opponentLandmarks = true;
		Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.DisableOpponentLandmark, this);
	}

	public override void WhenLandmarksDie()
	{
		if (DisabledLandmark != null)
		{
			GameState gameState = GameState.Instance;
			TargetInfo info = new TargetInfo();
			info.whichList.opponentLandmarks = true;
			info.index = -1;
			for (int i = 0; i < gameState.OpponentLandmarks.Count; i++)
			{
				if (gameState.OpponentLandmarks[i].Card != null && DisabledLandmark.Equals(gameState.OpponentLandmarks[i]))
				{
					info.index = i;
				}
			}

			if (info.index == -1) return;

			gameState.ChangeLandmarkStatus(info, true);
		}
	}
}
