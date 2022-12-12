using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TheOneWhoDraws", menuName = "Champion/TheOneWhoDraws", order = 1)]
public class TheOneWhoDraws : Champion
{
	int cardsDraw = 1;
    public override void EndStep()
	{
		base.EndStep();
		gameState.DrawCard(1, null);
	}
}
