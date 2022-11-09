using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Builder", menuName = "Champion/Builder", order = 1)]
public class Builder : Champion
{
	public int landmarkCount = 0;
	public int landmarkNeeded = 2;
	public int cardCostReduce = 2;
	public bool effectOn = false;

	public Builder(Builder c) : base(c.name, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect) {}

	public override void Awake()
	{
		base.Awake();
		UpdatePassive();
	}

	public override void AmountOfCardsPlayed(Card c)
	{
		base.AmountOfCardsPlayed(c);
		if (c.typeOfCard == CardType.Landmark)
		{
			landmarkCount++;
			UpdatePassive();
			if (landmarkCount >= landmarkNeeded && !effectOn)
				ReduceLandmarksInHand();
		}
	}

	public override void WhenLandmarksDie()
	{
		base.WhenLandmarksDie();
		landmarkCount--;
		if (landmarkCount < landmarkNeeded && effectOn)
			ResetLandmarkCost();
		UpdatePassive();
	}

	public override void DrawCard(CardDisplay cardDisplay)
	{
		base.DrawCard(cardDisplay);
		if (landmarkCount >= landmarkNeeded && cardDisplay.card.typeOfCard == CardType.Landmark)
		{
			cardDisplay.manaCost -= cardCostReduce;
			if (cardDisplay.manaCost < 0)
			{
				cardDisplay.manaCost = 0;
			}
		}
	}

	public override void WhenCurrentChampion()
	{
		base.WhenCurrentChampion();
		landmarkCount = 0;
		foreach (LandmarkDisplay landmark in gameState.playerLandmarks)
		{
			if (landmark.card != null)
				landmarkCount++;
		}
		if (landmarkCount >= landmarkNeeded)
			ReduceLandmarksInHand();
		UpdatePassive();
	}

	public override void WhenInactiveChampion()
	{
		if (effectOn)
			ResetLandmarkCost();
	}

	private void UpdatePassive()
	{
		passiveEffect = landmarkCount + "/" + landmarkNeeded;	
	}

	private void ReduceLandmarksInHand()
	{
        foreach (GameObject gO in ActionOfPlayer.Instance.handPlayer.cardsInHand)
        {
			CardDisplay cardDisplay = gO.GetComponent<CardDisplay>();
			if (cardDisplay.card != null)
			{
				if (cardDisplay.card.typeOfCard == CardType.Landmark)
				{
					cardDisplay.manaCost -= cardCostReduce;
					if (cardDisplay.manaCost < 0)
						cardDisplay.manaCost = 0;
				}
			}
        }
		effectOn = true;
	}

	private void ResetLandmarkCost()
	{
        foreach (GameObject gO in ActionOfPlayer.Instance.handPlayer.cardsInHand)
        {
			CardDisplay cardDisplay = gO.GetComponent<CardDisplay>();
			if (cardDisplay.card != null)
			{
				if (cardDisplay.card.typeOfCard == CardType.Landmark)
				{
					cardDisplay.manaCost += cardCostReduce;
					if (cardDisplay.manaCost > cardDisplay.card.maxManaCost)
						cardDisplay.manaCost = cardDisplay.card.maxManaCost;
				}
			}
        }
		effectOn = false;
    }
}
