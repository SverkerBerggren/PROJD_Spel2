using UnityEngine;

[CreateAssetMenu(fileName = "Shanker", menuName = "Champion/Shanker", order = 1)]
public class Shanker : Champion
{
	public int AttackCardsToPlay = 3;
	public int CardsToDraw = 3;

    public override void EndStep()
	{
		base.EndStep();
		if (gameState.attacksPlayedThisTurn >= AttackCardsToPlay)
			gameState.DrawCard(CardsToDraw, null);
	}

	public override void UpdatePassive()
	{
		if(gameState.opponentChampion.Champion != this)
			PassiveEffect = gameState.attacksPlayedThisTurn + "/" + AttackCardsToPlay + " attacks";
    }
}