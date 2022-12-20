using System.Collections;
using System.Collections.Generic;


public class RequestGameSetup : ClientRequest
{
	public bool reciprocate = false;
	public List<string> opponentChampions = new List<string>();
	public int lobbyId = 0;
	public bool firstTurn = false;
	public RequestGameSetup()
	{
		Type = 15;
	}
}
