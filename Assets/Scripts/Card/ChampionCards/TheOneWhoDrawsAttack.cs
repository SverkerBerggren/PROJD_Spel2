using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/TheOneWhoDrawsAttack")]
public class TheOneWhoDrawsAttack : Spells
{
    public int damage = 10;
    public override void PlaySpell()
    {
        int damageBoost = ActionOfPlayer.Instance.handPlayer.cardsInHand.Count;
        //OS�ker p� utr�kningen h�r
        damage *= damageBoost;
    }
}