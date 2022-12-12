using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/QueensShield")]
public class QueensShield : Effects
{
	private Champion affectedChampion;
	public int returnDamage = 10;

	private void DealReturnDamage()
	{
		TargetInfo info = new TargetInfo();
		info.whichList.opponentChampions = true;
		info.index = 0;
		TargetAndAmount targetAndAmount = new TargetAndAmount(info, returnDamage);
		GameState.Instance.DealDamage(targetAndAmount);
	}

	public override void AddEffect()
	{
		gameState = GameState.Instance;
		affectedChampion = gameState.playerChampion.champion;
	}

	public override void TakeDamage(int damage)
	{
		if (affectedChampion == gameState.playerChampion.champion || affectedChampion == gameState.opponentChampion.champion)
		{
			DealReturnDamage();
		}
		base.TakeDamage(damage);
	}
}
