using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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
        int damageToDeal = damage;
        int damageBoost = ActionOfPlayer.Instance.handPlayer.cardsInHand.Count;
     
        damageToDeal = (damageToDeal * damageBoost) - 10;
        if (damageToDeal < 0)
            damageToDeal = 0;

        GameState.Instance.CalculateAndDealDamage(damageToDeal, this);
    }
}
