using UnityEngine;

[CreateAssetMenu(fileName = "Shanker", menuName = "Champion/Shanker", order = 1)]
public class Shanker : Champion
{
	public int AttackCardsToPlay = 3;
	public int CardsToDraw = 3;

    public override void EndStep()
	{
		base.EndStep();
		if (gameState.AttacksPlayedThisTurn >= AttackCardsToPlay)
			gameState.DrawCard(CardsToDraw, null);
	}

	public override void UpdatePassive()
	{
		if(gameState.OpponentChampion.Champion != this)
			PassiveEffect = gameState.AttacksPlayedThisTurn + "/" + AttackCardsToPlay + " attacks";
    }
}