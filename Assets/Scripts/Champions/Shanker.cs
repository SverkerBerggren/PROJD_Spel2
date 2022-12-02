using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.FullSerializer;

[CreateAssetMenu(fileName = "Shanker", menuName = "Champion/Shanker", order = 1)]
public class Shanker : Champion
{
	public int attackCardsToPlay = 3;
	public int cardsToDraw = 3;

    public override void EndStep()
	{
		base.EndStep();
		if (gameState.attacksPlayedThisTurn >= attackCardsToPlay)
		{
			gameState.DrawCard(cardsToDraw, null);
		}
	}

	public override void UpdatePassive()
	{
		if(gameState.opponentChampion.champion != this)
			passiveEffect = gameState.attacksPlayedThisTurn + "/" + attackCardsToPlay + " attacks";
    }
}