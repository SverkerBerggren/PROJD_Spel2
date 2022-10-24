using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TheOneWhoDraws", menuName = "Champion/TheOneWhoDraws", order = 1)]
public class TheOneWhoDraws : Champion
{
	int cardsDraw = 1;
	public TheOneWhoDraws(TheOneWhoDraws c) : base(c.name, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect)
	{
		cardsDraw = c.cardsDraw;
	}

	public override void EndStep()
	{
		base.EndStep();
		if (true /*player chooses if he want to draw more */)
		{
			//Choice: do the player want to draw cards???
		}
	}
}
