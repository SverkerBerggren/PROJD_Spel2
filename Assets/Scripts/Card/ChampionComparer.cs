using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionComparer : IComparer<Champion>
{
	private CardFilter cardFilter;
	public ChampionComparer(CardFilter cardFilter)
	{
		this.cardFilter = cardFilter;
	}

	public int Compare(Champion x, Champion y)
	{
		switch (cardFilter)
		{
			case CardFilter.Name:
			return CompareName(x, y);

			case CardFilter.Health:
			return CompareHealth(x, y);

			case CardFilter.ManaCost:
			return CompareName(x, y);

			default:
				throw new ArgumentOutOfRangeException(string.Format("Failed to sort champion based on filter", cardFilter));
		}
	}

	public int CompareName(Champion x, Champion y)
	{
		var lastNameResult = x.championName.CompareTo(y.championName);

		if (lastNameResult != 0)
			return lastNameResult;

		return x.championName.CompareTo(y.championName);
	}

	public int CompareHealth(Champion x, Champion y)
	{
		if(x.health == y.health)
			return CompareName(x, y);

		return x.health.CompareTo(y.health);
	}
}
