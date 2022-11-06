using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/DrawCardLandmark")]
public class DrawCardLandmark : Landmarks
{

    public bool destroyOnRoundTen = false;
    public DrawCardLandmark(DrawCardLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.manaCost, card.tag)
    {
        destroyOnRoundTen = card.destroyOnRoundTen;
    }

    public override void PlaceLandmark()
    {
        base.PlaceLandmark();
        GameState.Instance.drawExtraCardsEachTurn = true;

        if (destroyOnRoundTen)
        {

        }
    }

    public override void LandmarkEffectTakeBack()
    {
        base.LandmarkEffectTakeBack();

        GameState.Instance.drawExtraCardsEachTurn = false;

        if (destroyOnRoundTen)
        {

        }
    }
}
