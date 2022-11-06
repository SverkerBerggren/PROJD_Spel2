using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/DamageLandmark")]
public class DamageLandmark : Landmarks
{
    public DamageLandmark(DamageLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.manaCost, card.tag)
    {

    }

    public override void PlaceLandmark()
    {
        GameState.Instance.tenExtraDamage += 1;
        LandmarkTarget.tenExtraDamage += 1;
    }
    public override void LandmarkEffectTakeBack()
    {
        GameState.Instance.tenExtraDamage -= 1;
        LandmarkTarget.tenExtraDamage -= 1;
    }

    

}
