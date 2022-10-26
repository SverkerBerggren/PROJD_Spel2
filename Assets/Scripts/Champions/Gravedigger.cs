using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Gravedigger", menuName = "Champion/Gravedigger", order = 1)]
public class Gravedigger : Champion
{
	public Gravedigger(Gravedigger c) : base(c.name, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect) { }

	public override void EndStep()
	{
		if (Graveyard.Instance.graveyardPlayer.Count == 0) return;
		base.EndStep();
		Card card = Graveyard.Instance.RandomizeCardFromGraveyard();
		GameState.Instance.DrawCard(1, card);
	}
}
