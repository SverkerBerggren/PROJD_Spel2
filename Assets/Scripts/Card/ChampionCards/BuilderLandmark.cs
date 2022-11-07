using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/BuilderLandmark")]
public class BuilderLandmark : Landmarks
{
    public int damagePerLandmark = 10;
    public bool slaughterhouse = false;
    public bool factory = false;
    public BuilderLandmark(BuilderLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.tag) { }

    public override void PlaceLandmark()
    {
        base.PlaceLandmark();
        if (factory)
            GameState.Instance.factory++;
    }

    public override void LandmarkEffectTakeBack()
    {
        if (factory)
            GameState.Instance.factory--;
    }

    public override int DealDamageAttack(int damage)
    {
        if (slaughterhouse)
        {
            return damage + damagePerLandmark;
        }

        return base.DealDamageAttack(damage) ;
    }
}
