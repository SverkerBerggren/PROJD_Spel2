using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/BuilderLandmark")]
public class BuilderLandmark : Landmarks
{
    public bool slaughterhouse = false;
    public bool factory = false;
    public BuilderLandmark(BuilderLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.damage, card.amountToHeal, card.amountToShield) 
    { 
        championCard = true;
        championCardType = ChampionCardType.Builder;
    }

    public override int CalculateManaCost(CardDisplay cardDisplay)
    {
        int amountOfLandmarksActive = 0;
        foreach (LandmarkDisplay lD in GameState.Instance.playerLandmarks)
        {
            if (lD.card != null)
                amountOfLandmarksActive++;
        }
        if (amountOfLandmarksActive >= 3)
            return base.CalculateManaCost(cardDisplay) - 2;
        return base.CalculateManaCost(cardDisplay);
    }

    public override int DealDamageAttack(int damage)
    {
        if (slaughterhouse)
        {
            return damage + this.damage;
        }

        return base.DealDamageAttack(damage) ;
    }
}
