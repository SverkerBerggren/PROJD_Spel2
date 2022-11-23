using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueensShield : Effects
{
	private int damageReturn = 10;

	public QueensShield(CardType cardType) : base(cardType, false){}

	public override void TakeDamage(int damage)
	{
		TargetInfo info = new TargetInfo();
		info.whichList.opponentChampions = true;
		info.index = 0;
		TargetAndAmount targetAndAmount = new TargetAndAmount(info, damageReturn);
		gameState.DealDamage(targetAndAmount);
		base.TakeDamage(damage);
	}
}
