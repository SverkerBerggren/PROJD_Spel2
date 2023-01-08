using UnityEngine;

[CreateAssetMenu(fileName = "Builder", menuName = "Champion/Builder", order = 1)]
public class Builder : Champion
{
	private int LandmarkCount = 0;
	public int LandmarkNeeded = 2;
	public int CardCostReduce = 2;

    public override void Awake()
	{
		base.Awake();
		UpdatePassive();
	}

	public override void UpdatePassive()
	{
		PassiveEffect = LandmarkCount + "/" + LandmarkNeeded + "  Landmarks";
	}

	public override int CalculateManaCost(CardDisplay cardDisplay)
	{
		LandmarkCount = 0;
		foreach (LandmarkDisplay landmark in gameState.PlayerLandmarks)
		{
			if (landmark.Card != null)
				LandmarkCount++;
		}

		if (LandmarkCount >= LandmarkNeeded && cardDisplay.Card.TypeOfCard == CardType.Landmark)
			cardDisplay.ManaCost -= CardCostReduce;
		return cardDisplay.ManaCost;
    }
}
