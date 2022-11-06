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

	public Builder(Builder c) : base(c.name, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect, c.mesh) {}

	public override void Awake()
	{
		base.Awake();
		UpdatePassive();
	}

	public override void AmountOfCardsPlayed(Card c)
	{
		base.AmountOfCardsPlayed(c);
		Debug.Log(landmarkCount + " " + landmarkNeeded);
		if (c.typeOfCard == CardType.Landmark)
		{
			landmarkCount++;
			UpdatePassive();
			if (landmarkCount >= landmarkNeeded && !effectOn)
			{
				ReduceLandmarksInHand();
			}
		}
	}

	public void ReduceLandmarksInHand()
	{
        foreach (GameObject gO in ActionOfPlayer.Instance.handPlayer.cardsInHand)
        {
			Card card = gO.GetComponent<CardDisplay>().card;
			if (card != null)
			{
				if (card.typeOfCard == CardType.Landmark)
				{
					card.manaCost -= cardCostReduce;
					if (card.manaCost < 0)
					{	
						card.manaCost = 0;
					}
				}
			}
        }
		effectOn = true;
	}

	public void ResetLandmarkCost()
	{
        foreach (GameObject gO in ActionOfPlayer.Instance.handPlayer.cardsInHand)
        {
            Card card = gO.GetComponent<CardDisplay>().card;
			if (card != null)
			{
				if (card.typeOfCard == CardType.Landmark)
				{
					card.manaCost += cardCostReduce;
					if (card.manaCost > card.maxManaCost)
					{
						card.manaCost = card.maxManaCost;
					}
				}
			}
        }
		effectOn = false;
    }

	public override void WhenLandmarksDie()
	{
		base.WhenLandmarksDie();
		landmarkCount--;
		if (landmarkCount < landmarkNeeded && effectOn)
		{
			ResetLandmarkCost();
        }
		UpdatePassive();
	}

	public override void DrawCard(Card card)
	{
		base.DrawCard(card);
		if (landmarkCount >= landmarkNeeded && card.typeOfCard == CardType.Landmark)
		{
			card.manaCost -= cardCostReduce;
			if (card.manaCost < 0)
			{
				card.manaCost = 0;
			}
		}
	}

	private void UpdatePassive()
	{
		passiveEffect = landmarkCount + "/" + landmarkNeeded;
		
	}

}
