using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestGameSetup : ClientRequest
{
	public bool reciprocate = false;
	public List<string> opponentChampions = new List<string>();
	public RequestGameSetup()
	{
		Type = 15;
	}
}