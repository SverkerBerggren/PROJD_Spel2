using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Landmarks/DamageLandmark")]
public class DamageLandmark : Landmarks
{
    public int damage = 10;
    public DamageLandmark(DamageLandmark card) : base(card.minionHealth, card.cardName, card.description, card.artwork, card.maxManaCost, card.tag)
    {
        this.damage = card.damage;
    }

    public override int DealDamageAttack(int damage)
    {
        return this.damage + damage;
    }
}
