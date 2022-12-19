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
        championCard = true;
        championCardType = ChampionCardType.Duelist;
    }

    public override void PlaySpell()
    {
        gameState = GameState.Instance;
        gameState.CalculateAndDealDamage(damage, this);
        gameState.StartCoroutine(WaitForOpponent());
    }

    public IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => gameState.opponentChampion.champion.health > 0);
        ListEnum lE = new ListEnum();
        lE.opponentChampions = true;      
        Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.SwitchChampionEnemy, this);
    }

    public void WaitForChoice()
    {
        Target = gameState.opponentChampion.champion;
        gameState.CalculateAndDealDamage(damage, this);
    }
}
