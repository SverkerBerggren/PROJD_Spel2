using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/TheOneWhoDrawsLandmark")]
public class TheOneWhoDrawsLandmark : Landmarks
{
    public int amountOfManaToDecreaseBy = 2;

    public TheOneWhoDrawsLandmark(TheOneWhoDrawsLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.tag, card.damage, card.amountToHeal, card.amountToShield)
    {
        championCard = true;
        championCardType = ChampionCardType.TheOneWhoDraws;
    }

    public override void UpKeep()
    {
        base.UpKeep();


    }

    public override int CalculateManaCost(CardDisplay cardDisplay)
    {
        base.CalculateManaCost(cardDisplay);
        if (cardDisplay.firstCardDrawn)
            return cardDisplay.manaCost - amountOfManaToDecreaseBy;

        return cardDisplay.manaCost;
    }
}
