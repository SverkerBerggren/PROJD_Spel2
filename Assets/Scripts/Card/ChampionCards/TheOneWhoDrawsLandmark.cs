using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/TheOneWhoDrawsLandmark")]
public class TheOneWhoDrawsLandmark : Landmarks
{
    public int amountOfManaToDecreaseBy = 2;
    private bool preparation = false;

    public TheOneWhoDrawsLandmark(TheOneWhoDrawsLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.tag, card.damage, card.amountToHeal, card.amountToShield)
    {
        championCard = true;
        championCardType = ChampionCardType.TheOneWhoDraws;
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
            return cardDisplay.manaCost - amountOfManaToDecreaseBy;

        return cardDisplay.manaCost;
    }
}
