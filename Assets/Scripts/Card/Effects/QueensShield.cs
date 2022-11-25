using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/QueensShield")]
public class QueensShield : Effects
{
	public int damageReturn = 10;
	public override void TakeDamage(int damage)
	{
		TargetInfo info = new TargetInfo();
		info.whichList.opponentChampions = true;
		info.index = 0;
		TargetAndAmount targetAndAmount = new TargetAndAmount(info, damageReturn);
		GameState.Instance.DealDamage(targetAndAmount);
		base.TakeDamage(damage);
	}
}
