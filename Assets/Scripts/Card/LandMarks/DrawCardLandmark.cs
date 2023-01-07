using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/DrawCardLandmark")]
public class DrawCardLandmark : Landmarks
{
    public bool MysteriousForest = false;
    public DrawCardLandmark(DrawCardLandmark card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield) {}

    public override void UpKeep()
    {
        base.UpKeep();
        GameState.Instance.DrawCard(1, null);
        if (MysteriousForest && GameState.Instance.amountOfTurns >= 10)
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
