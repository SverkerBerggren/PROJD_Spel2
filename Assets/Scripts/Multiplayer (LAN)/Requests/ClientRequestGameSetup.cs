using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientRequestGameSetup : ClientRequest
{
	public bool reciprocate = false;
	public List<string> opponentChampions = new List<string>();
	public ClientRequestGameSetup()
	{
		Type = 15;
	}
}
