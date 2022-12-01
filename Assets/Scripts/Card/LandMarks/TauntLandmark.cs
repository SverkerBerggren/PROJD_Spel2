using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/TauntLandmark")]
public class TauntLandmark : Landmarks
{    
    public TauntLandmark(TauntLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.damage, card.amountToHeal, card.amountToShield)
    {
        
    }

    public override void PlaceLandmark()
    {
        base.PlaceLandmark();

    }
}
