using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/DamageLandmark")]
public class DamageLandmark : Landmarks
{
    public bool IsPyromancyHut = false;
    public DamageLandmark(DamageLandmark card) : base(card.MinionHealth, card.CardName, card.Description, card.MaxManaCost, card.Damage, card.AmountToHeal, card.AmountToShield)
    {
        Damage = card.Damage;
    }

    public override int DealDamageAttack(int damage)
    {
        if (IsPyromancyHut) return damage;
           
        return this.Damage + damage;
    }

    public override void UpKeep()
    {
        base.UpKeep();
        if (!IsPyromancyHut) return;

        ListEnum lE = new ListEnum();
        lE.opponentChampions = true;
        TargetInfo tI = new TargetInfo(lE, 0);
        TargetAndAmount taa = new TargetAndAmount(tI, Damage);
        GameState.Instance.DealDamage(taa);
    }
}
