using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Duelist", menuName = "Champion/Duelist", order = 1)]
public class Duelist : Champion
{
	public Duelist(Duelist c) : base(c.name, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect) {}

	public override void WhenCurrentChampion()
	{
		base.WhenCurrentChampion();
		gameState.SwapActiveChampionEnemy(null);
		//Choose Opponent champion
	}
}
