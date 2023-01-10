using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/QueensShield")]
public class QueensShield : Effects
{
	private Champion affectedChampion;
	public int ReturnDamage = 10;

	private void DealReturnDamage()
	{
		TargetInfo info = new TargetInfo();
		info.whichList.opponentChampions = true;
		info.index = 0;
		TargetAndAmount targetAndAmount = new TargetAndAmount(info, ReturnDamage);
		GameState.Instance.DealDamage(targetAndAmount);
	}

	public override void AddEffect()
	{
		gameState = GameState.Instance;
		affectedChampion = gameState.PlayerChampion.Champion;
	}

	public override void TakeDamage(int damage)
	{
		if (affectedChampion == gameState.PlayerChampion.Champion || affectedChampion == gameState.OpponentChampion.Champion)
		{
			DealReturnDamage();
		}
		base.TakeDamage(damage);
	}
}
