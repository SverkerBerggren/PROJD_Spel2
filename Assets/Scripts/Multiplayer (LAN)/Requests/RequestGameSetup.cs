using System.Collections;
using System.Collections.Generic;


public class RequestGameSetup : ClientRequest
{
	public bool reciprocate = false;
	public List<CardAndAmount> opponentChampions = new List<CardAndAmount>();
	public int lobbyId = 0;
	public bool firstTurn = false;
	public List<string> deckList = new List<string>();
	public RequestGameSetup()
	{
		Type = 15;
	}
}
