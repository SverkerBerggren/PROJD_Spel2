using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Duelist", menuName = "Champion/Duelist", order = 1)]
public class Duelist : Champion
{
	public Duelist(Duelist c) : base(c.name, c.health, c.maxHealth, c.shield, c.artwork, c.passiveEffect) {}

	public override void WhenCurrentChampion()
	{
		Debug.Log("desad");
		base.WhenCurrentChampion();
		ListEnum lE = new ListEnum();
		lE.opponentChampions = true;

        gameState.hasPriority = true;
        if (gameState.isOnline)
        {
            RequestPassPriority requestPassPriority = new RequestPassPriority(false);
            requestPassPriority.whichPlayer = ClientConnection.Instance.playerId;
            ClientConnection.Instance.AddRequest(requestPassPriority, gameState.RequestEmpty);
        }

        Choise.Instance.ChoiceMenu(lE, 1, WhichMethod.switchChampion);
		




		//Choose Opponent champion
	}
}
