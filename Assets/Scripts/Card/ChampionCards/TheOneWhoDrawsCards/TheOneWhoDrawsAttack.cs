using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/TheOneWhoDrawsAttack")]
public class TheOneWhoDrawsAttack : Spells
{
    public TheOneWhoDrawsAttack()
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.TheOneWhoDraws;
    }
	protected override void PlaySpell()
    {
        int damageToDeal = Damage;
        int damageBoost = ActionOfPlayer.Instance.HandPlayer.cardsInHand.Count;
     
        damageToDeal = (damageToDeal * damageBoost) - 10;
        if (damageToDeal < 0)
            damageToDeal = 0;

        GameState.Instance.CalculateAndDealDamage(damageToDeal, this);
    }
}
