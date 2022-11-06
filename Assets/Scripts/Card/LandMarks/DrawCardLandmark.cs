using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/DrawCardLandmark")]
public class DrawCardLandmark : Landmarks
{
    public DrawCardLandmark(DrawCardLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.manaCost, card.tag)
    {

    }

    public override void PlaceLandmark()
    {

    }

    public override void LandmarkEffectTakeBack()
    {

    }

    public override void UpKeep()
    {
        base.UpKeep();
        GameState.Instance.DrawCard(1, null);
    }
}
