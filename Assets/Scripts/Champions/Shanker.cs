using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.FullSerializer;

[CreateAssetMenu(fileName = "Shanker", menuName = "Champion/Shanker", order = 1)]
public class Shanker : Champion
{
	public int AttackCardsToPlay = 3;
	public int CardsToDraw = 3;

    public override void EndStep()
	{
		base.EndStep();
		if (gameState.attacksPlayedThisTurn >= AttackCardsToPlay)
		{
			gameState.DrawCard(CardsToDraw, null);
		}
	}

	public override void UpdatePassive()
	{
		if(gameState.opponentChampion.champion != this)
			PassiveEffect = gameState.attacksPlayedThisTurn + "/" + AttackCardsToPlay + " attacks";
    }
}