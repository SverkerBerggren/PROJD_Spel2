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

	public Builder(Builder c) : base(c.name, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect) {}

	public override void Awake()
	{
		base.Awake();
		UpdatePassive();
	}


	public override void UpdatePassive()
	{
		passiveEffect = landmarkCount + "/" + landmarkNeeded + "  Landmarks";
	}


	public override int CalculateManaCost(CardDisplay cardDisplay)
	{
		landmarkCount = 0;
		foreach (LandmarkDisplay landmark in gameState.playerLandmarks)
		{
			if (landmark.card != null)
				landmarkCount++;
		}

		if (landmarkCount >= landmarkNeeded && cardDisplay.card.typeOfCard == CardType.Landmark)
		{
			cardDisplay.manaCost -= cardCostReduce;
		}
		return cardDisplay.manaCost;

    }
}
