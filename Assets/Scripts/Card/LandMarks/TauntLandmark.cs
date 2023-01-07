using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/TauntLandmark")]
public class TauntLandmark : Landmarks
{    
    public TauntLandmark(TauntLandmark card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield)
    {
        
    }

    public override void PlaceLandmark()
    {
        base.PlaceLandmark();

    }
}
