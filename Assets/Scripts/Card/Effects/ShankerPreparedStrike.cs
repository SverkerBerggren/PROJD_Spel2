using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Shanker/PreparedStrike")]
public class ShankerPreparedStrike : Effects
{
	public int ManaReduce = 1;

	public override void AddEffect() {}

	public override int CalculateManaCost(CardDisplay cardDisplay)
	{
		if (cardDisplay.Card.TypeOfCard == CardTrigger)
		{
			return cardDisplay.ManaCost - ManaReduce;
		}
		return cardDisplay.ManaCost;
	}
}
