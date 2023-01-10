using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Duelist", menuName = "Champion/Duelist", order = 1)]
public class Duelist : Champion
{
	private ListEnum lE = new ListEnum();
    public override void WhenCurrentChampion()
	{
		base.WhenCurrentChampion();
		lE.opponentChampions = true;
        if (gameState.IsOnline)
            Wait();
        else if (gameState.OpponentChampion != this)
            Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.SwitchChampionEnemy, null);
	}

    private void Wait()
    {
        gameState.StartCoroutine(WaitForOpponent());
    }

    private IEnumerator WaitForOpponent()
    {
        yield return new WaitUntil(() => gameState.OpponentChampion.Champion.Health > 0); // Waits until opponent has swapped champion before triggering choice
        Choice.Instance.ChoiceMenu(lE, 1, WhichMethod.SwitchChampionEnemy, null);
    }
}
