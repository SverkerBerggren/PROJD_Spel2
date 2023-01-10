using System;
using System.Collections.Generic;

public class CardComparer : IComparer<Card>
{
	private readonly CardFilter cardFilter;
	public CardComparer(CardFilter cardFilter)
	{
		this.cardFilter = cardFilter;
	}

	private int CompareName(Card x, Card y)
	{
		var lastNameResult = x.CardName.CompareTo(y.CardName);

		if (lastNameResult != 0)
			return lastNameResult;

		return x.CardName.CompareTo(y.CardName);
	}

	private int CompareManaCost(Card x, Card y)
	{
		if(x.MaxManaCost == y.MaxManaCost)
			return CompareName(x, y);

		return x.MaxManaCost.CompareTo(y.MaxManaCost);
	}

	private int CompareHealth(Landmarks x, Landmarks y)
	{
		if (x.MinionHealth == y.MinionHealth)
			return CompareManaCost(x, y);

		return x.MinionHealth.CompareTo(y.MinionHealth);
	}

	public int Compare(Card x, Card y)
	{
		switch (cardFilter)
		{
			case CardFilter.Name:
			return CompareName(x, y);

			case CardFilter.ManaCost:
			return CompareManaCost(x, y);

			case CardFilter.Health:
			if(x is not Landmarks || y is not Landmarks)
				throw new ArgumentOutOfRangeException(string.Format("Cant not sort anything except landmark cards", cardFilter));
			else
				return CompareHealth((Landmarks)x, (Landmarks)y);

			default:
				throw new ArgumentOutOfRangeException(string.Format("Failed to sort card based on filter", cardFilter));
		}
	}

}

public enum CardFilter
{
	Name,
	ManaCost,
	Health,
}
