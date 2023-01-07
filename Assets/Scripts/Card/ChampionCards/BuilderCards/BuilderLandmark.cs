using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/BuilderLandmark")]
public class BuilderLandmark : Landmarks
{
    public bool Slaughterhouse = false;
    public bool Factory = false;
    public BuilderLandmark(BuilderLandmark card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield) 
    { 
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Builder;
    }

    private int LandmarksActive()
    {
        int amountOfLandmarksActive = 0;
        foreach (LandmarkDisplay lD in GameState.Instance.playerLandmarks)
        {
            if (lD.Card != null)
                amountOfLandmarksActive++;
        }
        return amountOfLandmarksActive;
    }

    public override int CalculateManaCost(CardDisplay cardDisplay)
    {
        if (Factory && LandmarksActive() >= 3)
            return base.CalculateManaCost(cardDisplay) - 2;            
        
        return base.CalculateManaCost(cardDisplay);
    }

    public override int DealDamageAttack(int damage)
    {   
        if (Slaughterhouse)
        {
            return damage + (this.Damage * LandmarksActive());
        }

        return base.DealDamageAttack(damage) ;
    }
}
