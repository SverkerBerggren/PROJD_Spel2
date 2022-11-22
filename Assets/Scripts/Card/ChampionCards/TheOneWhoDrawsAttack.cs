using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/TheOneWhoDrawsAttack")]
public class TheOneWhoDrawsAttack : Spells
{
    public TheOneWhoDrawsAttack()
    {
        championCard = true;
        championCardType = ChampionCardType.TheOneWhoDraws;
    }
    public override void PlaySpell()
    {
        int damageBoost = ActionOfPlayer.Instance.handPlayer.cardsInHand.Count;
        //OSäker på uträkningen här
        damage *= damageBoost - 10;
        if (damage < 0)       
            damage = 0;
    }
    public override string WriteOutCardInfo()
    {
        string lineToWriteOut = base.WriteOutCardInfo();
        lineToWriteOut += "\nDamage: " + damage;
        return lineToWriteOut;
    }
}
