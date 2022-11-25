using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Shanker/PreparedStrike")]
public class ShankerPreparedStrike : Effects
{
	public int manaReduce = 1;
	public override int CalculateManaCost(CardDisplay cardDisplay)
	{
		if (cardDisplay.card.typeOfCard == cardTrigger)
		{
			return cardDisplay.manaCost - manaReduce;
		}
		return cardDisplay.manaCost;
	}
}
