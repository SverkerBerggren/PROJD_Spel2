using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionGameSetup : GameAction
{	
	public bool reciprocate = false;
	public List<string> opponentChampions = new List<string>();
	public GameActionGameSetup()
	{
		Type = 14;
	}
}
