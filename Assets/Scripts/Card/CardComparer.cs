using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardComparer : IComparer<Card>
{
	private CardFilter cardFilter;
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
			return CompareHealth(x, y);

			default:
				throw new ArgumentOutOfRangeException(string.Format("Failed to sort card based on filter", cardFilter));
		}
	}

	public int CompareName(Card x, Card y)
	{
		var lastNameResult = x.cardName.CompareTo(y.cardName);

		if (lastNameResult != 0)
			return lastNameResult;

		return x.cardName.CompareTo(y.cardName);
	}

	public int CompareManaCost(Card x, Card y)
	{
		if(x.maxManaCost == y.maxManaCost)
			return CompareName(x, y);

		return x.maxManaCost.CompareTo(y.maxManaCost);
	}

	public int CompareHealth(Card x, Card y)
	{
		if (y is Landmarks == false || x is Landmarks == false)
		{
			if (x is Landmarks) return 0;
			if (y is Landmarks) return 1;

			return CompareManaCost(x, y);
		}

		Landmarks landmarkX = x as Landmarks;
		Landmarks landmarkY = y as Landmarks;

		if (landmarkX.minionHealth == landmarkY.minionHealth)
			return CompareManaCost(x, y);
		return landmarkX.minionHealth.CompareTo(landmarkY.minionHealth);
	}
}

public enum CardFilter
{
	Name,
	ManaCost,
	Health
}
