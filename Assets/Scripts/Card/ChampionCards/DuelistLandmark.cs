using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/DuelistLandmark")]
public class DuelistLandmark : Landmarks
{
    public DuelistLandmark(DuelistLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.manaCost, card.tag)
    {

    }

    public override void PlaceLandmark()
    {

    }

    public override void LandmarkEffectTakeBack()
    {
        base.LandmarkEffectTakeBack();

    }
}
