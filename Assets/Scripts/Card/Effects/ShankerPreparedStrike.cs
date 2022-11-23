using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShankerPreparedStrike : Effects
{
	private int manaReduce;
	public ShankerPreparedStrike(CardType cardType) : base(cardType, true) {}

	public override int CalculateManaCost(CardDisplay cardDisplay)
	{
		if (cardDisplay.card.typeOfCard == cardTrigger)
		{
			return cardDisplay.manaCost - manaReduce;
		}
		return cardDisplay.manaCost;
	}
}
