using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Duelist", menuName = "Champion/Duelist", order = 1)]
public class Duelist : Champion
{
	public Duelist(Duelist c) : base(c.name, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect) {}

	public override void WhenCurrentChampion()
	{
		base.WhenCurrentChampion();
		ListEnum lE = new ListEnum();
		lE.opponentChampions = true;
        if (gameState.isOnline)
        {
            Choise.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampion);
        }
        else
        {
            if (gameState.opponentChampion != this)
                Choise.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampion);
        }
	}
}
