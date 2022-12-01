using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/DisableCardLandmark")]
public class DisableCardLandmark : Landmarks
{
    public DisableCardLandmark(DisableCardLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.damage, card.amountToHeal, card.amountToShield)
    {

    }
    public Landmarks disabledLandmark;

	public override void PlaceLandmark()
	{
		base.PlaceLandmark();
		ListEnum lE = new ListEnum();
		lE.opponentLandmarks = true;
		Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.DisableOpponentLandmark, this);
	}

	public override void WhenLandmarksDie()
	{
		if (disabledLandmark != null)
		{
			GameState gameState = GameState.Instance;
			TargetInfo info = new TargetInfo();
			info.whichList.opponentLandmarks = true;
			info.index = -1;
			for (int i = 0; i < gameState.opponentLandmarks.Count; i++)
			{
				if (gameState.opponentLandmarks[i].card != null && disabledLandmark.Equals(gameState.opponentLandmarks[i]))
				{
					info.index = i;
				}
			}
			gameState.ChangeLandmarkStatus(info, true);
		}
	}
}
