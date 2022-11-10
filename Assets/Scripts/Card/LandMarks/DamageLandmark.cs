using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/DamageLandmark")]
public class DamageLandmark : Landmarks
{
    public int damage = 10;
    public bool isPyromancyHut = false;
    public DamageLandmark(DamageLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.tag)
    {
        this.damage = card.damage;
    }

    public override int DealDamageAttack(int damage)
    {
        return this.damage + damage;
    }

    public override void UpKeep()
    {
        base.UpKeep();
        if (!isPyromancyHut) return;

        ListEnum lE = new ListEnum();
        lE.opponentChampions = true;
        TargetInfo tI = new TargetInfo(lE, 0);
        TargetAndAmount taa = new TargetAndAmount(tI, damage);

        GameState.Instance.DealDamage(taa);
    }
}
