using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/CultistLandmark")]
public class CultistLandmark : Landmarks
{
    public CultistLandmark(CultistLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.tag) { }

    public override void PlaceLandmark()
    {
        base.PlaceLandmark();
        GameState.Instance.occultGathering++;
    }

    public override void LandmarkEffectTakeBack()
    {
            GameState.Instance.occultGathering--;
    }
}
