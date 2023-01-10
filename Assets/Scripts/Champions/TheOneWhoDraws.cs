using UnityEngine;

[CreateAssetMenu(fileName = "TheOneWhoDraws", menuName = "Champion/TheOneWhoDraws", order = 1)]
public class TheOneWhoDraws : Champion
{
	public int CardsDraw = 1;
    public override void EndStep()
	{
		base.EndStep();
		gameState.DrawCard(CardsDraw, null);
	}
}
