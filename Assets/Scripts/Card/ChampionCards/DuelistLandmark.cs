using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/DuelistLandmark")]
public class DuelistLandmark : Landmarks
{
    public DuelistLandmark(DuelistLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.tag)
    {

    }

    public override void PlaceLandmark()
    {
        base.PlaceLandmark();
    }

    public override void LandmarkEffectTakeBack()
    {
        base.LandmarkEffectTakeBack();

    }
}
