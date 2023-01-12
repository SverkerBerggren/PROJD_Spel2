using System.Collections.Generic;

public class GameActionGameSetup : GameAction
{	
	public bool reciprocate = false;
	public List<string> opponentChampions = new List<string>();
	public bool firstTurn = false;
	public GameActionGameSetup()
	{
		Type = 14;
	}
}
