using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/CultistLandmark")]
public class CultistLandmark : Landmarks
{
    public int damage = 10;

    public CultistLandmark(CultistLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.tag) 
    {
        championCard = true;
        championCardType = ChampionCardType.Cultist;
    }

    public override int DealDamageAttack(int damage)
    {
        return damage + GameState.Instance.attacksPlayedThisTurn * this.damage;
    }

    public override string ToString()
    {
        base.ToString();
        string lineToWriteOut = null;
        lineToWriteOut = "\nDamage: " + damage;
        return lineToWriteOut;
    }
}
