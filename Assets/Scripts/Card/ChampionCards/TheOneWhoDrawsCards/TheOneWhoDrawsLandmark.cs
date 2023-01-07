using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/TheOneWhoDrawsLandmark")]
public class TheOneWhoDrawsLandmark : Landmarks
{
    private bool preparation = false;
    public int AmountOfManaToDecreaseBy = 2;

    public TheOneWhoDrawsLandmark(TheOneWhoDrawsLandmark card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield)
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.TheOneWhoDraws;
    }

	public override void EndStep()
	{
		base.EndStep();
        preparation = true;
	}

	public override int CalculateManaCost(CardDisplay cardDisplay)
    {
        base.CalculateManaCost(cardDisplay);
        if (cardDisplay.firstCardDrawn && preparation)
            return cardDisplay.manaCost - AmountOfManaToDecreaseBy;

        return cardDisplay.manaCost;
    }
}
