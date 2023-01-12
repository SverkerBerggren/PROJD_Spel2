using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/CultistLandmark")]
public class CultistLandmark : Landmarks
{
    public CultistLandmark(CultistLandmark card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield) 
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Cultist;
    }

    public override int DealDamageAttack(int damage)
    {
        return damage + GameState.Instance.AttacksPlayedThisTurn * Damage;
    }
}
