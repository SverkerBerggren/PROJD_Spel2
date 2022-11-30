using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Duelist", menuName = "Champion/Duelist", order = 1)]
public class Duelist : Champion
{
	public Duelist(Duelist c) : base(c.championName, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect, ChampionCardType.Duelist) {}

	public override void WhenCurrentChampion()
	{
		base.WhenCurrentChampion();
		ListEnum lE = new ListEnum();
		lE.opponentChampions = true;
        if (gameState.isOnline)
        {
            Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampionEnemy, null);
        }
        else
        {
            if (gameState.opponentChampion != this)
                Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampionEnemy, null);
        }
	}
}
