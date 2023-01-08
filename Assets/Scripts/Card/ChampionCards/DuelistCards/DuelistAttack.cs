using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/ChampionCards/DuelistAttack")]
public class DuelistAttack : Spells
{
    private GameState gameState;

    public DuelistAttack()
    {
        ChampionCard = true;
        ChampionCardType = ChampionCardType.Duelist;
    }

	protected override void PlaySpell()
    {
        gameState = GameState.Instance;
        gameState.CalculateAndDealDamage(Damage, this);
        gameState.StartCoroutine(WaitForOpponent());
    }

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => gameState.OpponentChampion.Champion.Health > 0);
        ListEnum lE = new ListEnum();
        lE.opponentChampions = true;      
        Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.SwitchChampionEnemy, this);
    }

    public void WaitForChoice()
    {
        Target = gameState.OpponentChampion.Champion;
        gameState.CalculateAndDealDamage(Damage, this);
    }
}
