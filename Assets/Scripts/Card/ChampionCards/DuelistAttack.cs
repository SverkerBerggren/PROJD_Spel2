using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/DuelistAttack")]
public class DuelistAttack : Spells
{
    private GameState gameState;

    public DuelistAttack()
    {
        championCard = true;
        championCardType = ChampionCardType.Duelist;
    }

    public override void PlaySpell()
    {
        gameState = GameState.Instance;
        int newDamage = Calculations.Instance.CalculateDamage(damage);
        //if (Target == null && newDamage >= Target.)
        gameState.CalculateAndDealDamage(damage, this);

        ListEnum lE = new ListEnum();
        lE.opponentChampions = true;      

        Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampionEnemy, null);

        gameState.CalculateAndDealDamage(damage, this);
    }
}
