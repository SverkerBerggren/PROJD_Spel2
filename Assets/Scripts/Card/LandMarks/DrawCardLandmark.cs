using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/DrawCardLandmark")]
public class DrawCardLandmark : Landmarks
{
    public bool mysteriousForest = false;
    public DrawCardLandmark(DrawCardLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.damage, card.amountToHeal, card.amountToShield) {}

    public override void UpKeep()
    {
        base.UpKeep();
        GameState.Instance.DrawCard(1, null);
        if (mysteriousForest && GameState.Instance.amountOfTurns >= 10)
        {
			GameState.Instance.DrawCard(5, null);
            foreach (LandmarkDisplay l in GameState.Instance.playerLandmarks)
            {
                if (l.card is DrawCardLandmark && (DrawCardLandmark)l.card)
                {
                    l.DestroyLandmark();
                }

            }
		}
    }
}
