using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionComparer : IComparer<Champion>
{
	private readonly CardFilter cardFilter;
	public ChampionComparer(CardFilter cardFilter)
	{
		this.cardFilter = cardFilter;
	}

	private int CompareName(Champion x, Champion y)
	{
		var lastNameResult = x.ChampionName.CompareTo(y.ChampionName);

		if (lastNameResult != 0)
			return lastNameResult;

		return x.ChampionName.CompareTo(y.ChampionName);
	}

	private int CompareHealth(Champion x, Champion y)
	{
		if (x.Health == y.Health)
			return CompareName(x, y);

		return x.Health.CompareTo(y.Health);
	}

	public int Compare(Champion x, Champion y)
	{
		return cardFilter switch
		{
			CardFilter.Name => CompareName(x, y),
			CardFilter.Health => CompareHealth(x, y),
			CardFilter.ManaCost => CompareHealth(x, y),
			_ => throw new ArgumentOutOfRangeException(string.Format("Failed to sort champion based on filter", cardFilter)),
		};
	}
}
