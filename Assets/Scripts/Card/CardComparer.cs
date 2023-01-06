using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardComparer : IComparer<Card>
{
	private readonly CardFilter cardFilter;
	public CardComparer(CardFilter cardFilter)
	{
		this.cardFilter = cardFilter;
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

	private int CompareName(Card x, Card y)
	{
		var lastNameResult = x.cardName.CompareTo(y.cardName);

		if (lastNameResult != 0)
			return lastNameResult;

		return x.cardName.CompareTo(y.cardName);
	}

	private int CompareManaCost(Card x, Card y)
	{
		if(x.maxManaCost == y.maxManaCost)
			return CompareName(x, y);

		return x.maxManaCost.CompareTo(y.maxManaCost);
	}

	private int CompareHealth(Landmarks x, Landmarks y)
	{
		if (x.minionHealth == y.minionHealth)
			return CompareManaCost(x, y);

		return x.minionHealth.CompareTo(y.minionHealth);
	}
}

public enum CardFilter
{
	Name,
	ManaCost,
	Health,
}
