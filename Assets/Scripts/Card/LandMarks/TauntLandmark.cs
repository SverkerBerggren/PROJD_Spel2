using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/TauntLandmark")]
public class TauntLandmark : Landmarks
{    
    public TauntLandmark(TauntLandmark card) : base(card.minionHealth, card.name, card.description, card.artwork, card.manaCost, card.tag)
    {
        
    }

    public override void PlaceLandmark()
    {
        ActionOfPlayer.Instance.tauntPlaced++;
    }

    public override void LandmarkEffectTakeBack()
    {
        base.LandmarkEffectTakeBack();
        ActionOfPlayer.Instance.tauntPlaced--;
    }
}
